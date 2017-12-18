using System;
using System.ComponentModel;
using System.Windows;
using System.Xml;

namespace NxServerMonitor
{
    [RunInstaller(true)]
    public partial class InstallerSetup : System.Configuration.Install.Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

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
    }
}