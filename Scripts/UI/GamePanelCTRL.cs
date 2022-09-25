using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Google.Protobuf.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;


namespace Doudizhu
{
    public class GamePanelCTRL : PanelCTRLBase
    {
        public Sprite[] allPokerSprite;
        public Sprite dizhuSprite,nongmingSprite;

        Dealer dealer;
        GetRoomInfoRequest getRoomInfoRequest;
        ExitRoomRequest exitRoomRequest;
        StartGameRquest startGameRquest;
        GetGameInfoR getGameInfoR;
        RobDizhu_R robDizhu;
        SendPoker_R sendPoker_R;
        DealPoker_R dealPoker_R;

        List<Transform> allPlayerTrans;
        public float updateRoomInfoInterval = 0.3f;
       

        

        List<Transform> sendPokersTrans;
        EventSystem eventSystem;
        GraphicRaycaster graphicRaycaster;
        PointerEventData pointerEventData;
        public override void Stay(float delta)
        {
            //timer1 += Time.time;
            //if ( timer1 > updateRoomInfoInterval ) 
            //{
            //    getRoomInfoRequest.Send(RequestManager.Instance.curRoomID);
            //    timer1 = 0;
            //} 
         
            //����ѡ���߼�
            if ( Input.GetMouseButtonDown(0) ) 
            {
                List<RaycastResult> tmepList = new List<RaycastResult>();
                pointerEventData.position = Input.mousePosition;    
                graphicRaycaster.Raycast(pointerEventData , tmepList);
                if ( tmepList.Count!= 0 ) 
                {
                    
                    if ( tmepList[0].gameObject.tag.Equals("SelectablePoker") ) 
                    {
                        if ( sendPokersTrans.Contains(tmepList[0].gameObject.transform) )
                        {
                            CancelSelectPoker(tmepList[0].gameObject.transform);
                        }
                        else 
                        {
                            SelectPoker(tmepList[0].gameObject.transform);
                        }
                    
                    }
                }             
            }


        }
        void SelectPoker(Transform pokerTrans) 
        {
            pokerTrans.DOLocalMoveY(20.0f , 0.6f);          
            sendPokersTrans.Add(pokerTrans);
        }
        void CancelSelectPoker( Transform pokerTrans  )
        {
            pokerTrans.DOLocalMoveY(0 , 0.6f);          
            sendPokersTrans.Remove(pokerTrans);
        }
        //ȡ��ѡ�������ѡ�е���
        void CancelAllSelectPoker ()
        {
            int i = 0;
            while( sendPokersTrans.Count !=0)
            {
                CancelSelectPoker(sendPokersTrans[i]);

            }

        }

