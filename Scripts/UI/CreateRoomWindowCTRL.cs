using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;

namespace Doudizhu
{
    public class CreateRoomWindowCTRL : PanelCTRLBase
    {
        CreateRoomRequest createRoomRequest;

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
        public override void Initial( )
        {
            base.Initial();

            createRoomRequest = RequestManager.Instance.GetRequest(ActionCode.CreateRoom) as CreateRoomRequest;
            createRoomRequest.ResponseAction += HandleResponse;

            GetUIBehevior("CreateButton_N").AddButtonEventListener(OnClickCreateButton_N);
        }
        void OnClickCreateButton_N( ) 
        {
           
            createRoomRequest.Send(GetUIBehevior("RoomNameInput_N").inputField.text);
        }


        public override void HandleResponse( MainPack pack )
        {
            if ( pack.Returncode == ReturnCode.Success )
            {
                RequestManager.Instance.curRoomID = pack.Roominfo[0].Roomid;
                Loom.QueueOnMainThread(( a ) =>
                {
                    Pop();
                    Pop();
                    Push("GamePanel");
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(2);
                } , null);

                //( uIManager.GetPanel("Lobby") as LobbyCTRL ).getRoomListRequest.Send();
            }
            else 
            {
                Loom.QueueOnMainThread(( a ) =>
                {
                    Pop();
                    ShowMessage("´´½¨Ê§°Ü");

                } , null);
            }

        }
    }
}