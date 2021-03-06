﻿using com.clusterrr.FelLib;
using com.clusterrr.hakchi_gui.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace com.clusterrr.hakchi_gui
{
    public partial class WaitingClovershellForm : Form
    {
        public WaitingClovershellForm()
        {
            InitializeComponent();
            buttonDriver.Left = label6.Left + label6.Width;
            timer.Enabled = true;
        }

        public static bool WaitForDevice(IWin32Window owner)
        {
            if (DeviceExists()) return true;
            var form = new WaitingClovershellForm();
            form.ShowDialog(owner);
            return form.DialogResult == DialogResult.OK;
        }

        static bool DeviceExists()
        {
            return hakchi.Shell.IsOnline;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (DeviceExists())
            {
                DialogResult = DialogResult.OK;
                timer.Enabled = false;
            }
        }

        private void WaitingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DeviceExists())
            {
                if (Tasks.MessageForm.Show(Resources.AreYouSure, Resources.DoYouWantCancel, Resources.sign_warning, new Tasks.MessageForm.Button[] { Tasks.MessageForm.Button.Yes, Tasks.MessageForm.Button.No }, Tasks.MessageForm.DefaultButton.Button2) == Tasks.MessageForm.Button.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    DialogResult = DialogResult.Abort;
                }
            }
        }

        private void buttonDriver_Click(object sender, EventArgs e)
        {
            try
            {
                var process = new Process();
                var fileName = Path.Combine(Path.Combine(Program.BaseDirectoryInternal, "driver"), "nesmini_driver.exe");
                process.StartInfo.FileName = fileName;
                process.Start();
            }
            catch (Exception ex)
            {
                Tasks.ErrorForm.Show(this.Text, ex.Message, ex.StackTrace);
            }
        }

        private void WaitingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Enabled = false;
        }
    }
}