        public override void Enter( )
        {

            base.Enter();
            InitialPanel();
            getGameInfoR.Send();

        }
        public override void Exit( )
        {
            base.Exit();
            transform.parent.gameObject.SetActive(false);
           
        }
        public override void Initial( )
        {
            base.Initial();
   
            allPlayerTrans = new List<Transform>();
            allPlayerTrans.Add( GetUITrans("Player1_N"));
            allPlayerTrans.Add( GetUITrans("Player2_N"));
            allPlayerTrans.Add( GetUITrans("Player3_N"));
            eventSystem = GameObject.FindWithTag("EventSystem").GetComponent<EventSystem>();
            graphicRaycaster = GameObject.FindWithTag("MainCanvas").GetComponent<GraphicRaycaster>();
            pointerEventData = new PointerEventData(eventSystem);


            getRoomInfoRequest = RequestManager.Instance.GetRequest(ActionCode.GetRoomInfo) as GetRoomInfoRequest;
            exitRoomRequest = RequestManager.Instance.GetRequest(ActionCode.ExitRoom) as ExitRoomRequest;
            startGameRquest = RequestManager.Instance.GetRequest(ActionCode.StartGame) as StartGameRquest;
            getGameInfoR = RequestManager.Instance.GetRequest(ActionCode.GetGameInfo) as GetGameInfoR;
            robDizhu = RequestManager.Instance.GetRequest(ActionCode.RobHost) as RobDizhu_R;
            sendPoker_R = RequestManager.Instance.GetRequest(ActionCode.SendPoker) as SendPoker_R;
            dealPoker_R = RequestManager.Instance.GetRequest(ActionCode.DealPoker) as DealPoker_R;
            getRoomInfoRequest.ResponseAction += HandleResponse;
            exitRoomRequest.ResponseAction += HandleResponse;
            startGameRquest.ResponseAction += HandleResponse;
            getGameInfoR.ResponseAction +=HandleResponse;
            sendPoker_R.ResponseAction += HandleResponse;
            dealPoker_R.ResponseAction += HandleResponse;
            BandUIEvent();

        }
        //��ʼ�����
        void InitialPanel( ) 
        {
            sendPokersTrans = new List<Transform>();
            dealer = new Dealer();
            ShowdizhuFlag = false;
            transform.parent.gameObject.SetActive(true);
            getRoomInfoRequest.Send(RequestManager.Instance.curRoomID);
            allPlayerTrans[0].gameObject.SetActive(false);
            allPlayerTrans[1].gameObject.SetActive(false);
            allPlayerTrans[2].gameObject.SetActive(false);
            GetSubUiMnager("Player1_N").GetUIBehevior("Touxiang_S").image.sprite = nongmingSprite;
            GetSubUiMnager("Player2_N").GetUIBehevior("Touxiang_S").image.sprite = nongmingSprite;
            GetSubUiMnager("Player3_N").GetUIBehevior("Touxiang_S").image.sprite = nongmingSprite;



            //����UI���
            GetUITrans("Player1Pokers_N").gameObject.SetActive(false);
            GetUITrans("DizhuPokers_N").gameObject.SetActive(false);
            GetUITrans("Player1TimeCountDown_N").gameObject.SetActive(false);
            GetUITrans("Player1SendPokers_N").gameObject.SetActive(false);
            GetUITrans("RobDizhuBT_N").gameObject.SetActive(false);
            GetUITrans("NotRobDizhuBT_N").gameObject.SetActive(false);
            GetUITrans("SendPokerBT_N").gameObject.SetActive(false);
            GetUITrans("NotSendPokerBT_N").gameObject.SetActive(false);

            allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(false);
            allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("SendPokers_S").gameObject.SetActive(false);
            allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(false);
            allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(false);
            allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("SendPokers_S").gameObject.SetActive(false);
            allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(false);
        }
        # region ��UI�¼�
        //��UI�¼�
        void BandUIEvent( ) 
        {

            GetUIBehevior("ExitRoomButton_N").AddButtonEventListener(OnClickExitRoomButton_N);
            GetUIBehevior("StartGameButton_N").AddButtonEventListener(OnClickStartGameButton_N);
            GetUIBehevior("RobDizhuBT_N").AddButtonEventListener(OnClickRobDizhuBT_N);
            GetUIBehevior("NotRobDizhuBT_N").AddButtonEventListener(OnClickNotRobDizhuBT_N);
            GetUIBehevior("SendPokerBT_N").AddButtonEventListener(OnClickSendPokerBT_N);
            GetUIBehevior("NotSendPokerBT_N").AddButtonEventListener(OnClickNotSendPokerBT_N);

        }
        void OnClickExitRoomButton_N( ) 
        {
           
            exitRoomRequest.Send(RequestManager.Instance.curRoomID);

        }
        void OnClickStartGameButton_N( )
        {
            startGameRquest.Send();

        }
        void OnClickRobDizhuBT_N( ) 
        {

            robDizhu.Send(RequestManager.Instance.username,true);
        
        }
        void OnClickNotRobDizhuBT_N( )
        {

            robDizhu.Send(RequestManager.Instance.username , false);

        }

        
        void OnClickSendPokerBT_N( ) 
        {
            if ( sendPokersTrans.Count == 0 ) 
            {
                return;
            }
            

            RepeatedField<SoketDoudizhuProtocol.Poker> packPokers = new RepeatedField<SoketDoudizhuProtocol.Poker>();
            List<Dealer.Poker> dealPokers = new List<Dealer.Poker>();
            foreach ( var item in sendPokersTrans ) 
            {
              
                dealPokers.Add(dealer.ParseDealPoker(GetSpritePoker(item.name)));
            }

            //���Ͳ�����
            if ( dealer.GetPokersType(dealPokers).porkerTypeUtils == Dealer.PokerTypeUtils.None ) 
            {
                CancelAllSelectPoker();
                return;
            }
            dealer.SortPork(dealPokers);
            foreach (var item in dealPokers ) 
            {
                packPokers.Add(dealer.ParsePackPoker(item));
            }


            sendPoker_R.Send(RequestManager.Instance.username , packPokers);

        }
        void OnClickNotSendPokerBT_N( )
        {
            sendPoker_R.Send(RequestManager.Instance.username);
            CancelAllSelectPoker();
        }
        #endregion

