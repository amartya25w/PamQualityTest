using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcosFloatingElement
{
    public static class ScreenResolutionManager
    {
        private static ScreenResolution _ScreenResolution = new ScreenResolution();

        public static int CalculateRelativeWidth(int width)
        {
            return Math.Abs(width + (width * _ScreenResolution.RelativeWidthPercent / 100));
        }

        public static int CalculateRelativeHight(int height)
        {
            return Math.Abs(height + (height * _ScreenResolution.RelativeHeightPercent / 100));
        }
    }
}
