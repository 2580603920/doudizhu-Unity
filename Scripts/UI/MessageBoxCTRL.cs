using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Doudizhu { 
    public class MessageBoxCTRL : PanelCTRLBase
    {
        Image bg;

        public override void Awake( )
        {
            base.Awake();
            bg = GetComponent<Image>();
        }
        public override void Enter( )
        {
            base.Enter();
            transform.parent.gameObject.SetActive(true);
            transform.parent.SetAsLastSibling();


        }
        public override void Exit( )
        {
            base.Exit();
            transform.parent.gameObject.SetActive(false);
           
        }

        Tweener tweener =null;
      
        public void ShowInfo(string content ) 
        {
            if ( tweener != null && tweener.IsPlaying() ) 
            {
                tweener.Restart();
            }

            GetUIBehevior("MessageBoxText_N").textUI.text = content;
            //bg.color = new Color(0,0,0,0);
            tweener = bg.DOFade(1 , 1.0f);
            tweener.SetAutoKill(false);
            tweener.SetLoops(2,LoopType.Yoyo);
            tweener.OnStart(Enter);
            tweener.onComplete += Exit;
        }
       

    }
  

}