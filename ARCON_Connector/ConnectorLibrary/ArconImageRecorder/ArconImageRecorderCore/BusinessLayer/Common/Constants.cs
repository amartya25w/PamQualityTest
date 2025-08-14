namespace ArconImageRecorderCore
{
    public class Constants
    {
        #region GDI32
        public const int SRCCOPY = 0x00CC0020;
        #endregion

        #region User32
        public const int WM_CLOSE = 0x0010;
        public const int WM_DESTROY = 0x0002;
        public const int WM_QUIT = 0x0012;
        public const int WM_ACTIVATEAPP = 0x1C;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_RBUTTONDOWN = 0x0204;

        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        public const uint EVENT_OBJECT_DESTROY = 0x8001;

        public const uint WINEVENT_OUTOFCONTEXT = 0;

        public const int RF_TESTMESSAGE = 0xA123;
      
        #endregion

        #region Others
        #endregion
    }
}
