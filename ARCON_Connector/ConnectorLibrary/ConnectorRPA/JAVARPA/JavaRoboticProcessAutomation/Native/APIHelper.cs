using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JavaRoboticProcessAutomation
{
    public class APIHelper
    {
        public static dynamic WinAccessAPIManager
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                    return new StaticMembersDynamicWrapper(typeof(WinAccess64APIManager));
                else
                    return new StaticMembersDynamicWrapper(typeof(WinAccess32APIManager));
            }
        }
    }
}
