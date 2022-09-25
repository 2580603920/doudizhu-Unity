using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class StartGameRquest : RequestBase
    {

        public StartGameRquest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Game;
            actionCode = ActionCode.StartGame;
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