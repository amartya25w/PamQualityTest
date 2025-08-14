using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconImageRecorder
{
    public enum ImageStorageType
    {
        Database = 0,
        FileSystem = 1
    }

    public enum ContentType
    {
        [Description("application/json")]
        Json = 0,
        [Description("application/x-www-form-urlencoded")]
        UrlEncoded = 1,
    }
}
