using System;

#if __UNIFIED__
using UIKit;
using Foundation;
using CoreGraphics;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

using System.Drawing;
using CGRect = global::System.Drawing.RectangleF;
using CGPoint = global::System.Drawing.PointF;
using CGSize = global::System.Drawing.SizeF;
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace PatridgeDev {
    public static class SizeFExtensions {
        public static CGSize ScaleProportional(this CGSize original, nfloat maxWidth, nfloat maxHeight) {
            nfloat ratioX = (float)maxWidth / original.Width;
            nfloat ratioY = (float)maxHeight / original.Height;
            nfloat ratio = (nfloat)Math.Min(ratioX, ratioY);

            nfloat newWidth = (nfloat)(original.Width * ratio);
            nfloat newHeight = (nfloat)(original.Height * ratio);
            return new CGSize(newWidth, newHeight);
        }
    }
}