using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoketDoudizhuProtocol;
using Google.Protobuf;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;

namespace Doudizhu 
{
    public class RequestManager
    {
        Socket socket;
        EndPoint remoteIP;
        Message message;
        Dictionary<ActionCode , RequestBase> allRequest;
        static RequestManager instance;
        string ip = "1.tcp.cpolar.top";
        

        public string curRoomID;
        public string username;
      

        LoginRequest loginRequest;
        RegisterRequest registerRequest;
        GetRoomListRequest getRoomListRequest;
        CreateRoomRequest createRoomRequest;
        JoinRoomRequest JoinRoomRequest;
        GetRoomInfoRequest getRoomInfoRequest;
        ExitRoomRequest exitRoomRequest;
        StartGameRquest startGameRquest;
        GetGameInfoR getGameInfoR;
        RobDizhu_R robDizhu_R;
        SendPoker_R sendPoker_R;
        DealPoker_R  dealPoker_R;

        IPHostEntry entry;
        void InitialRequest( )
        {
            loginRequest = new LoginRequest();
            registerRequest = new RegisterRequest();
            getRoomListRequest = new GetRoomListRequest();
            createRoomRequest = new CreateRoomRequest();
            JoinRoomRequest = new JoinRoomRequest();
            getRoomInfoRequest = new GetRoomInfoRequest();
            exitRoomRequest = new ExitRoomRequest();
            startGameRquest = new StartGameRquest();
            getGameInfoR = new GetGameInfoR();
            robDizhu_R = new RobDizhu_R();
            sendPoker_R = new SendPoker_R();
            dealPoker_R = new DealPoker_R();
            
            
        }

        public static RequestManager Instance { 
            get 
            {
                if ( instance ==null ) 
                {
                   
                    instance = new RequestManager();

                }
                    
                return instance;
            }  
        }

         RequestManager( ) 
        {
           
            allRequest = new Dictionary<ActionCode , RequestBase>();    
           
        }
        ~RequestManager( ) 
        {
            
            Close();
        
        }
        public  void Initial( ) 
        {
            //InitialSocket();
            InitialRequest();
        }
       
        public RequestBase GetRequest(ActionCode actionCode ) 
        {
            RequestBase rb = null;
            if(allRequest.ContainsKey(actionCode) )
                rb = allRequest[actionCode];
            return rb;
        
        }
        public void RegisterRequset(ActionCode actionCode,RequestBase requestBase ) 
        {
            if ( allRequest.ContainsKey(actionCode) ) 
            {
                Debug.LogWarning("�����Ѿ�ע��");
                return;
            } 
            allRequest.Add(actionCode , requestBase);
        }
        public void InitialSocket( ) 
        {
            entry = Dns.GetHostEntry(ip);
            
            ThreadPool.QueueUserWorkItem(( a ) =>
           {
              

               message = new Message();
               socket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
               //remoteIP = new IPEndPoint(IPAddress.Parse(ip) , 8888);
               
               remoteIP = new IPEndPoint(entry.AddressList[0], 30908); 
               try
               {
                   socket.Connect(remoteIP);
                   Debug.Log("TCP�������ӳɹ���");
                   StartReceive();
                  Loom.QueueOnMainThread(( a ) => { UIManager.Instance.ShowMessage("���������ӳɹ���"); } , null);

               }
               catch ( Exception e )
               {
                  
                   Debug.LogError(e.Message);
                   Close();
                   return;
               }


           } , null);
           

        }
        void StartReceive( ) 
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None,ReceiveCallback,null);
        }
        void ReceiveCallback(IAsyncResult ar ) 
        {
            Debug.Log("TCP��ʼ����");
            try 
            {

                int len = socket.EndReceive(ar);
                if ( len == 0 )
                {
                    Debug.Log("TCP��������Ϊ0");
                    Close();
                    return;
                }
                message.ReadBuffer(len , HandleResponse);
                StartReceive();
            } catch ( Exception e ) 
            {

                Debug.LogError(e.Message);
                Close ();
                return;
            
            }
            
        }
        void HandleResponse( MainPack pack ) 
        {
            
            Debug.Log(pack);
            if ( allRequest.ContainsKey(pack.Actioncode)==false ) 
            {
                Debug.Log(pack.Actioncode+"�Ĵ�����������");
                return;
            }

            allRequest[pack.Actioncode].HandleResponse(pack);
        }
        void Close( ) 
        {
            if ( socket != null ) socket.Close(); ;
            
            Debug.Log("TCP���ӶϿ�");
            //Loom.QueueOnMainThread(( a ) => { UIManager.Instance.ShowMessage("�������Ͽ���"); } , null);
            //������Ϸ
            Loom.QueueOnMainThread(( a) => 
            {

                //if ( Application.isPlaying == false ) return;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                UIManager.Instance.ClearStack();
                UIManager.Instance.PushPanel("Login");
                UIManager.Instance.ShowMessage("�������Ͽ���");


            } ,null);
        }
        public void Send( MainPack pack) 
        {
            if ( socket == null || socket.Connected == false ) 
            {
                
                Loom.QueueOnMainThread(( a ) =>
                {
                    UIManager.Instance.ShowMessage("�������Ͽ�");

                } , null);
                return;
            }
            Debug.Log("���ͣ�"+pack);
            socket.Send(Message.PackData(pack));   
        }
    }
   
}
