using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Xml;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml.Linq;

namespace MyAgencyVault.WcfService
{
    
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple,IncludeExceptionDetailInFaults=true)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,AddressFilterMode=AddressFilterMode.Any)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class MavService : IMavService, IErrorHandler
    {
        //public string GetData()
        //{
        //    return string.Format("You entered: {0}", "Hi");
        //}

        public NewVersionInfo GetServerURL(string appVersion)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " GetServerURL version : " + appVersion, true);
            NewVersionInfo obj = new NewVersionInfo();
            try
            {
                var url = ConfigurationManager.AppSettings["VersionUrl"].ToString();
                var builder = new StringBuilder();
                using (var stringWriter = new StringWriter(builder))
                {
                    using (var xmlReader = new XmlTextReader(url))
                    {
                        var doc = XDocument.Load(xmlReader);
                        //get versions.
                        var versions = from v in doc.Descendants("version")
                                       select new
                                       {
                                           // Name = v.Element("name").Value,
                                           Number = Convert.ToInt32(v.Element("number").Value),
                                           // URL = v.Element("url").Value,
                                           Date = Convert.ToDateTime(v.Element("date").Value)
                                       };

                        var version = versions.ToList()[0];
                        //check if latest version newer than current version.
                        //string serverUrl = "http://204.13.182.12/FileManager/Installers;Administrator;Comm!$$!onD3pt123;cdeptsql";
                        //   string[] CredentialsArray = serverUrl.Split(';'); //version.URL.Split(';');
                        //if (CredentialsArray.Length < 4)
                        //{
                        //    return strVersionUrl;
                        //}
                        if (version.Number > Convert.ToInt16(appVersion))
                        {
                            //strVersionUrl = version.URL;
                            obj.IsNewVersionRequired = true;
                            obj.ServerURL = ConfigurationManager.AppSettings["InstallerURL"]; // "http://204.13.182.12/FileManager/TestInstaller;Administrator;Comm!$$!onD3pt123;cdeptsql";
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " GetServerURL version found old ", true);
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " GetServerURL version found new ", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " GetServerURL exception : " + ex.Message, true);
            }
            return obj;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        #region IErrorHandler Members

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            System.Diagnostics.Debugger.Break();
            if (error is FaultException)
                return;
            var faultException = new FaultException("A General Error Occured");
            MessageFault messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, null);
        }

        #endregion
    }

    /*namespace WCFHeaderBehavior.Behavior
    {*/
        public class UsernameHeader : MessageHeader
        {
            #region Constants

            /// <summary>
            /// The name of the attribute to which the value of UserName is written
            /// </summary>
            public const string UserAttribute = "userName";

            /// <summary>
            /// The name of the custom message header.
            /// </summary>
            public const string MessageHeaderName = "UserName";

            /// <summary>
            /// The namespace of the custom message header.
            /// </summary>
            public const string MessageHeaderNamespace = "http://wcfheaderbehavior.com/services/username";

            #endregion

            #region Properties

            /// <summary>
            /// Gets the logon name of the user from which the request originated
            /// </summary>
            public string UserName { get; private set; }

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance
            /// </summary>
            /// <param name="userName">The logon name of the user from which the request originated</param>
            public UsernameHeader(string userName)
            {
                if (String.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                UserName = userName;
            }

            #endregion

            /// <summary>
            /// Gets the name of the message header.
            /// </summary>
            public override string Name
            {
                get { return MessageHeaderName; }
            }

            /// <summary>
            /// Gets the namespace of the message header.
            /// </summary>
            public override string Namespace
            {
                get { return MessageHeaderNamespace; }
            }

            /// <summary>
            /// Called when the header content is serialized using the specified XML writer.
            /// </summary>
            /// <param name="writer">An XmlDictionaryWriter.</param>
            /// <param name="messageVersion">Contains information related to the version of SOAP associated with a message and its exchange.</param>
            protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
            {
                writer.WriteAttributeString(UserAttribute, UserName);
            }
        }

        public class UsernameServiceBehavior : BehaviorExtensionElement, IServiceBehavior, IDispatchMessageInspector
        {
            /// <summary>
            /// Gets the UsernameHeader
            /// </summary>
            public UsernameHeader UsernameHeader { get; private set; }

            /// <summary>
            /// Creates a behavior extension based on the current configuration settings.
            /// </summary>
            /// <returns>
            /// The behavior extension.
            /// </returns>
            protected override object CreateBehavior()
            {
                return this;
            }

            /// <summary>
            /// Gets the type of behavior.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Type"/>.
            /// </returns>
            public override Type BehaviorType
            {
                get { return GetType(); }
            }

            /// <summary>
            /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
            /// </summary>
            /// <param name="serviceDescription">The service description.</param><param name="serviceHostBase">The service host that is currently being constructed.</param>
            public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
            }

            /// <summary>
            /// Provides the ability to pass custom data to binding elements to support the contract implementation.
            /// </summary>
            /// <param name="serviceDescription">The service description of the service.</param><param name="serviceHostBase">The host of the service.</param><param name="endpoints">The service endpoints.</param><param name="bindingParameters">Custom objects to which binding elements have access.</param>
            public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
            {
            }

            /// <summary>
            /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
            /// </summary>
            /// <param name="serviceDescription">The service description.</param><param name="serviceHostBase">The host that is currently being built.</param>
            public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
                foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
                {
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
                    }
                }
            }

            /// <summary>
            /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
            /// </summary>
            /// <returns>
            /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
            /// </returns>
            /// <param name="request">The request message.</param><param name="channel">The incoming channel.</param><param name="instanceContext">The current service instance.</param>
            public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            {
                string action = (request.Headers.Action != null) ? System.IO.Path.GetFileName(request.Headers.Action).ToLower() : "";

                
                // var action = request.Headers.Action.Substring(request.Headers.Action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                //string MethodNames = System.Configuration.ConfigurationSettings.AppSettings["MethodNamesToTrack"];
                //List<string> lstNames = MethodNames.Split(',').ToList<string>();
                //  ActionLogger.Logger.WriteImportPolicyLog("AfterReceiveRequest : " + action,true);
                //   if (action.ToLower().Contains("add") || action.ToLower().Contains("update") || action.ToLower().Contains("delete"))
                //
                // Find the index of the custom header
                // Will return -1 if the header was not send
                if (!string.IsNullOrEmpty(action) && action != "addlog" && (action.Contains("add") || action.Contains("update") || action.Contains("delete") || action.Contains("remove") || action.Contains("link") || action.Contains("post") || action.Contains("save"))) //(!action.Contains("get") && (!action.Contains("load")))) //
                {
                    string mailbody = "Request received on web service with blank UserID. Method name: " + action;
                    string devEmail = System.Configuration.ConfigurationManager.AppSettings["devEmail"];
                    try
                    {
                        var headerIndex = request.Headers.FindHeader(UsernameHeader.MessageHeaderName, UsernameHeader.MessageHeaderNamespace);
                        if (headerIndex >= 0)
                        {
                            // Parse the header and save
                            var reader = request.Headers.GetReaderAtHeader(headerIndex);
                            UsernameHeader = ParseHeader(reader);
                        }

                        if (UsernameHeader != null)
                        {
                            System.Threading.Tasks.Task.Factory.StartNew(() =>
                          {
                              ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Request received, Action: " + action + ", From user: " + UsernameHeader.UserName, true);
                          });
                            if (string.IsNullOrWhiteSpace(UsernameHeader.UserName) && (action == "deleteclients" || action == "deletepolicy"))
                            {

                                MyAgencyVault.BusinessLibrary.MailServerDetail.sendMail(devEmail, "Commission Alert! UserID received blank in request header", mailbody);
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " AfterReceiveRequest UserName Header is null , action:  " + action, true);
                                request = null;
                                return null;
                            }
                        }
                        else //if (action == "deleteclients" || action == "deletepolicy")
                        {

                            MyAgencyVault.BusinessLibrary.MailServerDetail.sendMail(devEmail, "Commission Alert! UserID received blank in request header", mailbody);
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " AfterReceiveRequest UserName Header is null , action:  " + action, true);
                            request = null;
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " AfterReceiveRequest exception : " + ex.Message, true);
                    }
                }
                // The return object is used to correlate state; it is passed to the BeforeSendReply method
                // We return null because we don't use it
                return null;
            }

            /// <summary>
            /// Called after the operation has returned but before the reply message is sent.
            /// </summary>
            /// <param name="reply">The reply message. This value is null if the operation is one way.</param><param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
            public void BeforeSendReply(ref Message reply, object correlationState)
            {
            }

            private static UsernameHeader ParseHeader(XmlDictionaryReader reader)
            {
                // Check reader position
                if (reader.IsStartElement(UsernameHeader.MessageHeaderName, UsernameHeader.MessageHeaderNamespace))
                {
                    // Parse the header
                    var originatingUser = reader.GetAttribute(UsernameHeader.UserAttribute);
                    if (String.IsNullOrEmpty(originatingUser))
                    {
                        throw new FaultException("No originating user provided", FaultCode.CreateSenderFaultCode(new FaultCode("ParseHeader")));
                    }

                    // Create new header
                    return new UsernameHeader(originatingUser);
                }

                return null;
            }
        }
    //}

        public class NewVersionInfo
        {
            public string ServerURL;
            public  bool IsNewVersionRequired;
        }
}
