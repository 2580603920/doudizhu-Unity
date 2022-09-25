using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu { 
  
     public class RegisterCTRL : PanelCTRLBase
    {
        RegisterRequest request;

        public override void Initial( )
        {
            base.Initial();
            BindEvent();
            request = RequestManager.Instance.GetRequest(ActionCode.Register) as RegisterRequest; 
            request.ResponseAction += HandleResponse;
        }
        public override void Enter( )
        {
            base.Enter();
            transform.parent.gameObject.SetActive(true);
        }
        public override void Exit( )
        {
            base.Exit();
            transform.parent.gameObject.SetActive(false);
        }
        public override void HandleResponse( MainPack pack )
        {
            base.HandleResponse(pack);
            Loom.QueueOnMainThread(( a ) =>
            {
                if ( ( pack as MainPack ).Returncode == ReturnCode.Success )
                {

                    Pop();
                    ShowMessage("×¢²á³É¹¦");
                }
                else ShowMessage("×¢²áÊ§°Ü");

            } , pack);
          
        }
        void BindEvent( )
        {
            GetUIBehevior("RegisterButton_N").AddButtonEventListener(OnClickRegisterButton_N);
            GetUIBehevior("LoginButton_N").AddButtonEventListener(OnClickLoginButton_N);

        }
        void OnClickLoginButton_N( )
        {
           
            Pop();
        }
        void OnClickRegisterButton_N( )
        {
            string username = GetUIBehevior("Username_N").inputField.text;
            string password = GetUIBehevior("Password_N").inputField.text;
            request.Send(username,password);
        }
    }
}