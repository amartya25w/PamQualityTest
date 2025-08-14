using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconRealTimeSessionMonitor
{
    public class Enum
    {
        public enum SizeConstants 
        {
            HORZRES = 8,
            VERTRES = 10,
            SRCCOPY = 0xCC0020,
            SRCINVERT = 0x660046,
            USE_SCREEN_WIDTH = -1,
            USE_SCREEN_HEIGHT = -1
        }

        public enum RealTimeState
        {
            None = 0,
            Active = 1,
            Freezed = 2,
            Expired = 3
        }
    }
}
