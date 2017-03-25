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
using CoreGraphics;
using UIKit;

namespace XibFree
{
	internal static class Extensions
	{
		/// <summary>
		/// Applies a set of UIEdgeInsets to a RectangleF
		/// </summary>
		/// <returns>The adjusted rectangle</returns>
		/// <param name="rect">The rectangle to be adjusted.</param>
		/// <param name="insets">The edge insets to be applied</param>
		public static CGRect ApplyInsets(this CGRect rect, UIEdgeInsets insets)
		{
			return new CGRect(rect.Left + insets.Left, rect.Top + insets.Top, rect.Width - insets.TotalWidth(), rect.Height- insets.TotalHeight());
		}

		public static nfloat TotalWidth(this UIEdgeInsets insets)
		{
			return insets.Left + insets.Right;
		}

		public static nfloat TotalHeight(this UIEdgeInsets insets)
		{
			return insets.Top + insets.Bottom;
		}

		public static CGRect ApplyGravity(this CGRect bounds, CGSize size, Gravity g)
		{
			nfloat left;
			switch (g & Gravity.HorizontalMask)
			{
				default:
					left = bounds.Left;
					break;

				case Gravity.Right:
					left = bounds.Right - size.Width;
					break;

				case Gravity.CenterHorizontal:
					left = (bounds.Left + bounds.Right - size.Width)/2;
					break;
			}

			nfloat top;
			switch (g & Gravity.VerticalMask)
			{
				default:
					top = bounds.Top;
					break;

				case Gravity.Bottom:
					top = bounds.Bottom - size.Height;
					break;

				case Gravity.CenterVertical:
					top = (bounds.Top + bounds.Bottom - size.Height)/2;
					break;
			}

			return new CGRect(left, top, size.Width, size.Height);
		}
	}
}

