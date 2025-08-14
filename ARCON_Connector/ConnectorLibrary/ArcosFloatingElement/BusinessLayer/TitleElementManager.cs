using ArconWinNativeAPI;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcosFloatingElement
{
    public class TitleElementManager
    {
        bool IsPanelVisible = true;
        User32.Rect WindowRec = new User32.Rect();
        private TitleElement _TitleElement;
        private Point _DragStart = Point.Empty;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _LogCode = "TEM", _MethodName = string.Empty;

        public void AddFloatingTitle(TitleElement objTitleElement)
        {
            if (_TitleElement == null)
            {
                InitialiseElements(objTitleElement);
                AddEventHandlers();
                AddControlsToProcessWindow();
            }
        }

        public void InitialiseElements(TitleElement objTitleElement)
        {
            _LogCode = "TEM:INE";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _TitleElement = objTitleElement;
                User32APIManager.GetWindowRect(_TitleElement.WindowHwnd, ref WindowRec);
                _TitleElement.WindowRec = WindowRec;
                _TitleElement.Logo = new PictureBox();
                _TitleElement.DragItem = new Panel();
                _TitleElement.TransParentPanel = new TransparentPanel();
                _TitleElement.PicChild = new PictureBox();
                _TitleElement.LblChild = new Label() { Text = objTitleElement.Title ?? "Arcon Connector" };
                ElementCSSManager.objTitleElement = _TitleElement;
                ElementCSSManager.SetTransPanelStyle(_TitleElement.TransParentPanel);
                ElementCSSManager.SetPanelStyle(_TitleElement.DragItem);
                ElementCSSManager.SetLogoStyle(_TitleElement.Logo);
                ElementCSSManager.SetLabelStyle(_TitleElement.LblChild);
                ElementCSSManager.SetPictureBoxStyle(_TitleElement.PicChild);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void AddEventHandlers()
        {
            _LogCode = "TEM:AEH";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _TitleElement.DragItem.MouseMove += new MouseEventHandler(dragItem_MouseMove);
                _TitleElement.DragItem.MouseDown += new MouseEventHandler(dragItem_MouseDown);
                _TitleElement.DragItem.MouseUp += new MouseEventHandler(dragItem_MouseUp);
                _TitleElement.LblChild.MouseMove += new MouseEventHandler(dragItem_MouseMove);
                _TitleElement.LblChild.MouseDown += new MouseEventHandler(dragItem_MouseDown);
                _TitleElement.LblChild.MouseUp += new MouseEventHandler(dragItem_MouseUp);
                _TitleElement.Logo.MouseMove += new MouseEventHandler(dragItem_MouseMove);
                _TitleElement.Logo.MouseDown += new MouseEventHandler(dragItem_MouseDown);
                _TitleElement.Logo.MouseUp += new MouseEventHandler(dragItem_MouseUp);
                _TitleElement.PicChild.Click += new EventHandler(picChild_Onclick);
                _TitleElement.TransParentPanel.MouseHover += new EventHandler(transParentPanel_MouseHover);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void AddControlsToProcessWindow()
        {
            _LogCode = "TEM:ACPW";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                User32APIManager.SetParent(_TitleElement.Logo.Handle, _TitleElement.DragItem.Handle);
                User32APIManager.SetParent(_TitleElement.LblChild.Handle, _TitleElement.DragItem.Handle);
                User32APIManager.SetParent(_TitleElement.PicChild.Handle, _TitleElement.DragItem.Handle);
                User32APIManager.SetParent(_TitleElement.TransParentPanel.Handle, _TitleElement.WindowHwnd);
                User32APIManager.SetParent(_TitleElement.DragItem.Handle, _TitleElement.WindowHwnd);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        #region DragItem Element Events
        private void dragItem_MouseMove(object sender, MouseEventArgs e)
        {
            _LogCode = "TEM:DIMM";
            try
            {
                if (_TitleElement.IsDragging)
                {
                    User32APIManager.GetWindowRect(_TitleElement.WindowHwnd, ref WindowRec);
                    var parentBounds = WindowRec;
                    Point endPoint = _TitleElement.DragItem.Location;
                    endPoint.X = Math.Max(0, endPoint.X + e.X - _DragStart.X);
                    endPoint.Y = 0;
                    if (endPoint.X + _TitleElement.DragItem.Width - parentBounds.left >= parentBounds.Width)
                        endPoint.X = parentBounds.Width - _TitleElement.DragItem.Width - 10;

                    _TitleElement.DragItem.Location = endPoint;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }

        private void dragItem_MouseDown(object sender, MouseEventArgs e)
        {
            _LogCode = "TEM:DIMD";
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    _TitleElement.IsDragging = true;
                    _DragStart = new Point(e.X, e.Y);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }

        private void dragItem_MouseUp(object sender, MouseEventArgs e)
        {
            _TitleElement.IsDragging = false;
        }
        #endregion

        #region BtnChild Element Events
        private void picChild_Onclick(object sender, EventArgs e)
        {
            _LogCode = "TEM:PCC";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _Log.Debug(_MethodName + " method picchild text : " + _TitleElement.PicChild.Text);
                if (_TitleElement.PicChild.Text == "Hide")
                {
                    User32APIManager.AnimateWindow(_TitleElement.DragItem.Handle, 800, Constants.AW_HIDE | Constants.AW_VER_NEGATIVE);
                    _TitleElement.PicChild.BackgroundImage = Resource1.pin_hor;
                    _TitleElement.PicChild.Text = "Show";
                    IsPanelVisible = false;
                }
                else
                {
                    User32APIManager.AnimateWindow(_TitleElement.DragItem.Handle, 800, Constants.AW_SLIDE | Constants.AW_VER_POSITIVE);
                    _TitleElement.PicChild.BackgroundImage = Resource1.pin_ver;
                    _TitleElement.PicChild.Text = "Hide";
                    IsPanelVisible = true;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }
        #endregion

        #region TransParentPanel Element Events
        private void transParentPanel_MouseHover(object sender, EventArgs e)
        {
            _LogCode = "TEM:TPMH";
            try
            {
                if (_TitleElement.PicChild.Text == "Show" && !IsPanelVisible)
                {
                    User32APIManager.AnimateWindow(_TitleElement.DragItem.Handle, 800, Constants.AW_SLIDE | Constants.AW_VER_POSITIVE);
                    IsPanelVisible = true;
                    HidePanelAfterInterval();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }

        private async void HidePanelAfterInterval(int ms = 5000)
        {
            _LogCode = "TEM:HPAI";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                await Task.Delay(ms);
                if (_TitleElement.PicChild.Text == "Show" && IsPanelVisible)
                    User32APIManager.AnimateWindow(_TitleElement.DragItem.Handle, 800, Constants.AW_HIDE | Constants.AW_VER_NEGATIVE);
                IsPanelVisible = false;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }
        #endregion

    }
}
