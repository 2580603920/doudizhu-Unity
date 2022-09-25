using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class RobDizhu_R : RequestBase
    {

        public RobDizhu_R( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Game;
            actionCode = ActionCode.RobHost;
            base.Initial();
        }
        public void Send( string username , bool isRob )
        {
            MainPack pack = new MainPack();
            if ( isRob )
                pack.Returncode = ReturnCode.Success;
            else
                pack.Returncode = ReturnCode.Fail;
            pack.Requestcode = requestCode;
            PlayerInfo playerInfo  = new PlayerInfo();
            playerInfo.Username = username;
            pack.Playerinfo.Add(playerInfo); 
            pack.Actioncode = actionCode;
            Send(pack);
        }
    }
}