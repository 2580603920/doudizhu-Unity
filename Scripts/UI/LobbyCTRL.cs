using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Google.Protobuf.Collections;

namespace Doudizhu
{

    public class LobbyCTRL : PanelCTRLBase
    {
        public GetRoomListRequest getRoomListRequest;
        JoinRoomRequest joinRoomRequest;

        public float getRoomlistInterval = 0.3f;
      
        List<string> allRoomId, ownRoomID;
       

        public override void Enter( )
        {
           
            base.Enter();
            transform.parent.gameObject.SetActive(true);
            getRoomListRequest.Send();
        }
        public override void Stay(float delta )
        {
            base.Stay(delta);

            //实时刷新房间列表
            //if ( timer > getRoomlistInterval ) 
            //{
            //    getRoomListRequest.Send();

            //}
          
        }
        public override void Exit( )
        {
            base.Exit();
            transform.parent.gameObject.SetActive(false);
        }
        public override void Initial( )
        {
            base.Initial();

            BindEvent();
            allRoomId = new List<string>();
            ownRoomID = new List<string>();
            getRoomListRequest = RequestManager.Instance.GetRequest(ActionCode.GetRoomList) as GetRoomListRequest;
            joinRoomRequest = RequestManager.Instance.GetRequest(ActionCode.JoinRoom) as JoinRoomRequest;
         

            getRoomListRequest.ResponseAction += HandleResponse;
            joinRoomRequest.ResponseAction += HandleResponse;
           

        }
        void BindEvent( ) 
        {
            GetUIBehevior("CreateRoomButton_N").AddButtonEventListener(OnClickCreateRoomButton_N);
            GetUIBehevior("RefreshButton_N").AddButtonEventListener(OnClickRefreshButton_N);
            
        }
        void OnClickCreateRoomButton_N( ) 
        {
           
            Push("CreateRoomWindow");

        }
        void OnClickRefreshButton_N ()
        {
            getRoomListRequest.Send();
        
        }


        public override void HandleResponse( MainPack pack )
        {   
            //加入房间
            if ( pack.Actioncode == ActionCode.GetRoomList )
            {
                Loom.QueueOnMainThread(( a ) =>
                {
                    

                        UpdateRoomItem(pack.Roominfo);
                    

                } , null);
            }
            //获取房间列表
            else if ( pack.Actioncode == ActionCode.JoinRoom ) 
            {
                if ( pack.Returncode == ReturnCode.Success )
                {
                    RequestManager.Instance.curRoomID = pack.Roominfo[0].Roomid;

                    Loom.QueueOnMainThread(( a ) =>
                    {
                        Pop();
                        Push("GamePanel");
                        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
                    } , null);

                }
                else if ( pack.Returncode == ReturnCode.RoomFull )
                {
                    Loom.QueueOnMainThread(( a ) =>
                    {
                        ShowMessage("房间已满");

                    } , null);


                }
                else if ( pack.Returncode == ReturnCode.RoomDissolve )
                {
                    Loom.QueueOnMainThread(( a ) =>
                    {
                        ShowMessage("房间已被房主解散");

                    } , null);


                }
                else 
                {
                    Loom.QueueOnMainThread(( a ) =>
                    {
                        ShowMessage("加入房间失败");

                    } , null);

                }
                getRoomListRequest.Send();

            }
           
        }

        public void  UpdateRoomItem(RepeatedField< RoomInfo> roomInfos ) 
        {
            allRoomId.Clear();
            foreach ( var item in roomInfos )
            {
                if ( !ownRoomID.Contains(item.Roomid) )
                {
                    uIManager.LoadUI("RoomItem_N" , GetUITrans("RoomList_N"));
                    RenameUI("RoomItem_N" , item.Roomid);
                    GetUIBehevior(item.Roomid).AddButtonEventListener(OnClickRoomItem);
                    ownRoomID.Add(item.Roomid);
                }

                GetUITrans(item.Roomid).GetComponent<SubUImanager>().GetUIBehevior("RoomTitle_S").textUI.text = item.Roomtile;
                GetUITrans(item.Roomid).GetComponent<SubUImanager>().GetUIBehevior("PlayerNum_S").textUI.text = item.Playernum + "/3";
                allRoomId.Add(item.Roomid);
            }

            var c = ownRoomID.Except(allRoomId);
            
            foreach ( var item in  c.ToList()) 
            {

                GetUIBehevior(item).Delete();
                ownRoomID.Remove(item);

            }
           

        }
        void OnClickRoomItem( ) 
        {
            
            joinRoomRequest.Send(EventSystem.current.currentSelectedGameObject.name);
        
        }
    }
}