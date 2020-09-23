using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace Launchpad
{
    public partial class Main : Form
    {
        private ChromiumWebBrowser Browser;


        public Main()
        {
            InitializeComponent(); // Initalizes WinForm Components

            // Initalizes ChromiumWebBrowser
            CefSettings Settings = new CefSettings();
            Cef.Initialize(Settings);

            Browser = new ChromiumWebBrowser(AppConfig.ServerAddress + "/updates-info");
            Browser.Dock = DockStyle.Fill;

            this.WebContainerPanel.Controls.Add(Browser);
        }

        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            if (Application.MessageLoop)
            {
                Application.Exit();
            }
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(AppConfig.Path + AppConfig.GameExe);
            }
            catch
            {
                MessageBox.Show("Unable to run game.");
            }
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            VersionControl.CheckForUpdate(true);
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            //VersionControl.CheckForUpdate();
            VersionControl.CheckForUpdate();
        }
    }
}
