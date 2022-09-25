using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class LoginCTRL :PanelCTRLBase
    {
        LoginRequest request;
       
        public override void Initial( )
        {
            base.Initial();
            BindEvent();
            
            request = RequestManager.Instance.GetRequest(ActionCode.Login)as LoginRequest;
            request.ResponseAction += HandleResponse;
        }
        public override void Enter( )
        {
            base.Enter();
            transform.parent.gameObject.SetActive( true );
           
        }
        public override void Exit( )
        {
            base.Exit();
            transform.parent.gameObject.SetActive(false);
        }
        public override void HandleResponse( MainPack pack )
        {
            base.HandleResponse(pack);
           
            if ( ( pack as MainPack ).Returncode == ReturnCode.Success )
            {
              
                    RequestManager.Instance.username = pack.Loginpack.Username;
                if ( pack.Roominfo.Count != 0 )
                    RequestManager.Instance.curRoomID = pack.Roominfo[0].Roomid;
                else
                    RequestManager.Instance.curRoomID = null;

                Loom.QueueOnMainThread(( a ) =>
                    {


                        if ( RequestManager.Instance.curRoomID == null ) 
                        {
                            Pop();
                            Push("Lobby");
                        }
                            
                        else 
                        {
                          
                            Pop();
                            Push("GamePanel");
                        }
                            
                       

                    } , null);
                    
            }
            else 
            {
                Loom.QueueOnMainThread(( a ) =>
                {
                    ShowMessage("µ«»Î ß∞‹");
                } , null);
            }
        }
       
        void BindEvent( ) 
        {
            GetUIBehevior("LoginButton_N").AddButtonEventListener(OnClickLoginButton_N);
            GetUIBehevior("RegisterButton_N").AddButtonEventListener(OnClickRegisterButton_N);
            GetUIBehevior("ReConnectButton_N").AddButtonEventListener(OnClickReConnectButton_N);
            

        }
        void OnClickLoginButton_N( ) 
        {
            string username = GetUIBehevior("Username_N").inputField.text;
            string password = GetUIBehevior("Password_N").inputField.text;
            request.Send(username, password);
            //UnityEditor.SceneManagement.EditorSceneManager.LoadScene(1);
        }
        void OnClickRegisterButton_N( )
        {
           Push("Register");
        }
        void OnClickReConnectButton_N( ) 
        {
            RequestManager.Instance.InitialSocket();
        }
    }
}