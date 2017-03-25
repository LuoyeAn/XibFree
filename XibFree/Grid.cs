using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreGraphics;

namespace XibFree
{
    public class Grid : UIView, ViewGroup.IHost
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="XibFree.UILayoutHost"/> class.
		/// </summary>
		/// <param name="layout">Root of the view hierarchy to be hosted by this layout host</param>
		public Grid(ViewGroup layout, CGRect frame) : base(frame)
		{
            this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            Layout = layout;
        }

        public Grid()
        {
            this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        }

        public Grid(ViewGroup layout)
        {
            this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            Layout = layout;
        }


        /// <summary>
        /// The ViewGroup declaring the layout to hosted
        /// </summary>
        /// <value>The ViewGroup.</value>
        public ViewGroup Layout
        {
            get
            {
                return _layout;
            }

            set
            {
                if (_layout != null)
                    _layout.SetHost(null);

                _layout = value;

                if (_layout != null)
                    _layout.SetHost(this);
            }
        }

        /// <summary>
        /// Finds the NativeView associated with a UIView
        /// </summary>
        /// <returns>The native view.</returns>
        /// <param name="view">View.</param>
        public NativeView FindNativeView(UIView view)
        {
            return _layout.FindNativeView(view);
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            if (_layout == null)
                return new CGSize(0, 0);

            // Measure the layout
            _layout.Measure(size.Width, size.Height);
            return _layout.GetMeasuredSize();
        }


        /// <Docs>Lays out subviews.</Docs>
        /// <summary>
        /// Called by iOS to update the layout of this view
        /// </summary>
        public override void LayoutSubviews()
        {
            if (_layout != null)
            {
                // Remeasure
                _layout.Measure(Bounds.Width, Bounds.Height);
                // Apply layout
                _layout.Layout(Bounds, false);
                if (DidLayoutAction != null)
                {
                    DidLayoutAction();
                }
            }
        }

        public Action DidLayoutAction { get; set; }

        #region IHost implementation

        /// <summary>
        /// Provide the hosting view
        /// </summary>
        UIView ViewGroup.IHost.GetUIView()
        {
            return this;
        }

        #endregion


        private ViewGroup _layout;
    }
}