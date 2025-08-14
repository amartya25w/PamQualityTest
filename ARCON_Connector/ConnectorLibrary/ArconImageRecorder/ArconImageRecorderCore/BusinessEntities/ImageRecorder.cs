using System;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace ArconImageRecorderCore
{
    public class ImageRecorder 
    {
        public ImageRecorder()
        {
            ProcessDetails = new List<ProcessDetails>();
        }

        public List<ProcessDetails> ProcessDetails { get; set; }

        public IntPtr LastWindowHandle { get; set; }

        public ImageStorageType ImageStorageType { get; set; }

        public string ImagePath { get; set; }

        public ImageFormat ImageFormat { get; set; }

        public Guid SessionId { get; set; }

        public int TimeInterval { get; set; }
    }
}
