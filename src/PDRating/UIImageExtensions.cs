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
    public static class UIImageExtensions {
        /// <summary>
        /// Resizes the image proportionally when it is larger than the parent UIImageView. If it is smaller, the fallback content mode is used.
        /// </summary>
        public static void SetAspectFitAsNeeded(this UIImageView imageView, UIViewContentMode fallbackContentMode) {
            if (imageView == null) { return; }

            imageView.ContentMode = fallbackContentMode;
            if (imageView.Bounds.Width < imageView.Image.Size.Width || imageView.Bounds.Height < imageView.Image.Size.Height) {
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }
        }
    }
}