using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
namespace Doudizhu
{
    public class ExitRoomRequest : RequestBase
    {

        public ExitRoomRequest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Room;
            actionCode = ActionCode.ExitRoom;
            base.Initial();
        }
        public void Send( string roomID )
        {
            MainPack pack = new MainPack();
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.Roomid = roomID;
            pack.Roominfo.Add(roomInfo);
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            Send(pack);
        }
    }
}