using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ArconImageRecorder
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

        public long? CompressionQualityLevel { get; set; } // 100 - Best Quality , 0 - Low quality

        public bool KeepImagesInMemory { get; set; }

        public List<ProcessDetails> ProcessName { get; set; }

        public List<ProcessDetails> ProcessTitle { get; set; }

    }
}
