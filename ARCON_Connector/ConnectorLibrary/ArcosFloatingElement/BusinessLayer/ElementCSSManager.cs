using System.Drawing;
using System.Windows.Forms;

namespace ArcosFloatingElement
{
    public static class ElementCSSManager
    {
        readonly static Color blueColor = ColorTranslator.FromHtml("#3C8DBC");
        public static TitleElement objTitleElement;

        public static void SetPanelStyle(Panel objPanel)
        {
            if (objPanel != null)
            {
                objPanel.Left = 0;
                objPanel.Top = 0;
                objPanel.Width = 500;
                objPanel.Height = 30;
                objPanel.BackColor = blueColor;
            }
        }

        public static void SetTransPanelStyle(TransparentPanel objPanel)
        {
            if (objPanel != null)
            {
                objPanel.Width = objTitleElement.WindowRec.Width;
                objPanel.Height = 2;
            }
        }

        public static void SetLogoStyle(PictureBox objPictureBox)
        {
            if (objPictureBox != null)
            {
                objPictureBox.Top = 1;
                objPictureBox.Location = new Point((objTitleElement.DragItem.Left + 7), 7);
                objPictureBox.Size = new Size(17, 15);
                objPictureBox.BackColor = blueColor;
                objPictureBox.BackgroundImage = Resource1.logo;
            }
        }

        public static void SetLabelStyle(Label objLabel)
        {
            if (objLabel != null)
            {
                objLabel.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold); ;
                objLabel.BackColor = blueColor;//
                objLabel.Width = objTitleElement.DragItem.Width - 100;
                objLabel.Location = new Point(25, 9);
                objLabel.ForeColor = Color.White;
            }
        }

        public static void SetPictureBoxStyle(PictureBox objPictureBox)
        {
            if (objPictureBox != null)
            {
                objPictureBox.Text = "Hide";
                Resource1 resource1 = new Resource1();
                objPictureBox.Size = new Size(14, 14);
                objPictureBox.Location = new Point(objTitleElement.DragItem.Width - 20, 7);
                objPictureBox.BackColor = blueColor;
                objPictureBox.BackgroundImage = Resource1.pin_ver;
            }
        }
    }
}
