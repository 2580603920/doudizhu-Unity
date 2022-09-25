
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf.Collections;

namespace Doudizhu
{
    public class SendPoker_R : RequestBase
    {

        public SendPoker_R( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.Game;
            actionCode = ActionCode.SendPoker;
            base.Initial();
        }
        public void Send( string username , RepeatedField<SoketDoudizhuProtocol.Poker> pokers = null)
        {
            MainPack pack = new MainPack();
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            SendPokersInfo sInfo = new SendPokersInfo();
            sInfo.Username = username;
            if ( pokers != null ) 
            {
                foreach ( var item in pokers )
                {
                    sInfo.Poker.Add(item);
                }
            }
            pack.Sendpokersinfo.Add(sInfo);

            Send(pack);
        }
    }
}