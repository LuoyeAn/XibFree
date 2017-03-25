using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace XibFree
{
    public class UICustomLayoutHostScrollable : UIScrollView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XibFree.UILayoutHostScrollable"/> class.
        /// </summary>
        /// <param name="layout">Root of the view hierarchy to be hosted by this layout host</param>
        public UICustomLayoutHostScrollable(ViewGroup layout, CGRect frame) : base(frame)
        {
            _layoutHost = new UILayoutHost(layout);
            _layoutHost.AutoresizingMask = UIViewAutoresizing.None;
            this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            this.AddSubview(_layoutHost);
        }

        public UICustomLayoutHostScrollable() : this(null, CGRect.Empty)
        {
        }

        public UICustomLayoutHostScrollable(ViewGroup layout) : this(layout, CGRect.Empty)
        {
        }

        UILayoutHost _layoutHost;

        /// <summary>
        /// The ViewGroup declaring the layout to hosted
        /// </summary>
        /// <value>The ViewGroup.</value>
        public ViewGroup Layout
        {
            get
            {
                return _layoutHost.Layout;
            }

            set
            {
                _layoutHost.Layout = value;
                SetNeedsLayout();
            }
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            var newSize= _layoutHost.SizeThatFits(size);
            //newSize.Height= newSize.Height > Bounds.Height ? Bounds.Height : newSize.Height;
            return newSize;
        }


        /// <Docs>Lays out subviews.</Docs>
        /// <summary>
        /// Called by iOS to update the layout of this view
        /// </summary>
        public override void LayoutSubviews()
        {
            if (Layout != null)
            {
                // Remeasure the layout
                Layout.Measure(Bounds.Width, Bounds.Height);

                var size = Layout.GetMeasuredSize();
                //size.Height = size.Height > Bounds.Height ? Bounds.Height : size.Height;

                // Reposition the layout host
                _layoutHost.Frame = new CGRect(CGPoint.Empty, size);

                // Update the scroll view content
                ContentSize = size;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            var view = this as UIView;
            if (view != null)
            {
                view.EndEditing(true);
            }
        }
    }
}
