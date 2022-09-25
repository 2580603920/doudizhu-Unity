using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class GetGameInfoR : RequestBase
    {

        public GetGameInfoR( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Game;
            actionCode = ActionCode.GetGameInfo;
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