using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Ionic.Zip;

namespace Launchpad
{
    public class VersionControl
    {
        private static UpdateMeta Meta = new UpdateMeta();
        private static GenericLoadingForm LoadingForm = new GenericLoadingForm();

        private static string LocalVersionFile
        {
            get
            {
                string FileLocation = AppConfig.Path + "version.txt";
                GetMetaInfo();

                if (File.Exists(FileLocation) == false)
                {
                    File.Create(FileLocation);
                }

                return FileLocation;
            }    
        }

        private static int LocalVersion
        {
            get
            {
                int VersionData;

                try
                {
                    VersionData = Int16.Parse(File.ReadAllText(LocalVersionFile));
                }
                catch
                {
                    GetMetaInfo();
                    File.WriteAllText(LocalVersionFile, Meta.Versions[0].ID.ToString());
                    VersionData = Meta.Versions[0].ID;
                }

                return VersionData;
            }
        }


        // This gets the meta data from the specifide server in the config and converts the received json to standard varibles
        public static void GetMetaInfo()
        {
            try
            {
                WebClient Client = new WebClient();
                Stream Data = Client.OpenRead(AppConfig.ServerAddress + "/meta.json");
                StreamReader Reader = new StreamReader(Data);
                string WebData = Reader.ReadToEnd();
                Reader.Close();

                Meta = JsonConvert.DeserializeObject<UpdateMeta>(WebData);
            }
            catch
            {
                MessageBox.Show("Unable to get information.");
            }
        }

        public static void CheckForUpdate(bool IsBackground = false)
        {
            // Note: IsBackground defines if this method should display the up to date message or not.

            GetMetaInfo();

            if (File.Exists(LocalVersionFile) == false)
            {
                File.Create(LocalVersionFile);
                File.WriteAllText(LocalVersionFile, Meta.Versions[0].ID.ToString());
            }

            if (Meta.Versions[0].ID > LocalVersion)
            {
                DialogResult Result;
                Result = MessageBox.Show("A new update has been released. Would you like to update?", "New Update", MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes)
                {
                    ChangeVersion(0);
                }
            }
            else
            {
                if (IsBackground == false)
                {
                    MessageBox.Show("Game is up to date.", "No update", MessageBoxButtons.OK);
                }
            }
        }

        public static void ChangeVersion(int Version)
        {
            WebClient Client = new WebClient();

            LoadingForm.Show();
            LoadingForm.Text = "Updating";

            try
            {
                Client.DownloadFile(Meta.Versions[Version].DownloadURL, AppConfig.Path + "package.zip");

                ZipFile Package = ZipFile.Read("package.zip");
                foreach (ZipEntry Entery in Package)
                {
                    Entery.Extract(AppConfig.Path, ExtractExistingFileAction.OverwriteSilently);
                }


                File.WriteAllText(LocalVersionFile, Meta.Versions[Version].ID.ToString());


                LoadingForm.Hide();
                MessageBox.Show("Update Succsessful", "Succsessful");
            }
            catch(Exception e)
            {
                LoadingForm.Hide();
                MessageBox.Show("Unable to install update. Exception thrown: " + e, "Error", MessageBoxButtons.OK);
            }
        }
    }


    public class UpdateMeta
    {
        public List<Versions> Versions;
        public List<Branches> Branches;
    }

    public class Versions
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "download_url")]
        public string DownloadURL { get; set; }
    }

    public class Branches
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
