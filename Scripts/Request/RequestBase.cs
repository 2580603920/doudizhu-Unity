using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
using UnityEngine.Events;

namespace Doudizhu
{
    public class RequestBase
    {
        public RequestCode requestCode;
        public ActionCode actionCode;
        public RequestManager requestManager;
        public UnityAction<MainPack> ResponseAction;


        public RequestBase( ) 
        {
            Initial();
        }
        public virtual void Initial( ) 
        {
            
            requestManager =RequestManager.Instance;
            requestManager.RegisterRequset(actionCode , this);

        }
        public virtual void Send(MainPack pack ) 
        {
            requestManager.Send(pack);
        }
        public virtual void HandleResponse( MainPack pack ) 
        {

            ResponseAction(pack);
        }

    }
}
