using ArconWinNativeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcosFloatingElement
{
    public class TitleElement
    {
        public string Title { get; set; }

        public Panel DragItem { get; set; }

        public IntPtr WindowHwnd { get; set; }

        public User32.Rect WindowRec { get; set; }

        public bool IsDragging { get; set; }

        public Label LblChild { get; set; }

        public PictureBox PicChild { get; set; }

        public TransparentPanel TransParentPanel { get; set; }

        public PictureBox Logo { get; set; }

    }
}
