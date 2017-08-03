using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using XibFree;

namespace Demo.Views
{

    [Register("NestingScrollView")]
    public class NestingScrollView : UIViewController
    {
        public NestingScrollView()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }
        private LinearLayout Layout { get; set; }
        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            // Perform any additional setup after loading the view

            Layout = new LinearLayout(Orientation.Vertical)
            {
                SubViews = new View[]
                {
                    new FrameLayout
					{
                        SubViews=new View[]
                        {
                            new NativeView
                            {
                                View=new UITableView(),
                                LayoutParameters=new LayoutParameters(AutoSize.FillParent,AutoSize.FillParent),
                                Init=view=>
                                {
                                    var table=view.As<UITableView>();
                                    table.SeparatorStyle=UITableViewCellSeparatorStyle.None;
                                    table.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
                                    table.AccessibilityIdentifier="chatTable";
                                }
                            },
                            new NativeView
                            {
                                View=new UILayoutHostScrollable(new LinearLayout()
                                {
                                    LayoutParameters=new LayoutParameters(AutoSize.WrapContent,AutoSize.FillParent),
                                    SubViews=new View[]
                                    {
                                        #region SubViews
                                        new LinearLayout()
                                        {
                                            Layer=new CoreAnimation.CALayer
                                            {
                                                BorderWidth=1f,
                                                BorderColor = UIColor.LightGray.CGColor,
                                                BackgroundColor = UIColor.White.CGColor
                                            },
                                            Animate=true,
                                            AnimateDuration=1f,
                                            LayoutParameters=new LayoutParameters
                                            {
                                                Width=UIScreen.MainScreen.Bounds.Width-20,
                                                Height=AutoSize.WrapContent,
                                                MarginLeft=10,
                                                MarginRight=10,
                                                MarginBottom=10,
                                                Gravity= Gravity.Bottom
                                            },
                                            Padding=new UIEdgeInsets(1,0,1,0),
                                            SubViews=new View[]
                                            {
                                                new NativeView
                                                {
                                                    View=new UIButton(),
                                                    LayoutParameters=new LayoutParameters
                                                    {
                                                        Width=AutoSize.WrapContent,
                                                        Height=AutoSize.WrapContent,
                                                        Gravity=Gravity.CenterVertical
                                                    },
                                                    Init=view=>
                                                    {
                                                        var button=view.As<UIButton>();
                                                        button.SetTitle("add",UIControlState.Normal);
                                                        button.TouchUpInside+=Button_TouchUpInside;
                                                        button.ContentEdgeInsets=new UIEdgeInsets(10,10,10,0);
                                                        button.AccessibilityIdentifier="add";
                                                    }
                                                },
                                                new NativeView
                                                {
                                                    View=new UITextView(),
                                                    LayoutParameters=new LayoutParameters
                                                    {
                                                        Width=AutoSize.FillParent,
                                                        Height=AutoSize.WrapContent,
                                                        MaxHeight=200,
                                                        Gravity=Gravity.CenterVertical
                                                    },
                                                    Init=view=>
                                                    {
                                                        var textView=view.As<UITextView>();
                                                        //textView.AutocapitalizationType= UITextAutocapitalizationType.None;
                                                        textView.TranslatesAutoresizingMaskIntoConstraints = false;
                                                        textView.TextContainerInset = new UIEdgeInsets(4f, 2f, 4f, 2f);
                                                        textView.ContentInset = new UIEdgeInsets(1f, 0f, 1f, 0f);
                                                        textView.ScrollEnabled = true;
                                                        textView.UserInteractionEnabled = true;
                                                        textView.TextAlignment = UITextAlignment.Natural;
                                                        textView.ContentMode = UIViewContentMode.Redraw;
                                                        textView.Changed+=TextView_Changed;
                                                        textView.ScrollAnimationEnded+=TextView_ScrollAnimationEnded;
                                                        textView.LayoutManager.AllowsNonContiguousLayout=true;
                                                        textView.TextColor=UIColor.Red;
                                                        textView.AccessibilityIdentifier="sendMessage";

                                                        var placeHolderLabel=new UILabel();
                                                        placeHolderLabel.Text="Type a message...";
                                                        //placeHolderLabel.Font=textView.Font;
                                                        placeHolderLabel.SizeToFit();
                                                        textView.AddSubview(placeHolderLabel);
                                                        textView.SetValueForKey(placeHolderLabel,new NSString("_placeholderLabel"));
                                                    }
                                                },
                                                new NativeView
                                                {
                                                    View=new UIButton(),
                                                    LayoutParameters=new LayoutParameters
                                                    {
                                                        Width=AutoSize.WrapContent,
                                                        Height=AutoSize.WrapContent,
                                                        Gravity=Gravity.Bottom,
                                                        MarginLeft=5f
                                                    },
                                                    Init=view=>
                                                    {
                                                        var button=view.As<UIButton>();
                                                        button.SetTitle("Send",UIControlState.Normal);
                                                        button.SetTitleColor(UIColor.LightGray,UIControlState.Disabled);
                                                        button.SetTitleColor(UIColor.Blue,UIControlState.Normal);
                                                        button.ContentEdgeInsets=new UIEdgeInsets(10,10,10,10);
                                                        button.AccessibilityIdentifier="sendButton";
                                                    }
                                                },
                                            }
                                        },
										new NativeView
                                        {
                                            View=new UIButton(),
                                            LayoutParameters=new LayoutParameters
                                            {
                                                Width=AutoSize.WrapContent,
                                                Height=AutoSize.WrapContent,
                                                Gravity=Gravity.Bottom,
                                                MarginLeft=5f
                                            },
                                            Init=view=>
                                            {
                                                var button = view.As<UIButton>();
												button.SetTitle("Send",UIControlState.Normal);
                                                button.SetTitleColor(UIColor.LightGray,UIControlState.Disabled);
                                                button.SetTitleColor(UIColor.Blue,UIControlState.Normal);
                                                button.ContentEdgeInsets=new UIEdgeInsets(10,10,10,10);
												button.AccessibilityIdentifier="sendButton";
                                            }
                                        },
                                        #endregion
                                    }
                                }),//
                                Init=view=>
                                {
                                    var scrollView=view.As<UIScrollView>();
                                    scrollView.ShowsHorizontalScrollIndicator=false;
                                    scrollView.ShowsVerticalScrollIndicator=false;
                                    scrollView.BackgroundColor=UIColor.Red;
                                },
                                LayoutParameters=new LayoutParameters(AutoSize.FillParent,55)
                                {
                                    Gravity= Gravity.Bottom
                                },
                            },
                        }
                    },
               }
            };
            this.View = new UILayoutHost(Layout) { BackgroundColor =UIColor.White, AccessibilityIdentifier = this.GetType().Name };
        }

        private void TextView_ScrollAnimationEnded(object sender, EventArgs e)
        {
        }

        private void TextView_Changed(object sender, EventArgs e)
        {
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
        }
    }
}