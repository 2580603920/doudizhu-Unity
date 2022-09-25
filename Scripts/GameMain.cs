using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doudizhu
{
    public class GameMain : MonoBehaviour
    {
        Loom loom;

        UIManager uiManager;
        RequestManager requestManager;
        public static bool isCreate;

        static GameMain instance;

        public static GameMain Instance 
        { 
            get => instance;
        }

        private void Awake( )
        {
            if ( isCreate  )
                Destroy(gameObject);
            else
               
            {
                DontDestroyOnLoad(gameObject);
                isCreate = true;
                instance= this;
                Initial();
            }
            
        }
       

        void Initial( ) 
        {
            loom = Loom.Current;
            requestManager = RequestManager.Instance;
            requestManager.Initial();
            uiManager = UIManager.Instance;
            uiManager.Initial();
            requestManager.InitialSocket();
            Screen.orientation = ScreenOrientation.AutoRotation;//���÷���Ϊ�Զ�(������Ҫ�Զ���ת��Ļ�����κ����õķ���)
            Screen.autorotateToLandscapeRight = true;           //�����Զ���ת���Һ���
            Screen.autorotateToLandscapeLeft = true;            //�����Զ���ת�������
            Screen.autorotateToPortrait = false;                //�������Զ���ת������
            Screen.autorotateToPortraitUpsideDown = false;      //�������Զ���ת����������
            Screen.sleepTimeout = SleepTimeout.NeverSleep;      //˯��ʱ��Ϊ�Ӳ�˯��
        }

    }
}