using UIKit;

namespace PDRatingSample {
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