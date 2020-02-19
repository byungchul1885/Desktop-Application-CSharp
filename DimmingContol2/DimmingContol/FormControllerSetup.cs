﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace DimmingContol
{
    public partial class FormControllerSetup : Form
    {
        public List<string> DimLevelValue { get; set; }
        public int ControllerIdx { get; set; }
        public string ControllerName { get; set; }

        public FormControllerSetup()
        {
            InitializeComponent();

            DimLevelValue = new List<string>();

            FormMain.DimmLevelValueReceived += DimmLevelValueRefresh;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DimmLevelSetup_Click(object sender, EventArgs e)
        {
            using (var form = new FormInputDimmLevel())
            {
                form.StartPosition = FormStartPosition.CenterParent;

                form.ControllerName = ControllerName;
                form.DimLevelValue.Clear();
                form.DimLevelValue.AddRange(DimLevelValue);

                form.ShowDialog();
            }
        }

        private void MaintenanceFactorSetup_Click(object sender, EventArgs e)
        {
            
        }

        private void FormControllerSetupDiaglog_Load(object sender, EventArgs e)
        {
            foreach (Control c in dimmLevelPanel.Controls)
            {
                if (c.GetType() == typeof(BunifuCustomLabel)
                    && c.Name.Contains("dimmLevelLabel"))
                {
                    int levelIndex = Int32.Parse(c.Name.Remove(0, "dimmLevelLabel".Length));
                    c.Text = DimLevelValue[levelIndex];
                }
            }

            titleLabel.Text = ControllerName + " 제어기 설정 상태";
        }

        private void DimmLevelValueRefresh(object sender, EventArgs e)
        {
            if (sender is List<string> li)
            {
                DimLevelValue.Clear();
                DimLevelValue.AddRange(li);

                foreach (Control c in dimmLevelPanel.Controls)
                {
                    if (c.GetType() == typeof(BunifuCustomLabel)
                        && c.Name.Contains("dimmLevelLabel"))
                    {
                        int levelIndex = Int32.Parse(c.Name.Remove(0, "dimmLevelLabel".Length));
                        c.Text = DimLevelValue[levelIndex];
                    }
                }
            }
        }


        
    }
}
