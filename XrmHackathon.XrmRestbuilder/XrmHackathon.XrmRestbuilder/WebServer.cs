using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace XrmHackathon.XrmRestbuilder
{
    internal class WebServer
    {
        private NameTable nt = new NameTable();
        private XmlNamespaceManager prefixes;
			

        private HttpListener _listener;

        private string _baseFolder;

        public string UriPrefix;
        
        private Dictionary<string, string> _packageIndex = new Dictionary<string, string>();
        private Guid _userId;
        private readonly IOrganizationService service;

        [method: CompilerGenerated]
        [CompilerGenerated]
        public event OnPageRequestArgs OnPageRequested;
        public delegate byte[] OnPageRequestArgs(string urlRequested);

        public WebServer(string baseFolder, IOrganizationService service)
        {
            string uriPrefix = "http://localhost:" + WebServer.FreeTcpPort().ToString() + "/";
            this.Init(uriPrefix, baseFolder);
            this.service = service;
        }

        public WebServer(string uriPrefix, string baseFolder)
        {
            this.Init(uriPrefix, baseFolder);
        }

        private void Init(string uriPrefix, string baseFolder)
        {
            ThreadPool.SetMaxThreads(50, 1000);
            ThreadPool.SetMinThreads(50, 50);
            this._listener = new HttpListener();
            this._listener.Prefixes.Add(uriPrefix);
            this._baseFolder = baseFolder;
            this.UriPrefix = uriPrefix;
         prefixes   = new XmlNamespaceManager(nt);
            //string format = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:d=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\"  xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\"><s:Body>{0}</s:Body></s:Envelope>";
            prefixes.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            prefixes.AddNamespace("a", "http://schemas.microsoft.com/xrm/2011/Contracts");
            prefixes.AddNamespace("d", "http://schemas.microsoft.com/xrm/2011/Contracts/Services");
            prefixes.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
            prefixes.AddNamespace("b", "http://schemas.datacontract.org/2004/07/System.Collections.Generic");
            prefixes.AddNamespace("p", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
            prefixes.PushScope();
            //string[] files = Directory.GetFiles(this._baseFolder + "\\RibbonWorkbench", "*.zip", SearchOption.TopDirectoryOnly);
            //if (files.Length != 0)
            //{
            //    string[] array = files;
            //    int i = 0;
            //    while (i < array.Length)
            //    {
            //        string text = array[i];
            //        string fileName = Path.GetFileName(text);
            //        if (fileName.StartsWith("RibbonWorkbench2016"))
            //        {
            //            this._solutionPackage = Package.Open(text, FileMode.Open, FileAccess.Read, FileShare.Read);
            //            using (IEnumerator<PackagePart> enumerator = this._solutionPackage.GetParts().GetEnumerator())
            //            {
            //                while (enumerator.MoveNext())
            //                {
            //                    string originalString = enumerator.Current.Uri.OriginalString;
            //                    if (originalString.StartsWith("/WebResources"))
            //                    {
            //                        string key = originalString.Substring(0, originalString.Length - 36);
            //                        this._packageIndex[key] = originalString;
            //                    }
            //                }
            //                goto IL_186;
            //            }
            //            goto IL_10B;
            //        }
            //        goto IL_10B;
            //    IL_186:
            //        i++;
            //        continue;
            //    IL_10B:
            //        if (fileName.StartsWith("_imgs"))
            //        {
            //            this._imagesPackage = ZipFile.Open(text, ZipArchiveMode.Read);
            //            foreach (ZipArchiveEntry current in this._imagesPackage.Entries)
            //            {
            //                string key2 = "/" + current.FullName.ToLower();
            //                this._packageIndex[key2] = current.FullName;
            //            }
            //            goto IL_186;
            //        }
            //        goto IL_186;
            //    }
            //}
        }

        public static int FreeTcpPort()
        {
            TcpListener expr_0B = new TcpListener(IPAddress.Loopback, 0);
            expr_0B.Start();
            int port = ((IPEndPoint)expr_0B.LocalEndpoint).Port;
            expr_0B.Stop();
            return port;
        }

        public void Start()
        {
            this._listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = this._listener.GetContext();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessRequest), context);
                    continue;
                }
                catch (HttpListenerException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                break;
            }
        }

        public void Stop()
        {
            this._listener.Stop();
            //if (this._solutionPackage != null)
            //{
            //    this._solutionPackage.Close();
            //}
            //this._imagesPackage.Dispose();
        }

        private void ProcessRequest(object listenerContext)
        {
            try
            {
                HttpListenerContext httpListenerContext = (HttpListenerContext)listenerContext;
                Path.GetFileName(httpListenerContext.Request.RawUrl);
                Path.GetFullPath(httpListenerContext.Request.RawUrl);

                byte[] array = new byte[0];
                string rawUrl = httpListenerContext.Request.RawUrl;
                string text = this._baseFolder.Substring(0, _baseFolder.Length - 1) + rawUrl.Replace("/", "\\");
                if (rawUrl.ToLower().EndsWith("clientglobalcontext.js.aspx"))
                {
                    array = this.GetClientGlobalContext();
                }
                else
                {
                    bool flag = false;
                    string str = rawUrl.Substring(14).Replace(".", "").Replace("/", "");
                    string key = "/WebResources/rwb_" + str;
                    
                    if (File.Exists(text))
                    {
                        httpListenerContext.Response.StatusCode = 200;
                        array = File.ReadAllBytes(text);
                    }
                    else
                    {
                        string documentContents;
                        using (Stream receiveStream = httpListenerContext.Request.InputStream)
                        {
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                            {
                                documentContents = readStream.ReadToEnd();
                            }
                        }
                        array = ProcessSoap(documentContents);

                        //        if (!flag4)
                        //        {
                        //httpListenerContext.Response.StatusCode = 404;
                        //array = Encoding.UTF8.GetBytes("HTTP 404");
                        //        }
                    }
                }
                //    if (this._solutionPackage != null && rawUrl.StartsWith("/WebResources/"))
                //    {
                //        Uri partUri = new Uri(this._packageIndex[key], UriKind.Relative);
                //        if (!this._solutionPackage.PartExists(partUri))
                //        {
                //            goto IL_21B;
                //        }
                //        PackagePart part = this._solutionPackage.GetPart(partUri);
                //        httpListenerContext.Response.StatusCode = 200;
                //        Package solutionPackage = this._solutionPackage;
                //        lock (solutionPackage)
                //        {
                //            using (Stream stream = part.GetStream())
                //            {
                //                array = new BinaryReader(stream).ReadBytes((int)stream.Length);
                //                flag = true;
                //            }
                //            goto IL_21B;
                //        }
                //    }
                //    if (this._imagesPackage != null && rawUrl.StartsWith("/_imgs") && this._packageIndex.ContainsKey(rawUrl.ToLower()))
                //    {
                //        ZipArchive imagesPackage = this._imagesPackage;
                //        lock (imagesPackage)
                //        {
                //            ZipArchiveEntry entry = this._imagesPackage.GetEntry(this._packageIndex[rawUrl.ToLower()]);
                //            ZipArchive imagesPackage2 = this._imagesPackage;
                //            lock (imagesPackage2)
                //            {
                //                if (entry != null)
                //                {
                //                    using (Stream stream2 = entry.Open())
                //                    {
                //                        array = new BinaryReader(stream2).ReadBytes((int)entry.Length);
                //                        flag = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //IL_21B:
                //    if (!flag)
                //    {
                //        bool flag4 = false;
                //        if (File.Exists(text))
                //        {
                //            httpListenerContext.Response.StatusCode = 200;
                //            array = File.ReadAllBytes(text);
                //            flag4 = true;
                //        }
                //        else if (this.OnPageRequested != null)
                //        {
                //            byte[] array2 = this.OnPageRequested(rawUrl);
                //            if (array2 != null)
                //            {
                //                array = array2;
                //                flag4 = true;
                //            }
                //        }
                //        if (!flag4)
                //        {
                //            httpListenerContext.Response.StatusCode = 404;
                //            array = Encoding.UTF8.GetBytes("HTTP 404");
                //        }
                //    }
                if (text.EndsWith(".css"))
                {
                    httpListenerContext.Response.ContentType = "text/css";
                }
                else if (text.EndsWith(".htm"))
                {
                    httpListenerContext.Response.ContentType = "text/html";
                }
                httpListenerContext.Response.ContentLength64 = (long)array.Length;
                using (Stream outputStream = httpListenerContext.Response.OutputStream)
                {
                    outputStream.Write(array, 0, array.Length);
                }
            }
            catch (Exception arg)
            {
                ((HttpListenerContext)listenerContext).Response.StatusCode = 500;
                using (Stream outputStream = ((HttpListenerContext)listenerContext).Response.OutputStream)
                {
                    byte[] array = new byte[0];
                    array = Encoding.UTF8.GetBytes("HTTP 500: "+arg);
                    outputStream.Write(array, 0, array.Length);
                }
                Console.WriteLine("HTTP 500: " + arg);
            }
        }

        private byte[] ProcessSoap(string documentContents)
        {
            XmlDocument expr_06 = new XmlDocument();
            expr_06.LoadXml(documentContents);

            string s = expr_06.SelectSingleNode("//d:request", prefixes).OuterXml
                .Replace("<request", "<a:OrganizationRequest")
                .Replace("<d:request", "<a:OrganizationRequest xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" ")
                .Replace("</request>", "</a:OrganizationRequest>")
                .Replace("</d:request>", "</a:OrganizationRequest>");

            DataContractSerializerSettings dataContractSerializerSettings = new DataContractSerializerSettings();
            dataContractSerializerSettings.KnownTypes = new Type[]
            {
                typeof(RetrieveEntityRequest),
                typeof(RetrieveVersionRequest),
                typeof(RetrieveMetadataChangesRequest),
                typeof(EntityQueryExpression)
            };
            OrganizationRequest organizationRequest = (OrganizationRequest)new DataContractSerializer(typeof(OrganizationRequest), 
                dataContractSerializerSettings).ReadObject(new XmlTextReader(new StringReader(s)));
            OrganizationResponse organizationResponse = null;
            organizationResponse = this.service.Execute(organizationRequest);
            StringWriter stringWriter = new StringWriter();
            DataContractSerializerSettings dataContractSerializerSettings2 = new DataContractSerializerSettings();
            XmlDictionary xmlDictionary = new XmlDictionary();
            dataContractSerializerSettings2.RootName = xmlDictionary.Add("ExecuteResult");
            dataContractSerializerSettings2.RootNamespace = xmlDictionary.Add("http://schemas.microsoft.com/xrm/2011/Contracts");
            dataContractSerializerSettings2.KnownTypes = new Type[]
            {
                typeof(RetrieveEntityResponse),
                typeof(RetrieveVersionResponse),
                typeof(RetrieveMetadataChangesResponse)
            };
            new DataContractSerializer(typeof(OrganizationResponse), dataContractSerializerSettings2).WriteObject(new XmlTextWriter(stringWriter), organizationResponse);
            string arg = stringWriter.ToString();
            return Encoding.UTF8.GetBytes(
                string.Format("<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body><ExecuteResponse xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services'>{0}</ExecuteResponse></s:Body></s:Envelope>", arg)

            );

        }

        private byte[] GetClientGlobalContext()
        {
            return Encoding.UTF8.GetBytes(string.Format("window.USER_GUID='\\x7b{0}\\x7d';\r\n                        window.USER_NAME='{1}';\r\n                        window.ORG_LANGUAGE_CODE={2};\r\n                        window.ORG_UNIQUE_NAME='{3}';\r\n                        window.USER_LANGUAGE_CODE={4};\r\n                        window.SERVER_URL='{5}';\r\n                        window.USE_XRM_BRIDGE = true;\r\n                        window.APP_VERSION = '{6}';\r\n                        ", new object[]
            {
                this._userId.ToString("D"),
                "",
                0,
                "",
                0,
                this.UriPrefix.TrimEnd(new char[]
                {
                    '/'
                }),
                "1.9"
            }));
        }
    }
}