using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using XibFree;

namespace Demo.Views
{
    [Register("RootView")]
    public class RootView : UIViewController
    {
        public RootView()
        {
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
            var contentLayout = new LinearLayout(Orientation.Vertical)
            {
                LayoutParameters=new LayoutParameters(AutoSize.FillParent, AutoSize.FillParent)
                {
                },
                SubViews=new View[]
                {
                    new LinearLayout(Orientation.Vertical)
                    {
                        Layer=new CoreAnimation.CALayer
                        {
                            BackgroundColor=UIColor.Gray.CGColor,
                            CornerRadius=20,
                            BorderColor=UIColor.Red.CGColor,
                            BorderWidth=4
                        },
                        LayoutParameters=new LayoutParameters(AutoSize.FillParent,50)
                        {
                            Margins=new UIEdgeInsets(20,20,20,20),
                        },
                        SubViews=new View[]
                        {
                            new NativeView
                            {
                                View =new UIButton(),
                                LayoutParameters=new LayoutParameters(AutoSize.FillParent,20),
                                Init=view=>
                                {
                                    var button=view.As<UIButton>();
                                    button.SetTitle("test button",UIControlState.Normal);
                                    button.BackgroundColor=UIColor.Green;
                                }
                            }
                        }
                    }
                }
            };
            View = new UILayoutHost(contentLayout) {BackgroundColor=UIColor.White };
            //View = new UIView(new CoreGraphics.CGRect(100, 100, 200, 200)) {BackgroundColor=UIColor.Purple };
        }
    }
}