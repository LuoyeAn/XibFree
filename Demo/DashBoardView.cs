using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using XibFree;

namespace Demo
{
    [Register("DashBoardView")]
    public class DashBoardView : UIViewController
    {
        public DashBoardView()
        {
            Title = "Demo";
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        { 
            base.ViewDidLoad();

            // Perform any additional setup after loading the view

            var contentLayout = new LinearLayout(Orientation.Horizontal)
            {
                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.FillParent)
            };

            View = new UILayoutHost(contentLayout) {BackgroundColor=UIColor.White };
        }
    }
}