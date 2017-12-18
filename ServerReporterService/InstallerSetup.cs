using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace ServerReporterService
{
    [RunInstaller(true)]
    public partial class InstallerSetup : System.Configuration.Install.Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                AddEndpointDetails();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured. Endpoint could not be replaced successfully.");
            }

            try
            {
                AddUserDetails();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured. User details could not be replaced successfully.");
            }

            try
            {
                AddDatabaseDetails();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured. Database details could not be replaced successfully.");
            }

            try
            {
                InstallService();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            UninstallService();
        }

        private void AddEndpointDetails()
        {
            try
            {
                string ENDPOINT = Context.Parameters["ENDPOINT"];
                string assemblypath = Context.Parameters["assemblypath"];
                string appConfigPath = assemblypath + ".config";

                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "configuration")
                        configuration = node;
                }
                if (configuration != null)
                {
                    XmlNode setting = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "system.serviceModel")
                            setting = node;
                    }

                    if (setting != null)
                    {
                        XmlNode settingNode = null;
                        foreach (XmlNode node in setting.ChildNodes)
                        {
                            if (node.Name == "client")
                                settingNode = node;
                        }

                        if (settingNode != null)
                        {
                            foreach (XmlNode node in settingNode.ChildNodes)
                            {
                                XmlAttribute attribute = node.Attributes["address"];
                                attribute.Value = ENDPOINT;
                                break;
                            }
                        }
                        doc.Save(appConfigPath);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void AddUserDetails()
        {
            try
            {
                string USERNAME = Context.Parameters["USERNAME"];
                string PASSWORD = Context.Parameters["PASSWORD"];
                string assemblypath = Context.Parameters["assemblypath"];
                string appConfigPath = assemblypath + ".config";

                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "configuration")
                        configuration = node;
                }
                if (configuration != null)
                {
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "appSettings")
                            settingNode = node;
                    }

                    if (settingNode != null)
                    {
                        foreach (XmlNode node in settingNode.ChildNodes)
                        {
                            XmlAttribute attribute = node.Attributes["value"];

                            if (node.Attributes["key"] != null)
                            {
                                switch (node.Attributes["key"].Value)
                                {
                                    case "userName":
                                        attribute.Value = USERNAME;
                                        break;
                                    case "password":
                                        attribute.Value = PASSWORD;
                                        break;
                                }
                            }
                        }
                    }
                    doc.Save(appConfigPath);
                }
            }
            catch
            {
                throw;
            }
        }

        private void AddDatabaseDetails()
        {
            try
            {
                string dataSource = Context.Parameters["DATASOURCE"];
                string DataSource = dataSource.Replace("\\\\", "\\");
                string InitialCatalog = Context.Parameters["INITIALCATALOG"];
                string assemblypath = Context.Parameters["assemblypath"];
                string appConfigPath = assemblypath + ".config";

                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "configuration")
                    {
                        configuration = node;
                        break;
                    }
                }
                if (configuration != null)
                {
                    XmlNode connectionNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "connectionStrings")
                        {
                            connectionNode = node;
                            break;
                        }
                    }
                    if (connectionNode != null)
                    {
                        foreach (XmlNode node in connectionNode.ChildNodes)
                        {
                            XmlAttribute attribute = node.Attributes["connectionString"];
                            string tekst = "Data Source=" + DataSource + ";Initial catalog=" + InitialCatalog + ";Integrated Security=True;";
                            attribute.Value = tekst;
                            break;
                        }
                    }
                }
                doc.Save(appConfigPath);
            }
            catch
            {
                throw;
            }
        }
        private void InstallService()
        {
            try
            {
                string admin = Context.Parameters["ADMINUSER"];
                string admin2 = admin.Replace("\\\\", "\\");
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c \"" + Context.Parameters["assemblyPath"] + "\" install -username:" + admin2 + " -password:" + Context.Parameters["ADMINPASSWORD"] + " --autostart";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UninstallService()
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c \"" + Context.Parameters["assemblyPath"] + "\" uninstall";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}