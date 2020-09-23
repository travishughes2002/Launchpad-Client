using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launchpad
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }

    public class AppConfig
    {
        private static string _ServerAddress = "http://launchpad.secure-servers.cloud/";
        private static string _GameExe = "BuildingGame.exe";

        public static string GameExe
        {
            get
            {
                return _GameExe;
            }
        }

        public static string ServerAddress {
            get
            {
                return _ServerAddress;
            }
        }

        public static string Path { 
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory; // Returns the path of the exacutable
            }
        }
    }
}
