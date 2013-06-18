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
        private Settings settings;
        private SwitchController controller;

        public MatrixControlBar()
        {
            this.settings = new Settings();
            this.settings.InputsChanged += new InputsChangedHandler(settings_InputsChanged);
            this.settings.PresetsChanged += new PresetsChangedHandler(settings_PresetsChanged);
            this.settings.OverridesChanged += new OverridesChangedHandler(settings_OverridesChanged);
            this.controller = new SwitchController(this.settings);

            InitializeComponent();

            // call these to make sure the comboboxes are filled in
            settings_PresetsChanged(this.settings);
            settings_InputsChanged(this.settings);

            overrideWarningTip.SetToolTip(this.presetLabel, "Overrides are active.  Right-click to configure");
        }

        void settingsMenuItem_Click(object sender, System.EventArgs e)
        {
            new SettingsDialog(this.settings).ShowDialog();
        }

        private void overridesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OverridesDialog(this.settings).ShowDialog();
        }

        void aboutMenuItem_Click(object sender, System.EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void settings_PresetsChanged(Settings settings)
        {
            PopulateComboBox(presetComboBox, settings.SelectedPreset, settings.Presets);
        }

        private void settings_InputsChanged(Settings settings)
        {
            PopulateComboBox(previewComboBox, settings.SelectedPreview, settings.Inputs);
        }

        private void settings_OverridesChanged(Settings settings)
        {
            bool overridesActive = false;
            foreach (int i in settings.Overrides)
            {
                if (i > 0)
                {
                    overridesActive = true;
                }
            }

            if (overridesActive)
            {
                this.presetLabel.BackColor = System.Drawing.Color.Red;
                overrideWarningTip.Active = true;
            }
            else
            {
                this.presetLabel.BackColor = System.Drawing.Color.Transparent;
                overrideWarningTip.Active = false;
            }
        }

        private static void PopulateComboBox(ComboBox comboBox, int selected, string[] items)
        {
            comboBox.Items.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    comboBox.Items.Add(new IndexedItem(items[i], i + 1));
                }
            }

            foreach (IndexedItem item in comboBox.Items)
            {
                if (item.ItemIndex == selected)
                {
                    comboBox.SelectedItem = item;
                }
            }
        }

        private void presetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexedItem item = this.presetComboBox.SelectedItem as IndexedItem;
            if (item != null)
            {
                this.settings.SelectedPreset = item.ItemIndex;
            }
        }

        private void previewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexedItem item = this.previewComboBox.SelectedItem as IndexedItem;
            if (item != null)
            {
                this.settings.SelectedPreview = item.ItemIndex;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        { 
            RECT rect = new RECT(pevent.ClipRectangle);
            IntPtr hdc = pevent.Graphics.GetHdc(); 
            int ret = ParentBackground.DrawThemeParentBackground(this.Handle, hdc, ref rect);
            pevent.Graphics.ReleaseHdc(hdc);
        }

    }
}


