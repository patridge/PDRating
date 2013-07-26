using System;
using System.Drawing;

namespace PDRatingSample {
    public static class SizeFExtensions {
        public static SizeF ScaleProportional(this SizeF original, float maxWidth, float maxHeight) {
            float ratioX = (float)maxWidth / original.Width;
            float ratioY = (float)maxHeight / original.Height;
            float ratio = Math.Min(ratioX, ratioY);

            float newWidth = (float)(original.Width * ratio);
            float newHeight = (float)(original.Height * ratio);
            return new SizeF(newWidth, newHeight);
        }
    }
}