using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class DealPoker_R : RequestBase
    {

        public DealPoker_R( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Game;
            actionCode = ActionCode.DealPoker;
            base.Initial();
        }
        public void Send( string username)
        {
            MainPack pack = new MainPack();
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            PlayerInfo player = new PlayerInfo();
            player.Username = username;
            pack.Playerinfo.Add(player);
            Send(pack);
        }
    }
}