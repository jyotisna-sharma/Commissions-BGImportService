using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
/*
// If you want to disable SSL certificate validation
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
*/

namespace MyAgencyVault.BusinessLibrary.Masters
{
    public delegate void ListCompleteDel(List<FileData> files, int statusCode);
    public delegate void UploadCompleteDel(int statusCode, object state);
    public delegate void DownloadCompleteDel(int statusCode);
    public delegate void CreateDirCompleteDel(int statusCode);
    public delegate void DeleteCompleteDel(int statusCode);
    public delegate void ErrorOccuredDel(Exception error);

    public class FileData
    {
        public string FileName { get; set; }
        public string LastModifiedDate { get; set; }
    }

    public class FileUtility
    {
        public event ListCompleteDel ListComplete;
        public event UploadCompleteDel UploadComplete;
        public event DownloadCompleteDel DownloadComplete;
        public event CreateDirCompleteDel CreateDirComplete;
        public event DeleteCompleteDel DeleteComplete;
        public event ErrorOccuredDel ErrorOccured;

        //XXX: submit along with state object.
        HttpWebRequest httpWebRequest;

        #region WebDAV connection parameters
        private String server;
        /// <summary>
        /// Specify the WebDAV hostname (required).
        /// </summary>
        /// 

        public static FileUtility CreateClient(string server,string UserName,string password,string domain)
        {
            FileUtility dav = new FileUtility();
            dav.Server = server;
            dav.User = UserName;
            dav.Pass = password;
            dav.Domain = domain;
            return dav;
        }

        public String Server
        {
            get { return server; }
            set
            {
                value = value.TrimEnd('/');
                server = value;
            }
        }
        private String basePath = "/";
        /// <summary>
        /// Specify the path of a WebDAV directory to use as 'root' (default: /)
        /// </summary>
        public String BasePath
        {
            get { return basePath; }
            set
            {
                value = value.Trim('/');
                basePath = "/" + value + "/";
            }
        }
        private int? port = null;
        /// <summary>
        /// Specify an port (default: null = auto-detect)
        /// </summary>
        public int? Port
        {
            get { return port; }
            set { port = value; }
        }
        private String user;
        /// <summary>
        /// Specify a username (optional)
        /// </summary>
        public String User
        {
            get { return user; }
            set { user = value; }
        }
        private String pass;
        /// <summary>
        /// Specify a password (optional)
        /// </summary>
        public String Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        private String domain = null;
        public String Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        Uri getServerUrl(String path, Boolean appendTrailingSlash)
        {
            String completePath = basePath;
            if (path != null)
            {
            	completePath += path.Trim('/');
            }

            if (appendTrailingSlash && completePath.EndsWith("/") == false) { completePath += '/'; }

            if(port.HasValue) {
				return new Uri(server + ":" + port + completePath);
            }
            else {
            	return new Uri(server + completePath);
            }
            
        }
        #endregion

        #region WebDAV operations
        /// <summary>
        /// List files in the root directory
        /// </summary>
        public void List()
        {
            // Set default depth to 1. This would prevent recursion (default is infinity).
            List("/", 1);
        }

        /// <summary>
        /// List files in the given directory
        /// </summary>
        /// <param name="path"></param>
        public void List(String path)
        {
            // Set default depth to 1. This would prevent recursion.
            List(path, 1);
        }

