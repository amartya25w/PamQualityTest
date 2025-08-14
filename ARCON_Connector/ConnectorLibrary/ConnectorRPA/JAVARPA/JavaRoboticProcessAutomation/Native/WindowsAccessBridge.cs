using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JavaRoboticProcessAutomation
{
    public class WindowsAccessBridge
    {
        #region Structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct VersionInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            string VMversion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            string bridgeJavaClassVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            string bridgeJavaDLLVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            string bridgeWinDLLVersion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AccessibleContextInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string name; // the AccessibleName of the object
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string description; // the AccessibleDescription of the object

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string role; // localized AccesibleRole string
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string role_en_US; // AccesibleRole string in the en_US locale
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string states; // localized AccesibleStateSet string (comma separated)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string states_en_US; // AccesibleStateSet string in the en_US locale (comma separated)

            public Int32 indexInParent; // index of object in parent
            public Int32 childrenCount; // # of children, if any

            public Int32 x; // screen coords in pixels
            public Int32 y; // "
            public Int32 width; // pixel width of object
            public Int32 height; // pixel height of object

            public Boolean accessibleComponent; // flags for various additional
            public Boolean accessibleAction; // Java Accessibility interfaces
            public Boolean accessibleSelection; // FALSE if this object doesn't
            public Boolean accessibleText; // implement the additional interface
                                           // in question

            // BOOL accessibleValue; // old BOOL indicating whether AccessibleValue is supported
            public Boolean accessibleInterfaces; // new bitfield containing additional interface flags
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AccessibleTextInfo
        {
            public Int32 charCount; // # of characters in this text object
            public Int32 caretIndex; // index of caret
            public Int32 indexAtPoint;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AccessibleTextItemsInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public String letter;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String word;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public String sentence;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AccessibleTextAttributesInfo
        {
            public Boolean bold;
            public Boolean italic;
            public Boolean underline;
            public Boolean strikethrough;
            public Boolean superscript;
            public Boolean subscript;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String backgroundColor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String foregroundColor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String fontFamily;
            public Int32 fontSize;

            public Int32 alignment;
            public Int32 bidiLevel;

            public Single firstLineIndent;
            public Single leftIndent;
            public Single rightIndent;
            public Single lineSpacing;
            public Single spaceAbove;
            public Single spaceBelow;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public String fullAttributesString;
        }

        #endregion Structures
    }
}
