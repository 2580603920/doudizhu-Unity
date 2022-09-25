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
            Screen.orientation = ScreenOrientation.AutoRotation;//设置方向为自动(根据需要自动旋转屏幕朝向任何启用的方向。)
            Screen.autorotateToLandscapeRight = true;           //允许自动旋转到右横屏
            Screen.autorotateToLandscapeLeft = true;            //允许自动旋转到左横屏
            Screen.autorotateToPortrait = false;                //不允许自动旋转到纵向
            Screen.autorotateToPortraitUpsideDown = false;      //不允许自动旋转到纵向上下
            Screen.sleepTimeout = SleepTimeout.NeverSleep;      //睡眠时间为从不睡眠
        }

    }
}