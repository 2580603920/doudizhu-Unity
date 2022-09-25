using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class RegisterRequest : RequestBase
    {

        public RegisterRequest( ) : base() { }

        public override void Initial( )
        {
            requestCode = RequestCode.User;
            actionCode = ActionCode.Register;
            base.Initial();
        }
        public void Send( string username , string password )
        {
            MainPack pack = new MainPack();
            LoginPack loginPack = new LoginPack();
            loginPack.Username = username;
            loginPack.Password = password;
            pack.Loginpack = loginPack;
            pack.Requestcode = requestCode;
            pack.Actioncode = actionCode;
            Send(pack);
        }
    }
}