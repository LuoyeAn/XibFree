//  XibFree - http://www.toptensoftware.com/xibfree/
//
//  Copyright 2013  Copyright © 2013 Topten Software. All Rights Reserved
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace XibFree
{
	/// <summary>
	/// UILayoutHostScrollable is the native UIView that hosts that XibFree layout
	/// </summary>
	public class UILayoutHostScrollable : UIScrollView
	{
        private CGRect _frame;
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.UILayoutHostScrollable"/> class.
		/// </summary>
		/// <param name="layout">Root of the view hierarchy to be hosted by this layout host</param>
		public UILayoutHostScrollable(ViewGroup layout, CGRect frame) : base(frame)
		{
			_layoutHost = new UILayoutHost(layout);
			_layoutHost.AutoresizingMask = UIViewAutoresizing.None;
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			this.AddSubview(_layoutHost);
            _frame = frame;
        }

		public UILayoutHostScrollable() : this(null, CGRect.Empty)
		{
		}

		public UILayoutHostScrollable(ViewGroup layout) : this(layout, CGRect.Empty)
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
            if (_frame != null)
                return _frame.Size;
			var newSize= _layoutHost.SizeThatFits(size);
            return newSize;
		}


		/// <Docs>Lays out subviews.</Docs>
		/// <summary>
		/// Called by iOS to update the layout of this view
		/// </summary>
		public override void LayoutSubviews()
		{
			if (Layout!=null)
			{
				// Remeasure the layout
				Layout.Measure(Bounds.Width, Bounds.Height);

				var size = Layout.GetMeasuredSize();
                
				// Reposition the layout host
				_layoutHost.Frame = new CGRect(CGPoint.Empty, size);

                Layout.Layout(Bounds, false);

				// Update the scroll view content
				ContentSize = size;
			}
		}

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            var view = this as UIView;
            view?.EndEditing(true);
        }
    }
}

