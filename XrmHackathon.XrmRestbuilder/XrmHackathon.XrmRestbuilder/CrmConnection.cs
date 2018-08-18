using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XrmHackathon.XrmRestbuilder
{
    [ComVisible(true)]
    public class CrmConnection
    {
        public XrmPage Page { get; set; }

        public IOrganizationService Service { get; }

        public CrmConnection(IOrganizationService service)
        {
            Service = service;
        }
        public string LoadVersion()
        {
            int i = 0;

            return "10982";
        }
        public void Test(String message)
        {
            MessageBox.Show(message, "client code");
        }

        public void execute(dynamic request, string callback, string errorhandler)
        {
            int i = 1;
        }

        public int[] getUserLcid()
        {
            return new int[] { 1033 };
        }

        public void GetAllEntityMetadata(object callback, string errorhandler)
        {

            RetrieveAllEntitiesResponse r = Service.Execute(new
               Microsoft.Xrm.Sdk.Messages.RetrieveAllEntitiesRequest
            {
                EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.All,
                RetrieveAsIfPublished = true
            }) as RetrieveAllEntitiesResponse;
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(r.EntityMetadata
                .Select(x => new { x.SchemaName, x.EntitySetName, x.LogicalName, x.ObjectTypeCode, x.DisplayName }));


            Action<string> asynccallback = delegate (string response)
            {
                callback.GetType().InvokeMember(string.Empty, BindingFlags.Instance
                    | BindingFlags.Public | BindingFlags.InvokeMethod, null, callback, new object[]
                {
                    result
                });
            };

            Task.Run(delegate
            {
                this.Process(asynccallback, result);
            });
        }
        public void RetrieveMetadataChangesRequest(string name, object callback, string errorhandler)
        {
            RetrieveMetadataChangesResponse r = Service.Execute(new
              Microsoft.Xrm.Sdk.Messages.RetrieveMetadataChangesRequest
            {
                Query = new Microsoft.Xrm.Sdk.Metadata.Query.EntityQueryExpression()
                {
                    Criteria = new Microsoft.Xrm.Sdk.Metadata.Query.MetadataFilterExpression
                    {

                    }
                }

            }) as RetrieveMetadataChangesResponse;
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(r.EntityMetadata
                .Select(x => new { x.SchemaName, x.EntitySetName, x.LogicalName, x.ObjectTypeCode, x.DisplayName }));


            Action<string> asynccallback = delegate (string response)
            {
                callback.GetType().InvokeMember(string.Empty, BindingFlags.Instance
                    | BindingFlags.Public | BindingFlags.InvokeMethod, null, callback, new object[]
                {
                    result
                });
            };

            Task.Run(delegate
            {
                this.Process(asynccallback, result);
            });
        }

        private void Process(Action<string> asynccallback, string r)
        {
            asynccallback(r);
        }
    }
}