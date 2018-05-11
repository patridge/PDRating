using System;
using CoreGraphics;

namespace PatridgeDev
{
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