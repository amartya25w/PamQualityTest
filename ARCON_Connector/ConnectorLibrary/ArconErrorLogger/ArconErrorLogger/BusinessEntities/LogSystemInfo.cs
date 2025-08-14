using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconErrorLogger.BusinessEntities
{
    #region Enum
    public enum OSVersionType
    {
        Unknown = 0,
        Windows95 = 1,
        Windows98 = 2,
        Windows98SE = 3,
        WindowsMe = 4,
        WindowsNT351 = 5,
        WindowsNT40 = 6,
        Windows2000 = 7,
        WindowsXP = 8,
        WindowsVista = 9,
        Windows7 = 10,
        Windows8 = 11
    }

    public enum OSBitType
    {
        Unknown = 0,
        X86 = 1,
        X64 = 2
    }
    #endregion Enum

    public class LogSystemInfo
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public OSVersionType OSVersionType { get { return GetOSVersionType(); } }
        public OSBitType OSBitType { get { return GetOSBitType(); } }

        public static OSVersionType GetOSVersionType()
        {
            try
            {
                OSVersionType osVersion = OSVersionType.Unknown;
                OperatingSystem os = Environment.OSVersion;
                Version vs = os.Version;

                if (os.Platform == PlatformID.Win32Windows)
                {
                    //This is a pre-NT version of Windows
                    switch (vs.Minor)
                    {
                        case 0:
                            osVersion = OSVersionType.Windows95;
                            break;
                        case 10:
                            osVersion = vs.Revision.ToString() == "2222A" ? OSVersionType.Windows98SE : OSVersionType.Windows98;
                            break;
                        case 90:
                            osVersion = OSVersionType.WindowsMe;
                            break;
                        default:
                            break;
                    }
                }
                else if (os.Platform == PlatformID.Win32NT)
                {
                    switch (vs.Major)
                    {
                        case 3:
                            osVersion = OSVersionType.WindowsNT351;
                            break;
                        case 4:
                            osVersion = OSVersionType.WindowsNT40;
                            break;
                        case 5:
                            osVersion = vs.Minor == 0 ? OSVersionType.Windows2000 : OSVersionType.WindowsXP;
                            break;
                        case 6:
                            if (vs.Minor == 0)
                                osVersion = OSVersionType.WindowsVista;
                            else if (vs.Minor == 1)
                                osVersion = OSVersionType.Windows7;
                            else
                                osVersion = OSVersionType.Windows8;
                            break;
                        default:
                            break;
                    }
                }
                return osVersion;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static OSBitType GetOSBitType()
        {
            try
            {
                if (Environment.Is64BitOperatingSystem)
                    return OSBitType.X64;
                return OSBitType.X86;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
