using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MatrixControl
{
    class ParentBackground
    {
        [DllImport("uxtheme", ExactSpelling = true)]
        public extern static Int32 DrawThemeParentBackground(IntPtr hWnd, IntPtr hdc, ref RECT pRect);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public Int32 Left;
        public Int32 Top;
        public Int32 Right;
        public Int32 Bottom;

        public int Width
        {
            get
            {
                return Right - Left;
            }
        }

        public int Height
        {
            get
            {
                return Bottom - Top;
            }
        }

        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left; this.Top = top; this.Right = right; this.Bottom = bottom;
        }

        public RECT(Rectangle rc)
        {
            Left = rc.Left; Top = rc.Top; Right = rc.Left + rc.Width; Bottom = rc.Top + rc.Height;
        }

        public static implicit operator RECT(Rectangle rc)
        {
            return new RECT(rc.Left, rc.Top, rc.Left + rc.Width, rc.Top + rc.Height);
        }

        public static implicit operator Rectangle(RECT rc)
        {
            return new Rectangle(rc.Left, rc.Top, rc.Left + rc.Width, rc.Top + rc.Height);
        }

        public override string ToString()
        {        // TODO: change this to use String.Format?        
            return "{ " + Left + ", " + Top + ", " + Right + ", " + Bottom + " }";
        }
    }

}
