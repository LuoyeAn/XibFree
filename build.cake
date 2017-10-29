#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=gitlink&version=2.4.0
#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.ConsoleRunner
#addin nuget:?package=Cake.Incubator&version=1.5.0
#addin nuget:?package=Cake.Git&version=0.16.0

var sln = new FilePath(“XibFree.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
	CleanDirectories(outputDir.FullPath);

	EnsureDirectoryExists(outputDir);
});

GitVersion versionInfo = null;
Task("Version").Does(() => {
	versionInfo = GitVersion(new GitVersionSettings {
		UpdateAssemblyInfo = true,
		OutputType = GitVersionOutput.Json
	});

	Information("GitVersion -> {0}", versionInfo.Dump());
});

Task("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Version")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(versionInfo.FullBuildMetaData);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
	.Does(() => 
{
	var vsLatest = VSWhereLatest();
	msBuildPath = (vsLatest == null)
		? null
		: vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
});

Task("Restore")
	.IsDependentOn("ResolveBuildTools")
	.Does(() => {
	NuGetRestore(sln, new NuGetRestoreSettings {
		Verbosity = NuGetVerbosity.Quiet
	});
	// MSBuild(sln, settings => settings.WithTarget("Restore"));
});

Task("Build")
	.IsDependentOn("ResolveBuildTools")
	.IsDependentOn("Clean")
	.IsDependentOn("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Restore")
	.Does(() =>  {

	var settings = new MSBuildSettings 
	{
		Configuration = "Release",
		ToolPath = msBuildPath,
		Verbosity = Verbosity.Minimal
	};

	settings.Properties.Add("DebugSymbols", new List<string> { "True" });
	settings.Properties.Add("DebugType", new List<string> { "Full" });

	MSBuild(sln, settings);
});

RunTarget(target);

bool IsMasterOrReleases()
{
	if (StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "master"))
		return true;

	if (versionInfo.BranchName.Contains("releases/"))
		return true;

	return false;
}

bool IsRepository(string repoName)
{
	if (isRunningOnAppVeyor)
	{
		var buildEnvRepoName = AppVeyor.Environment.Repository.Name;
		Information("Checking repo name: {0} against build repo name: {1}", repoName, buildEnvRepoName);
		return StringComparer.OrdinalIgnoreCase.Equals(repoName, buildEnvRepoName);
	}
	else
	{
		try
		{
			var path = MakeAbsolute(sln).GetDirectory().FullPath;
			using (var repo = new LibGit2Sharp.Repository(path))
			{
				var origin = repo.Network.Remotes.FirstOrDefault(
					r => r.Name.ToLowerInvariant() == "origin");
				return origin.Url.ToLowerInvariant() == 
					"https://github.com/" + repoName.ToLowerInvariant();
			}
		}
		catch(Exception ex)
		{
			Information("Failed to lookup repository: {0}", ex);
			return false;
		}
	}
}

bool IsTagged()
{
	var path = MakeAbsolute(sln).GetDirectory().FullPath;
	using (var repo = new LibGit2Sharp.Repository(path))
	{
		var head = repo.Head;
		var headSha = head.Tip.Sha;
		
		var tag = repo.Tags.FirstOrDefault(t => t.Target.Sha == headSha);
		if (tag == null)
		{
			Information("HEAD is not tagged");
			return false;
		}

		Information("HEAD is tagged: {0}", tag.FriendlyName);
		return true;
	}
}

Tuple<string, string> GetNugetKeyAndSource()
{
	var apiKeyKey = string.Empty;
	var sourceKey = string.Empty;
	if (isRunningOnAppVeyor)
	{
		apiKeyKey = "NUGET_APIKEY";
		sourceKey = "NUGET_SOURCE";
	}
	else
	{
		if (StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "develop"))
		{
			apiKeyKey = "NUGET_APIKEY_DEVELOP";
			sourceKey = "NUGET_SOURCE_DEVELOP";
		}
		else if (IsMasterOrReleases())
		{
			apiKeyKey = "NUGET_APIKEY_MASTER";
			sourceKey = "NUGET_SOURCE_MASTER";
		}
	}

	var apiKey = EnvironmentVariable(apiKeyKey);
	if (string.IsNullOrEmpty(apiKey))
		throw new Exception(string.Format("The {0} environment variable is not defined.", apiKeyKey));

	var source = EnvironmentVariable(sourceKey);
	if (string.IsNullOrEmpty(source))
		throw new Exception(string.Format("The {0} environment variable is not defined.", sourceKey));

	return Tuple.Create(apiKey, source);
}
