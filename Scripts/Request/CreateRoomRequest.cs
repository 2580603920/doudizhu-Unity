using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
namespace Doudizhu
{
    public class CreateRoomRequest : RequestBase
    {

        public CreateRoomRequest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Room;
            actionCode = ActionCode.CreateRoom;
            base.Initial();
        }
        public void Send(string roomTitle )
        {
            MainPack pack = new MainPack();
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.Roomtile = roomTitle;
            pack.Roominfo.Add( roomInfo );
            Send(pack);
        }
    }
}