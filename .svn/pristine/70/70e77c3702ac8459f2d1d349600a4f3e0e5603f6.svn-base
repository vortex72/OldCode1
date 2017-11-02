using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Message inspector for capturing ShipExec Request and response
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class MessageInspector : IClientMessageInspector
    {
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            ResponseBody = reply.ToString();
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            RequestBody = request.ToString();
            return null;
        }
    }

    /// <summary>
    /// Endpoint Behavior, for capturing ShipExec Request and response
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class InspectorBehavior : IEndpointBehavior
    {
        public MessageInspector Inspector { get; set; }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            Inspector = new MessageInspector();

            clientRuntime.ClientMessageInspectors.Add(Inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
           
        }

        public void Validate(ServiceEndpoint endpoint)
        {
           
        }
    }


}
