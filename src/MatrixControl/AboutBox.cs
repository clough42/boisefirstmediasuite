﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MatrixControl
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            this.versionLabel.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
