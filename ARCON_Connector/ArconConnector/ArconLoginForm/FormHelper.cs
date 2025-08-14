using log4net;
using System;
using System.Reflection;

namespace ArconConnector.Forms
{
    public static class FormHelper
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GenerateRandomDigits(int value)
        {
            string result = string.Empty;
            try
            {
                Random rnd = new Random(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + +DateTime.Now.Millisecond);
                for (int i = 1; i <= value; i++)
                    result = result + rnd.Next(1, 9) + " ";
                return result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