        public override void HandleResponse( MainPack pack )
        {
            base.HandleResponse(pack);
            switch ( pack.Actioncode ) 
            {
                //��ȡ��ǰ������Ϣ
                case ActionCode.GetRoomInfo: 
                    {
                        HandleGetRoomInfo(pack);
                        break;
                    }
                //�˳�����
                case ActionCode.ExitRoom:
                    {
                        if ( pack.Returncode == ReturnCode.Success )
                        {

                            Loom.QueueOnMainThread(( a ) =>
                            {
                                Pop();
                                Push("Lobby");
                            } , null);
                        }
                        else
                        {
                            Loom.QueueOnMainThread(( a ) =>
                            {
                                uIManager.ShowMessage("�˳�����ʧ��");

                            } , null);
                        }
                        break;
                    }

                //��ʼ��Ϸ
                case ActionCode.StartGame: 
                    {

                        HandleStartGame(pack);
                        break;
                    }
                //��ȡ��Ϸ��Ϣ
                case ActionCode.GetGameInfo: 
                    {
                        HandleGetGameInfo(pack);

                        break;
                    }
                    //��Ⱦ�������������������
                case ActionCode.DealPoker:
                    {
                        RenderPlayersPokers(pack.Gameinfo.Playerinfo);

                        break;
                    }
                    //��������Ӧ
                case ActionCode.SendPoker: 
                    {
                        HandleSendPoker(pack);
                        
                        break;
                    }
            }
           
        }

        //�����ȡ������Ϣ��Ӧ
        void HandleGetRoomInfo(MainPack pack ) 
        {
            if ( pack.Returncode == ReturnCode.Success ) 
            {
                Loom.QueueOnMainThread(( a ) =>
                {
                    if(pack.Roominfo[0].Status == 3 || RequestManager.Instance.username != pack.Roominfo[0].Landlord )
                        GetUITrans("StartGameButton_N").gameObject.SetActive(false);
                    else
                        GetUITrans("StartGameButton_N").gameObject.SetActive(true);
                    int i = 1;
                    allPlayerTrans[0].gameObject.SetActive(false);
                    allPlayerTrans[1].gameObject.SetActive(false);
                    allPlayerTrans[2].gameObject.SetActive(false);

                    foreach ( var item in pack.Roominfo[0].Playerinfo )
                    {

                        if ( item.Username == RequestManager.Instance.username )
                        {

                            allPlayerTrans[0].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text = item.Username;
                            allPlayerTrans[0].GetComponent<SubUImanager>().GetUIBehevior("Coin_S").textUI.text = item.Coin;
                            allPlayerTrans[0].GetComponent<SubUImanager>().GetUIBehevior("ReadyState_S").textUI.text = item.Status.ToString();
                            allPlayerTrans[0].gameObject.SetActive(true);
                            continue;
                        }
                        allPlayerTrans[i].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text = item.Username;
                        allPlayerTrans[i].GetComponent<SubUImanager>().GetUIBehevior("Coin_S").textUI.text = item.Coin;
                        allPlayerTrans[i].GetComponent<SubUImanager>().GetUIBehevior("ReadyState_S").textUI.text = item.Status.ToString();
                        allPlayerTrans[i].gameObject.SetActive(true);
                        i++;
                    }
                } , null);
            }
            

        }
        //����ʼ��Ϸ��Ӧ
        void HandleStartGame(MainPack pack ) 
        {
            Loom.QueueOnMainThread(( a ) => 
            {
                if ( pack.Returncode == ReturnCode.Fail )
                {
                    ShowMessage("��ʼ��Ϸʧ��");
                }
                else
                {
                    GetUITrans("StartGameButton_N").gameObject.SetActive(false);
                }

            } ,null);
           


        }

