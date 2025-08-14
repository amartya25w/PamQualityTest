using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JavaRoboticProcessAutomation
{
    public class WinAccess32APIManager
    {
        public const string WinAccessBridgeDll = "WindowsAccessBridge-32.dll";
        public const Int32 MAX_STRING_SIZE = 1024;
        public const Int32 SHORT_STRING_SIZE = 256;

        #region Event Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
            [MarshalAs(UnmanagedType.LPWStr)] string property, [MarshalAs(UnmanagedType.LPWStr)] string oldValue, [MarshalAs(UnmanagedType.LPWStr)] string newValue);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JavaShutDownDelegate(System.Int32 vmID);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FocusGainedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FocusLostDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CaretUpdateDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MouseClickedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MouseEnteredDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MouseExitedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MousePressedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MouseReleasedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MenuCanceledDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MenuDeselectedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MenuSelectedDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PopupMenuCanceledDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PopupMenuWillBecomeInvisibleDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PopupMenuWillBecomeVisibleDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyNameChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
            [MarshalAs(UnmanagedType.LPWStr)] string oldName, [MarshalAs(UnmanagedType.LPWStr)] string newName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyDescriptionChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
            [MarshalAs(UnmanagedType.LPWStr)] string oldDescription, [MarshalAs(UnmanagedType.LPWStr)] string newDescription);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyStateChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
            [MarshalAs(UnmanagedType.LPWStr)] string oldState, [MarshalAs(UnmanagedType.LPWStr)] string newState);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyValueChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
            [MarshalAs(UnmanagedType.LPWStr)] string oldValue, [MarshalAs(UnmanagedType.LPWStr)] string newValue);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertySelectionChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyTextChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyCaretChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
        Int32 oldPosition, Int32 newPosition);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyVisibleDataChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyChildChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
        IntPtr oldChild, IntPtr newChild);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyActiveDescendentChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
        IntPtr oldActiveDescendent, IntPtr newActiveDescendent);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PropertyTableModelChangeDelegate(System.Int32 vmID, IntPtr jevent, IntPtr ac,
        IntPtr oldValue, IntPtr newValue);

        #endregion Event Delegates

        #region EventDLLImport
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMouseClickedFP(MouseClickedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMouseEnteredFP(MouseEnteredDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMouseExitedFP(MouseExitedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMousePressedFP(MousePressedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMouseReleasedFP(MouseReleasedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setFocusGainedFP(FocusGainedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setFocusLostFP(FocusLostDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setCaretUpdateFP(CaretUpdateDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setJavaShutdownFP(JavaShutDownDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMenuCanceledFP(MenuCanceledDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMenuDeselectedFP(MenuDeselectedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setMenuSelectedFP(MenuSelectedDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPopupMenuCanceledFP(PopupMenuCanceledDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPopupMenuWillBecomeInvisibleFP(PopupMenuWillBecomeInvisibleDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPopupMenuWillBecomeVisibleFP(PopupMenuWillBecomeVisibleDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyActiveDescendentChangeFP(PropertyActiveDescendentChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyCaretChangeFP(PropertyCaretChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyChangeFP(PropertyChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyChildChangeFP(PropertyChildChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyDescriptionChangeFP(PropertyDescriptionChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyNameChangeFP(PropertyNameChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertySelectionChangeFP(PropertySelectionChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyStateChangeFP(PropertyStateChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyTableModelChangeFP(PropertyTableModelChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyTextChangeFP(PropertyTextChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyValueChangeFP(PropertyValueChangeDelegate fp);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setPropertyVisibleDataChangeFP(PropertyVisibleDataChangeDelegate fp);

        //Inits the JAB.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void Windows_run();

        //Checks if window is JavaWindow.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Int32 isJavaWindow(IntPtr hwnd);

        //Releases the specified java object.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void releaseJavaObject(Int32 vmID, IntPtr javaObject);

        //Sets the text of the given accessible context.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void setTextContents(Int32 vmID, IntPtr ac, [MarshalAs(UnmanagedType.LPWStr)] string text);

        //Gets basic version info about JVM/JAB
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static void getVersionInfo(Int32 vmID, IntPtr versionInfo);

        //Gets the next java window, where the hwnd passed is the previous window.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe IntPtr getNextJavaWindow(IntPtr hwnd);

        //Returns ac from window handle.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleContextFromHWND(IntPtr hwnd, out Int32 vmID, out IntPtr ac);

        //Returns handle from ac
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe IntPtr getHWNDFromAccessibleContext(Int32 vmID, IntPtr ac);

        //Compares two objects
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean isSameObject(Int32 vmID, IntPtr ac1, IntPtr ac2);

        //Returns an AccessibleContext object that represents the point given, offset by window coordinates.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleContextAt(Int32 vmID, IntPtr acparent, Int32 x, Int32 y, out IntPtr ac);

        //Returns an AccessibleContext object that represents the nth child of the object ac, where n is specified by the value index.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe IntPtr getAccessibleChildFromContext(Int32 vmID, IntPtr ac, Int32 index);

        //Returns an AccessibleContext object that represents the window with focus.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleContextWithFocus(void* window, out Int32 vmID, out IntPtr ac);

        //Returns an AccessibleContext object that represents the parent of the specified object.
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe IntPtr getAccessibleParentFromContext(Int32 vmID, IntPtr ac);

        //Returns detailed information about an AccessibleContext object belonging to the JVM
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleContextInfo(Int32 vmID, IntPtr accessibleContext, IntPtr acInfo);

        //**ACCESSIBLE TEXT FUNCTIONS
        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleTextInfo(Int32 vmID, IntPtr AccessibleContext, IntPtr textInfo, Int32 x, Int32 y);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleTextItems(Int32 vmID, IntPtr AccessibleContext, IntPtr textItems, Int32 index);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean getAccessibleTextAttributes(Int32 vmID, IntPtr AccessibleContext, Int32 index, IntPtr attributes);

        [DllImport(WinAccessBridgeDll, SetLastError = true, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public extern static unsafe Boolean GetAccessibleTextSelectionInfo(Int32 vmID, IntPtr AccessibleContext, Int32 index, IntPtr textSelection);

        #endregion EventDLLImport

    }
}
