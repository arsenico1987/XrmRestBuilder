using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.IO;

namespace XrmHackathon.XrmRestbuilder
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;

        public CrmConnection CrmConnection { get; private set; }
        internal WebServer Webserver { get; private set; }

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAccounts);
        }

        private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            this.CrmConnection = new CrmConnection(newService);
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
            //this._appHost = new CRMSolutionAppHost(base.get_Service(), appRoot);
            //this.browser.ObjectForScripting = this._appHost.SparkleXrmBridge;
            
            this.browser.ObjectForScripting = this.CrmConnection;
            //this.browser.Navigate(@"D:\Projects\GitHub\XrmRestBuilder\XrmHackathon.XrmRestbuilder\XrmHackathon.XrmRestbuilder\webresources\lat_\CRMRESTBuilder\Xrm.RESTBuilder.htm");
            string appRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            this.Webserver = new WebServer(appRoot, newService);
            Task.Run(delegate
            {
                this.Webserver.Start();
            });
            this.browser.Navigate(GetUrl("webresources/lat_/CRMRESTBuilder/Xrm.RESTBuilder.htm"));
            return;
        }
        public Uri GetUrl(string relativeUrl)
        {
            return new Uri(this.Webserver.UriPrefix + relativeUrl);
        }

        public string Test(string message)
        {
            return "hello " + message;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.browser.Navigate(@"D:\Projects\GitHub\XrmRestBuilder\XrmHackathon.XrmRestbuilder\XrmHackathon.XrmRestbuilder\webresources\lat_\CRMRESTBuilder\Xrm.RESTBuilder.htm");

        }
    }
}