using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
namespace Doudizhu
{
    public class JoinRoomRequest : RequestBase
    {

        public JoinRoomRequest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Room;
            actionCode = ActionCode.JoinRoom;
            base.Initial();
        }
        public void Send( string roomID )
        {
            MainPack pack = new MainPack();
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.Roomid = roomID;
            pack.Roominfo.Add( roomInfo );
            Send(pack);
        }
    }
}