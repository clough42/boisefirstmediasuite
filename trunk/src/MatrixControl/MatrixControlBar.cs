using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BandObjectLib;
using System.Runtime.InteropServices;

namespace MatrixControl
{
    [Guid("123568A6-43E0-11DE-A2FC-CBA955D89593")]
    [BandObject("Matrix Control Bar", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "Shows bar that says hello.")]
    public partial class MatrixControlBar : BandObject
    {
        public MatrixControlBar()
        {
            InitializeComponent();
        }


        void settingsMenuItem_Click(object sender, System.EventArgs e)
        {
            new Settings().Show();
        }

    }
}


