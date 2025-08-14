namespace ArconWinNativeAPI
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

        public const int AW_SLIDE = 0X40000;
        public const int AW_HIDE = 0x00010000;
        public const int AW_HOR_POSITIVE = 0X1;
        public const int AW_HOR_NEGATIVE = 0X2;
        public const int AW_BLEND = 0X80000;
        public const int AW_VER_POSITIVE = 0x00000004;
        public const int AW_VER_NEGATIVE = 0x00000008;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        public const uint WM_COMMAND = 0x0111;
        public const int MIN_ALL = 419;
        public const int MIN_ALL_UNDO = 416;

        public const int GWL_STYLE = (-16);
        public const int WS_VISIBLE = 0x10000000;
        #endregion

        #region Others
        #endregion
    }
}