        bool ShowdizhuFlag ;
        //������Ϸ��Ϣ
        void HandleGetGameInfo( MainPack pack ) 
        {
            Loom.QueueOnMainThread(( a) => 
            {
                switch ( pack.Gameinfo.Status )
                {
                    //��Ϸ��ʼ�׶�
                    case 1:
                        {
                            RenderPlayersPokers(pack.Gameinfo.Playerinfo);
                            break;
                        }
                    //�������׶�
                    case 2:
                        {

                            InitialUI();
                            
                            if ( pack.Gameinfo.Curusername == RequestManager.Instance.username )
                            {
                                //��ʾ��������ť
                                GetUITrans("RobDizhuBT_N").gameObject.SetActive(true);
                                GetUITrans("NotRobDizhuBT_N").gameObject.SetActive(true);
                                //��ʾ�غϼ�ʱ
                                GetUIBehevior("TimeCountDownText_N").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString() ;
                                GetUITrans("Player1TimeCountDown_N").gameObject.SetActive(true);


                            }
                            //��Ⱦ���������Ϣ
                            else if ( pack.Gameinfo.Curusername == allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                            {
                                //��ʾ�غϼ�ʱ
                                allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("TimeCountDownText_S").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString();
                                allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(true);



                            }
                            else if ( pack.Gameinfo.Curusername == allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                            {
                                //��ʾ�غϼ�ʱ
                                allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("TimeCountDownText_S").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString() ;
                                allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(true);


                            }

                            break;
                        }
                    //��ս�׶�
                    case 3:
                        {
                            InitialUI();
                            allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(true);
                            allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(true);

                            if ( ShowdizhuFlag==false ) 
                            {
                                ShowdizhuFlag = true;
                                //��ʾ������
                                int i = 1;
                                foreach ( var item in pack.Gameinfo.Dizhupoker )
                                {

                                    GetUIBehevior("DizhuPoker" + ( i++ ) + "_N").image.sprite = GetPokerSprite(item.Weight , item.Pokercolor);
                                }


                            }



                            //��ʾ����ͷ��
                            if ( pack.Gameinfo.Dizhu != null )
                            {
                                foreach ( var item in allPlayerTrans )
                                {
                                    if ( GetSubUiMnager(item.name).GetUIBehevior("Username_S").textUI.text.Equals(pack.Gameinfo.Dizhu) )
                                    {

                                        GetSubUiMnager(item.name).GetUIBehevior("Touxiang_S").image.sprite = dizhuSprite;


                                    }

                                }
                            }

                            if ( pack.Gameinfo.Curusername == RequestManager.Instance.username )
                            {
                                
                                //��ʾ���ư�ť
                                GetUITrans("SendPokerBT_N").gameObject.SetActive(true);
                                GetUITrans("NotSendPokerBT_N").gameObject.SetActive(true);
                                //��ʾ�غϼ�ʱ
                                GetUIBehevior("TimeCountDownText_N").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString() ;
                                GetUITrans("Player1TimeCountDown_N").gameObject.SetActive(true);


                            }
                            //��Ⱦ���������Ϣ
                            else if ( pack.Gameinfo.Curusername == allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                            {
                                //��ʾ�غϼ�ʱ
                                allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("TimeCountDownText_S").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString();
                                allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(true);



                            }
                            else if ( pack.Gameinfo.Curusername == allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                            {
                                //��ʾ�غϼ�ʱ
                                allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("TimeCountDownText_S").textUI.text = ( pack.Gameinfo.Playertimes - 1 ).ToString();
                                allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(true);


                            }





                            break;
                        }
                    //��Ϸ����
                    case 4: 
                        {
                            Loom.QueueOnMainThread(( a ) =>
                            {
                                if ( pack.Gameinfo.Curusername == pack.Gameinfo.Dizhu && pack.Gameinfo.Dizhu == RequestManager.Instance.username )
                                {

                                    ShowMessage("��Ϸʤ��");
                                    
                                   
                                }
                                else 
                                {
                                    ShowMessage("��Ϸʧ��");

                                }
                                Invoke("InitialPanel" , 0.8f);

                            } , null);
                            break;
                        }
                }
            } , null);
           
           
          
            
        }
        //���������Ϣ
        void HandleSendPoker(MainPack pack ) 
        {
            Loom.QueueOnMainThread(( a ) =>
            {
                if ( pack.Returncode == ReturnCode.Success )
                {
                  
                    GetUITrans("SendPokerBT_N").gameObject.SetActive(false);
                    GetUITrans("NotSendPokerBT_N").gameObject.SetActive(false);
                    GetUITrans("Player1TimeCountDown_N").gameObject.SetActive(false);

                    foreach ( var item in pack.Sendpokersinfo ) 
                    {
                        //��Ⱦ�������
                        if ( item.Username == RequestManager.Instance.username )
                        {


                            foreach ( var tempTrans in GetUITrans("Player1SendPokers_N").GetComponentsInChildren<Transform>() )
                            {
                                if ( tempTrans.name != "Player1SendPokers_N" )
                                {
                                    Destroy(tempTrans.gameObject);
                                }


                            }
                            int i = 0;
                            foreach ( var poker in item.Poker )
                            {
                                Transform temp = LoadUI("SmallPoker" , GetUITrans("Player1SendPokers_N"));
                                temp.name = GetPokerSprite(poker.Weight , poker.Pokercolor).name;
                                temp.GetComponent<Image>().sprite = GetPokerSprite(poker.Weight , poker.Pokercolor);
                                temp.localPosition = new Vector3(-30 + 10 * i , 0 , 0);
                                temp.SetAsLastSibling();
                                i++;
                            }
                            GetUITrans("Player1SendPokers_N").gameObject.SetActive(true);
                            sendPokersTrans.Clear();
                        }
                        //��Ⱦ�����������
                       
                        else if ( item.Username == allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                        {
                            Transform tempT = allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("SendPokers_S");
                            foreach ( var tempTrans in tempT.GetComponentsInChildren<Transform>() )
                            {
                                if ( tempTrans.name != "SendPokers_S" )
                                {
                                    Destroy(tempTrans.gameObject);
                                }


                            }
                            int i = 0;
                            foreach ( var poker in item.Poker )
                            {
                                Transform temp = LoadUI("SmallPoker" , tempT);
                                temp.name = GetPokerSprite(poker.Weight , poker.Pokercolor).name;
                                temp.GetComponent<Image>().sprite = GetPokerSprite(poker.Weight , poker.Pokercolor);
                                temp.localPosition = new Vector3(20 + 10 * i , 0 , 0);
                                temp.SetAsLastSibling();
                                i++;
                            }
                            tempT.gameObject.SetActive(true);

                        }
                        else if ( item.Username == allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                        {
                            Transform tempT = allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("SendPokers_S");
                            foreach ( var tempTrans in tempT.GetComponentsInChildren<Transform>() )
                            {
                                if ( tempTrans.name != "SendPokers_S" )
                                {
                                    Destroy(tempTrans.gameObject);
                                }


                            }
                            int i = 0;
                            foreach ( var poker in item.Poker )
                            {
                                Transform temp = LoadUI("SmallPoker" , tempT);
                                temp.name = GetPokerSprite(poker.Weight , poker.Pokercolor).name;
                                temp.GetComponent<Image>().sprite = GetPokerSprite(poker.Weight , poker.Pokercolor);
                                temp.localPosition = new Vector3(-33 -10 * i , 0 , 0);
                                temp.SetAsFirstSibling();
                                i++;
                            }
                            tempT.gameObject.SetActive(true);

                        }
                    }
                }
                else
                {
                    CancelAllSelectPoker();

                }

            } , null);

        }
        //��ʼ����Ϸ���ui
        void InitialUI( ) 
        {

            //��ʼ�����м�ʱ��
            allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(false);
            allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("TimeCountDown_S").gameObject.SetActive(false);
            GetUITrans("Player1TimeCountDown_N").gameObject.SetActive(false);

            //������������ť
            GetUITrans("RobDizhuBT_N").gameObject.SetActive(false);
            GetUITrans("NotRobDizhuBT_N").gameObject.SetActive(false);
            GetUITrans("SendPokerBT_N").gameObject.SetActive(false);
            GetUITrans("NotSendPokerBT_N").gameObject.SetActive(false);

        }
        //��Ⱦ�������
        void RenderPlayersPokers(RepeatedField<PlayerInfo> playerInfos ) 
        {
            
            Loom.QueueOnMainThread(( a ) =>
            {
                CancelAllSelectPoker();
                allPlayerTrans[1].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(true);
                allPlayerTrans[2].GetComponent<SubUImanager>().GetUITrans("PokerNum_S").gameObject.SetActive(true);
                foreach ( var info in playerInfos )
                {
                   
                    
                    //Ϊ�Լ���Ⱦ����
                    if ( info.Username == RequestManager.Instance.username )
                    {
                       
                       
                            foreach (var item in GetUITrans("Player1Pokers_N").GetComponentsInChildren<Transform>()) 
                            {
                                if ( item.name != "Player1Pokers_N" ) 
                                {

                                    GetSubUiMnager("Player1Pokers_N").UnRegister(item.name);
                                    //item.gameObject.SetActive(false);
                                    Destroy(item.gameObject);
                                  }
                               
                                
                            } 
                            
                       

                        int i = 0;
                        foreach ( var item in info.Poker )
                        {
                            Transform temp = LoadUI("BigPorker" , GetUITrans("Player1Pokers_N"));
                            temp.name = GetPokerSprite(item.Weight , item.Pokercolor).name;
                            temp.GetComponent<Image>().sprite = GetPokerSprite(item.Weight , item.Pokercolor);
                            temp.localPosition = new Vector3(245 - 25 * i , 0 , 0);
                            temp.SetAsFirstSibling();
                            i++;
                        }
                        GetUITrans("Player1Pokers_N").gameObject.SetActive(true);

                    }
                    //��Ⱦ���������Ϣ
                    else if ( info.Username == allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                    {

                        allPlayerTrans[1].GetComponent<SubUImanager>().GetUIBehevior("PokerNumText_S").textUI.text = info.Curpokernum.ToString();

                    }
                    else if ( info.Username == allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("Username_S").textUI.text )
                    {
                        allPlayerTrans[2].GetComponent<SubUImanager>().GetUIBehevior("PokerNumText_S").textUI.text = info.Curpokernum.ToString();

                    }

                }


                GetUITrans("Player1Pokers_N").gameObject.SetActive(true);
                GetUITrans("DizhuPokers_N").gameObject.SetActive(true);

            } , null);

        }
        //��ȡ��Ӧ����ľ���ͼ
        Sprite GetPokerSprite( int weight , int pokerColor )
        {
            int index = 0;

            //��С��
            if ( pokerColor == 5 )
            {
                index += weight - 48;
                return allPokerSprite[index];
            }
            //A��2
            if ( weight > 13 )
            {
                index += weight - 14;
            }
            else
                //��ͨ��
                index += weight - 1;


            for ( int i = 0 ; i < pokerColor - 1 ; i++ )
            {

                index += 13;

            }
            return allPokerSprite[index];
        }
        //��ȡ��Ӧ����ͼ���ֵ�����
        SoketDoudizhuProtocol.Poker GetSpritePoker(string spriteName ) 
        {
            SoketDoudizhuProtocol.Poker poker = new SoketDoudizhuProtocol.Poker();
            int index =Convert.ToInt32(spriteName.Split("_")[1]);

            //��ɫ�ж�
            if ( index > 51 )
            {
                poker.Pokercolor = 5;
                if ( index == 52 ) poker.Weight = 100;
                if ( index == 53 ) poker.Weight = 101;
                return poker;
            }
            else if ( index > 38 )
            {

                poker.Pokercolor = 4;

            }
            else if ( index > 25 ) 
            {

                poker.Pokercolor = 3;
            }
            else if ( index > 12 )
            {

                poker.Pokercolor = 2;
            }
            else
            {

                poker.Pokercolor = 1;
            }

            //����Ȩֵ�ж�

            poker.Weight = (index % 13)+1;

            if ( poker.Weight == 1 || poker.Weight == 2 ) 
            {
                poker.Weight += 13;

            }

            return poker;

        }
        SubUImanager GetSubUiMnager(string name )
        {

            return GetUITrans(name).GetComponent<SubUImanager>();
        }
      

       
       
    }
    public class Dealer
    {
        //����
        public enum PokerTypeUtils
        {
            None,
            Single, //���ţ�һ��������
            Double,//���ӣ�������ͬ����
            Trebling,//���ţ�������ͬ����
            TreblingWithOne,//����һ��������ͬ����+һ��������
            TreblingWithTwo,//��������������ͬ����+����
            Bomb,//ը����������ͬ����
            QuadrupleWithSingle,//�Ĵ�һ��������ͬ����+2��������
            QuadrupleWithDouble,//�Ĵ�����������ͬ����+����
            KingBomb,//��ը����С��
            Straight,//˳�ӣ�5~12�ŵ��ţ���С3�����A
            ContinuousDouble,//���ӣ�6~20�ţ���������3������(3~10��)����С3�����A
            Plane,//�ɻ���6~18�ţ�������������3��
            PlaneWithSingle,
            PlaneWithDouble,
        }
        public class PokerType
        {
            public PokerTypeUtils porkerTypeUtils;
            public int weight;
        }
        public enum PokerColor
        {
            Meihua,
            Fangkuai,
            Hongtao,
            Heitao,
            Max
        }
        public class Poker
        {
            public int weight; //��3��ʼ~15
            public PokerColor pokerColor;
        }

        public Dealer( )
        {
           
        }
       
        //����
        public bool ComparePoker( List<Poker> compare , List<Poker> compared )
        {

            PokerType compareType = GetPokersType(compare);
            PokerType comparedType = GetPokersType(compared);

            if ( compareType.porkerTypeUtils != comparedType.porkerTypeUtils ) return false;

            if ( compareType.weight > comparedType.weight ) return true;

            return false;
        }
        //��ȡ�������ͺ�����Ȩ��
        public PokerType GetPokersType( List<Poker> pokers = null )
        {
            PokerType porkerType = new PokerType();
            porkerType.porkerTypeUtils = PokerTypeUtils.None;
            if ( pokers == null )
            {
                porkerType.weight = 0;
                return porkerType;
            }

            //Ȩ����Ϊkey
            var tempDict = pokers.GroupBy(c => c.weight).ToDictionary(k => k.Key , k => k.Count());
            tempDict = ( from pair in tempDict orderby pair.Value descending select pair ).ToDictionary(k => k.Key , v => v.Value);

            //����ظ�������
            int MaxNum = tempDict.First().Value;
            //��ͬ���������
            int TypeNum = tempDict.Count;

            switch ( MaxNum )
            {
                //��ǰ���ƶ��ǵ���
                case 1:
                    {
                        //��������
                        if ( TypeNum == 1 )
                        {
                            porkerType.weight = tempDict.First().Key;
                            porkerType.porkerTypeUtils = PokerTypeUtils.Single;

                        }
                        //��ը����
                        if ( TypeNum == 2 )
                        {
                            int tempWeight = 0;
                            foreach ( var item in tempDict )
                            {
                                tempWeight += item.Key;
                            }
                            if ( tempWeight == 201 )
                            {
                                porkerType.weight = tempWeight;
                                porkerType.porkerTypeUtils = PokerTypeUtils.KingBomb;
                            }

                        }

                        //˳������
                        if ( TypeNum >= 5 && TypeNum <= 12 )
                        {
                            int tempWeight = 0;
                            bool isContinuous = true;

                            var sortList = ( from pair in tempDict orderby pair.Key ascending select pair ).ToList();

                            for ( int i = 0 ; i < sortList.Count - 1 ; i++ )
                            {
                                if ( sortList[i].Key + 1 != sortList[i + 1].Key )
                                {
                                    isContinuous = false;
                                    break;

                                }
                                tempWeight += sortList[i].Key;
                            }
                            if ( sortList.Last().Key > 14 ) isContinuous = false;
                            tempWeight += sortList.Last().Key;


                            if ( isContinuous )
                            {

                                porkerType.weight = tempWeight;
                                porkerType.porkerTypeUtils = PokerTypeUtils.Straight;

                            }

                        }
                        break;
                    }
                //��ǰ��������ظ�������
                case 2:
                    {
                        //��������
                        if ( TypeNum == 1 )
                        {

                            int tempWeight = tempDict.First().Key * 2;
                            porkerType.weight = tempWeight;
                            porkerType.porkerTypeUtils = PokerTypeUtils.Double;
                        }
                        //��������
                        if ( TypeNum >= 3 && TypeNum <= 12 )
                        {

                            int tempWeight = 0;
                            bool isContinuous = true;

                            var sortList = ( from pair in tempDict orderby pair.Key ascending select pair ).ToList();

                            for ( int i = 0 ; i < sortList.Count - 1 ; i++ )
                            {
                                if ( sortList[i].Key + 1 != sortList[i + 1].Key || sortList[i].Value != 2 )
                                {
                                    isContinuous = false;
                                    break;
                                }
                                tempWeight += sortList[i].Key;
                            }
                            if ( sortList.Last().Key > 14 ) isContinuous = false;
                            tempWeight += sortList.Last().Key;

                            if ( isContinuous )
                            {

                                porkerType.weight = tempWeight * 2;
                                porkerType.porkerTypeUtils = PokerTypeUtils.ContinuousDouble;

                            }
                        }

                        break;
                    }
                //��ǰ��������ظ�������
                case 3:
                    {
                        //��������
                        if ( TypeNum == 1 )
                        {

                            porkerType.weight = tempDict.First().Key * 3;
                            porkerType.porkerTypeUtils = PokerTypeUtils.Trebling;

                        }
                        //����һ����
                        if ( TypeNum == 2 && tempDict.ElementAt(1).Value == 1 )
                        {

                            porkerType.weight = tempDict.First().Key * 3;
                            porkerType.porkerTypeUtils = PokerTypeUtils.TreblingWithOne;

                        }
                        //����������
                        if ( TypeNum == 2 && tempDict.ElementAt(1).Value == 2 )
                        {

                            porkerType.weight = tempDict.First().Key * 3;
                            porkerType.porkerTypeUtils = PokerTypeUtils.TreblingWithTwo;

                        }
                        //�ɻ�����
                        if ( TypeNum >= 2 && TypeNum <= 6 )
                        {

                            bool isContinous = true;
                            int tempWeight = 0;

                            var sortList = ( from pair in tempDict orderby pair.Key ascending select pair ).ToList();

                            var singleList = new List<KeyValuePair<int , int>>();
                            var doubleList = new List<KeyValuePair<int , int>>();
                            var treblingList = new List<KeyValuePair<int , int>>();

                            foreach ( var item in sortList )
                            {
                                if ( item.Value == 1 ) singleList.Add(item);
                                else if ( item.Value == 2 ) doubleList.Add(item);
                                else treblingList.Add(item);
                            }

                            for ( int i = 0 ; i < treblingList.Count - 1 ; i++ )
                            {
                                if ( treblingList[i].Key + 1 != treblingList[i + 1].Key )
                                {
                                    isContinous = false;
                                    break;
                                }
                                tempWeight += treblingList[i].Key;
                            }

                            //���A
                            if ( treblingList.Last().Key > 14 ) isContinous = false;
                            tempWeight += treblingList.Last().Key;


                            if ( isContinous && singleList.Count == 0 && doubleList.Count == 0 )
                            {

                                porkerType.weight = tempWeight * 3;
                                porkerType.porkerTypeUtils = PokerTypeUtils.Plane;
                            }

                        }
                        //�ɻ�����������
                        if ( TypeNum >= 4 && TypeNum <= 10 )
                        {


                            bool isContinous = true;
                            int tempWeight = 0;

                            var sortList = ( from pair in tempDict orderby pair.Key ascending select pair ).ToList();

                            var singleList = new List<KeyValuePair<int , int>>();
                            var doubleList = new List<KeyValuePair<int , int>>();
                            var treblingList = new List<KeyValuePair<int , int>>();

                            foreach ( var item in sortList )
                            {
                                if ( item.Value == 1 ) singleList.Add(item);
                                else if ( item.Value == 2 ) doubleList.Add(item);
                                else treblingList.Add(item);
                            }

                            for ( int i = 0 ; i < treblingList.Count - 1 ; i++ )
                            {
                                if ( treblingList[i].Key + 1 != treblingList[i + 1].Key )
                                {
                                    isContinous = false;
                                    break;
                                }
                                tempWeight += treblingList[i].Key;
                            }

                            //���A
                            if ( treblingList.Last().Key > 14 ) isContinous = false;

                            tempWeight += treblingList.Last().Key;

                            if ( isContinous && singleList.Count == treblingList.Count && doubleList.Count == 0 )
                            {

                                porkerType.weight = tempWeight * 3;
                                porkerType.porkerTypeUtils = PokerTypeUtils.PlaneWithSingle;
                            }

                        }
                        //�ɻ���������
                        if ( TypeNum >= 4 && TypeNum <= 8 )
                        {
                            bool isContinous = true;
                            int tempWeight = 0;

                            var sortList = ( from pair in tempDict orderby pair.Key ascending select pair ).ToList();

                            var singleList = new List<KeyValuePair<int , int>>();
                            var doubleList = new List<KeyValuePair<int , int>>();
                            var treblingList = new List<KeyValuePair<int , int>>();

                            foreach ( var item in sortList )
                            {
                                if ( item.Value == 1 ) singleList.Add(item);
                                else if ( item.Value == 2 ) doubleList.Add(item);
                                else treblingList.Add(item);
                            }

                            for ( int i = 0 ; i < treblingList.Count - 1 ; i++ )
                            {
                                if ( treblingList[i].Key + 1 != treblingList[i + 1].Key )
                                {
                                    isContinous = false;
                                    break;
                                }
                                tempWeight += treblingList[i].Key;
                            }

                            //���A
                            if ( treblingList.Last().Key > 14 ) isContinous = false;

                            tempWeight += treblingList.Last().Key;

                            if ( isContinous && singleList.Count == 0 && doubleList.Count == treblingList.Count )
                            {

                                porkerType.weight = tempWeight * 3;
                                porkerType.porkerTypeUtils = PokerTypeUtils.PlaneWithDouble;
                            }

                        }

                        break;
                    }
                //��ǰ����ظ�������
                case 4:
                    {
                        //ը������
                        if ( TypeNum == 1 )
                        {

                            porkerType.weight = tempDict.First().Key * 4;
                            porkerType.porkerTypeUtils = PokerTypeUtils.Bomb;
                        }
                        //�Ĵ������� �� �Ĵ���������
                        if ( TypeNum == 3 )
                        {
                            bool isSinglePass = true;
                            bool isDoublePass = true;

                            foreach ( var item in tempDict )
                            {
                                if ( item.Value == 2 ) isSinglePass = false;
                                if ( item.Value == 1 ) isDoublePass = false;

                            }


                            if ( isSinglePass )
                            {

                                porkerType.weight = tempDict.First().Key * 4;
                                porkerType.porkerTypeUtils = PokerTypeUtils.QuadrupleWithSingle;

                            }
                            if ( isDoublePass )
                            {
                                porkerType.weight = tempDict.First().Key * 4;
                                porkerType.porkerTypeUtils = PokerTypeUtils.QuadrupleWithDouble;

                            }

                        }

                        break;
                    }

            }

            if ( porkerType.porkerTypeUtils == PokerTypeUtils.None ) porkerType.weight = 0;

            return porkerType;

        }
        //������������
        public void SortPork( List<Poker> pokers )
        {
            if ( pokers.Count == 0 ) return;
            //��Ȩ�ط���
            Dictionary<int , List<Poker>> tempDict = new Dictionary<int , List<Poker>>();

            foreach ( var item in pokers )
            {
                if ( !tempDict.ContainsKey(item.weight) )
                {

                    tempDict.Add(item.weight , new List<Poker>());

                }
                tempDict[item.weight].Add(item);
            }
            var sortResult1 = ( from pair in tempDict orderby pair.Key ascending select pair ).ToDictionary(k => k.Key , v => v.Value);

            tempDict.Clear();


            foreach ( var item in sortResult1 )
            {
                item.Value.Sort(( a , b ) => (int) a.pokerColor <= (int) b.pokerColor ? -1 : 1);
                tempDict.Add(item.Key , item.Value);
            }
            pokers.Clear();
            foreach ( var item in tempDict )
            {
                foreach ( var item1 in item.Value )
                {
                    pokers.Add(item1);

                }

            }
        }
        //�����е��˿�����תΪDeal���е��˿�����
        public  Dealer.Poker ParseDealPoker( SoketDoudizhuProtocol.Poker packPoker )
        {
            Dealer.Poker poker = new Dealer.Poker();
            poker.weight = packPoker.Weight;
            poker.pokerColor = (Dealer.PokerColor) ( packPoker.Pokercolor - 1 );
            return poker;
        }
        //��Deal���е��˿�������תΪ�����˿˵�����
        public SoketDoudizhuProtocol.Poker ParsePackPoker( Dealer.Poker Dpoker )
        {
            SoketDoudizhuProtocol.Poker poker = new SoketDoudizhuProtocol.Poker();
            poker.Weight = Dpoker.weight;
            poker.Pokercolor = (int)Dpoker.pokerColor + 1 ;
            return poker;
        }
    }
}