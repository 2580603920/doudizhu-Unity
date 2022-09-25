using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
namespace Doudizhu
{
    public class GetRoomListRequest : RequestBase
    {

        public GetRoomListRequest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Room;
            actionCode = ActionCode.GetRoomList;
            base.Initial();
        }
        public void Send()
        {
            MainPack pack = new MainPack();
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            Send(pack);
        }
    }
}