        /// <summary>
        /// List all files present on the server.
        /// </summary>
        /// <param name="remoteFilePath">List only files in this path</param>
        /// <param name="depth">Recursion depth</param>
        /// <returns>A list of files (entries without a trailing slash) and directories (entries with a trailing slash)</returns>
        public void List(String remoteFilePath, int? depth)
        {
            try
            {
                // Uri should end with a trailing slash
                Uri listUri = getServerUrl(remoteFilePath, true);

                // http://webdav.org/specs/rfc4918.html#METHOD_PROPFIND
                StringBuilder propfind = new StringBuilder();
                propfind.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                propfind.Append("<propfind xmlns=\"DAV:\">");
                propfind.Append("  <allprop/>");
                propfind.Append("</propfind>");

                // Depth header: http://webdav.org/specs/rfc4918.html#rfc.section.9.1.4
                IDictionary<string, string> headers = new Dictionary<string, string>();
                if (depth != null)
                {
                    headers.Add("Depth", depth.ToString());
                }

                AsyncCallback callback = new AsyncCallback(FinishList);
                HTTPRequest(listUri, "PROPFIND", headers, Encoding.UTF8.GetBytes(propfind.ToString()), null, callback, remoteFilePath);
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        void FinishList(IAsyncResult result)
        {
            try
            {
                string remoteFilePath = (string)result.AsyncState;
                int statusCode = 0;
                List<FileData> files = new List<FileData>();

                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(result))
                {
                    statusCode = (int)response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(stream);
                        XmlNamespaceManager xmlNsManager = new XmlNamespaceManager(xml.NameTable);
                        xmlNsManager.AddNamespace("d", "DAV:");

                        foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                        {
                            XmlNode xmlNode = node.SelectSingleNode("d:href", xmlNsManager);
                            XmlNode xmlNode1 = node.SelectSingleNode("descendant::d:getlastmodified", xmlNsManager);

                            string filepath = Uri.UnescapeDataString(xmlNode.InnerXml);
                            string fileDate = Uri.UnescapeDataString(xmlNode1.InnerXml);

                            FileData fileData = new FileData { FileName = filepath, LastModifiedDate = fileDate };

                            string[] file = filepath.Split(new string[1] { basePath }, 2, StringSplitOptions.RemoveEmptyEntries);
                            if (file.Length > 0)
                            {
                                // Want to see directory contents, not the directory itself.
                                if (file[file.Length - 1] == remoteFilePath || file[file.Length - 1] == server) { continue; }
                                files.Add(fileData);
                            }
                        }
                    }
                }

                if (ListComplete != null)
                {
                    ListComplete(files, statusCode);
                }
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }

        /// <summary>
        /// Upload a file to the server
        /// </summary>
        /// <param name="localFilePath">Local path and filename of the file to upload</param>
        /// <param name="remoteFilePath">Destination path and filename of the file on the server</param>
        public void Upload(String localFilePath, String remoteFilePath)
        {
            ActionLogger.Logger.WriteImportPolicyLog("Upload: " + localFilePath.ToString() , true);

            ActionLogger.Logger.WriteImportPolicyLog("Upload: " + remoteFilePath.ToString(), true);
            Upload(localFilePath, remoteFilePath, null);
        }

        /// <summary>
        /// Upload a file to the server
        /// </summary>
        /// <param name="localFilePath">Local path and filename of the file to upload</param>
        /// <param name="remoteFilePath">Destination path and filename of the file on the server</param>
        /// <param name="state">Object to pass along with the callback</param>
        public void Upload(String localFilePath, String remoteFilePath, object state)
        {
            try
            {
               // ActionLogger.Logger.WriteImportPolicyLog("Upload internal 1: " + localFilePath, true);

              //  ActionLogger.Logger.WriteImportPolicyLog("Upload internal 2: " + remoteFilePath, true);
                FileInfo fileInfo = new FileInfo(localFilePath);
                long fileSize = fileInfo.Length;

                Uri uploadUri = getServerUrl(remoteFilePath, false);
             //   ActionLogger.Logger.WriteImportPolicyLog("Upload internal 3: " + uploadUri, true);
                string method = WebRequestMethods.Http.Put.ToString();

                AsyncCallback callback = new AsyncCallback(FinishUpload);
                HTTPRequest(uploadUri, method, null, null, localFilePath, callback, state);

               
            }
            catch (System.Exception error)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Error uploading reprt: " + error.Message, true);
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        void FinishUpload(IAsyncResult result)
        {
            ActionLogger.Logger.WriteImportPolicyLog("FinishUpload: " + result.IsCompleted, true);
            try
            {
                int statusCode = 0;

                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(result))
                {
                    statusCode = (int)response.StatusCode;
                }

                if (UploadComplete != null)
                {
                    UploadComplete(statusCode, result.AsyncState);
                }
            }
            catch (System.Exception error)
            {
                ActionLogger.Logger.WriteImportPolicyLog("FinishUpload error: " + error.Message, true);
                if(error.InnerException != null)
                    {
                    ActionLogger.Logger.WriteImportPolicyLog("FinishUpload error: " + error.InnerException.Message, true);
                }
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        /// <summary>
        /// Download a file from the server
        /// </summary>
        /// <param name="remoteFilePath">Source path and filename of the file on the server</param>
        /// <param name="localFilePath">Destination path and filename of the file to download on the local filesystem</param>
        public void Download(String remoteFilePath, String localFilePath)
        {
            try
            {
                // Should not have a trailing slash.
                Uri downloadUri = getServerUrl(remoteFilePath, false);
                string method = WebRequestMethods.Http.Get.ToString();

                AsyncCallback callback = new AsyncCallback(FinishDownload);
                HTTPRequest(downloadUri, method, null, null, null, callback, localFilePath);
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        void FinishDownload(IAsyncResult result)
        {
            try
            {
                string localFilePath = (string)result.AsyncState;
                int statusCode = 0;

                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(result))
                {
                    statusCode = (int)response.StatusCode;
                    int contentLength = int.Parse(response.GetResponseHeader("Content-Length"));
                    using (Stream s = response.GetResponseStream())
                    {
                        using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] content = new byte[4096];
                            int bytesRead = 0;
                            do
                            {
                                bytesRead = s.Read(content, 0, content.Length);
                                fs.Write(content, 0, bytesRead);
                            } while (bytesRead > 0);
                        }
                    }
                }

                if (DownloadComplete != null)
                {
                    DownloadComplete(statusCode);
                }
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        /// <summary>
        /// Create a directory on the server
        /// </summary>
        /// <param name="remotePath">Destination path of the directory on the server</param>
        public void CreateDir(string remotePath)
        {
            try
            {
                // Should not have a trailing slash.
                Uri dirUri = getServerUrl(remotePath, false);

                string method = WebRequestMethods.Http.MkCol.ToString();

                AsyncCallback callback = new AsyncCallback(FinishCreateDir);
                HTTPRequest(dirUri, method, null, null, null, callback, null);
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        void FinishCreateDir(IAsyncResult result)
        {
            try
            {
                int statusCode = 0;

                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(result))
                {
                    statusCode = (int)response.StatusCode;
                }

                if (CreateDirComplete != null)
                {
                    CreateDirComplete(statusCode);
                }
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        /// <summary>
        /// Delete a file on the server
        /// </summary>
        /// <param name="remoteFilePath"></param>
        public void Delete(string remoteFilePath)
        {
            try
            {
                Uri delUri = getServerUrl(remoteFilePath, remoteFilePath.EndsWith("/"));

                AsyncCallback callback = new AsyncCallback(FinishDelete);
                HTTPRequest(delUri, "DELETE", null, null, null, callback, null);
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }


        void FinishDelete(IAsyncResult result)
        {
            try
            {
                int statusCode = 0;

                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(result))
                {
                    statusCode = (int)response.StatusCode;
                }

                if (DeleteComplete != null)
                {
                    DeleteComplete(statusCode);
                }
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }
        #endregion

        #region Server communication

        /// <summary>
        /// This class stores the request state of the request.
        /// </summary>
        public class RequestState
        {
            public WebRequest request;
            // The request either contains actual content...
            public byte[] content;
            // ...or a reference to the file to be added as content.
            public string uploadFilePath;
            // Callback and state to use after handling the request.
            public AsyncCallback userCallback;
            public object userState;
        }

        /// <summary>
        /// Perform the WebDAV call and fire the callback when finished.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestMethod"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <param name="uploadFilePath"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        void HTTPRequest(Uri uri, string requestMethod, IDictionary<string, string> headers, byte[] content, string uploadFilePath, AsyncCallback callback, object state)
        {
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                ActionLogger.Logger.WriteImportPolicyLog("httprequest 1: ", true);
                /*
                 * The following line fixes an authentication problem explained here:
                 * http://www.devnewsgroups.net/dotnetframework/t9525-http-protocol-violation-long.aspx
                 */
                System.Net.ServicePointManager.Expect100Continue = false;

                // If you want to disable SSL certificate validation
                /*
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslError)
                {
                        bool validationResult = true;
                        return validationResult;
                };
                */

                // The server may use authentication
                if (user != null && pass != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("httprequest 11: ", true);
                    NetworkCredential networkCredential;
                    if (domain != null)
                    {
                        networkCredential = new NetworkCredential(user, pass, domain);
                    }
                    else
                    {
                        networkCredential = new NetworkCredential(user, pass);
                    }
                    httpWebRequest.Credentials = networkCredential;
                    // Send authentication along with first request.
                    httpWebRequest.PreAuthenticate = true;
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog("httprequest 12: ", true);
                    //create a blank credential object so that if we have a previous one, it's now toast.
                    NetworkCredential networkCredential = new NetworkCredential();
                    httpWebRequest.Credentials = networkCredential;
                    httpWebRequest.PreAuthenticate = true;
                }

                httpWebRequest.Method = requestMethod;

                // Need to send along headers?
                if (headers != null)
                {
                    foreach (string key in headers.Keys)
                    {
                        httpWebRequest.Headers.Set(key, headers[key]);
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("httprequest headers set: ", true);
                // Need to send along content?
                if (content != null || uploadFilePath != null)
                {
                    RequestState asyncState = new RequestState();
                    asyncState.request = httpWebRequest;
                    asyncState.userCallback = callback;
                    asyncState.userState = state;

                    if (content != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("httprequest headers 3: ", true);
                        // The request either contains actual content...
                        httpWebRequest.ContentLength = content.Length;
                        asyncState.content = content;
                        httpWebRequest.ContentType = "text/xml";
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("httprequest headers 4: " + uploadFilePath, true);
                        // ...or a reference to the file to be added as content.
                        httpWebRequest.ContentLength = new FileInfo(uploadFilePath).Length;
                        asyncState.uploadFilePath = uploadFilePath;
                    }

                    // Perform asynchronous request.
                    IAsyncResult r = (IAsyncResult)asyncState.request.BeginGetRequestStream(new AsyncCallback(ReadCallback), asyncState);
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog("httprequest headers 5: ", true);
                    // Begin async communications
                    httpWebRequest.BeginGetResponse(callback, state);
                }
            }
            catch (System.Exception error)
            {
                ActionLogger.Logger.WriteImportPolicyLog("httprequest error: " + error.Message, true);
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }

        /// <summary>
        /// Submit data asynchronously
        /// </summary>
        /// <param name="result"></param>
        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                WebRequest request = state.request;

                // End the Asynchronus request.
                using (Stream streamResponse = request.EndGetRequestStream(result))
                {
                    // Submit content
                    if (state.content != null)
                    {
                        streamResponse.Write(state.content, 0, state.content.Length);
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(state.uploadFilePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] content = new byte[4096];
                            int bytesRead = 0;
                            do
                            {
                                bytesRead = fs.Read(content, 0, content.Length);
                                streamResponse.Write(content, 0, bytesRead);
                            } while (bytesRead > 0);

                            //XXX: perform upload status callback
                        }
                    }
                }

                // Done, invoke user callback
                request.BeginGetResponse(state.userCallback, state.userState);
            }
            catch (System.Exception error)
            {
                if (ErrorOccured != null)
                    ErrorOccured(error);
            }
        }
        #endregion
    }
}
