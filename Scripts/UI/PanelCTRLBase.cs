using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using SoketDoudizhuProtocol;



namespace Doudizhu
{
    public class PanelCTRLBase : MonoBehaviour
    {
        [HideInInspector]public UIManager uIManager;
        protected float timer;

        public virtual void Awake( )
        {

            uIManager = UIManager.Instance;
            Initial();
        }
        public virtual void Enter( ) { }
        public virtual void Stay( float delta ) 
        {
            timer += delta;
        }
        public virtual void Exit( ) { }
        public virtual void HandleResponse(MainPack pack ) { }
        public void ShowMessage( string content ) 
        {
            uIManager.ShowMessage( content );
        
        }
        public void Pop()
        {
            uIManager.PopPanel();

        }
        public void Push(string panelName )
        {
            uIManager.PushPanel(panelName);

        }
        public virtual void Initial( )
        {

            Transform[] childrens = transform.GetComponentsInChildren<Transform>();

            foreach ( Transform item in childrens )
            {
                if ( item.name.EndsWith("_N") )
                {

                    item.gameObject.AddComponent<UIBehevior>().enabled = true;

                }

            }

            uIManager.AddPanel(name , this);
        }
        public UIBehevior GetUIBehevior( string componentName )
        {
            if( uIManager.GetUITrans(name , componentName) == null)return null;
            return uIManager.GetUITrans(name , componentName).GetComponent<UIBehevior>();

        }
        public Transform GetUITrans( string componentName )
        {
            
            return uIManager.GetUITrans(name , componentName);

        }
        public void RenameUI( string oldName,string newName ) 
        {
            uIManager.RenameUI(name, oldName , newName);
        
        }
        public void AddEvent( string componentName , EventTriggerType eventTriggerType , UnityAction<BaseEventData> action )
        {
            GetUIBehevior(componentName).AddEvent(eventTriggerType , action);
        }
        public void RemoveEvent( string componentName , EventTriggerType eventTriggerType , UnityAction<BaseEventData> action )
        {
            GetUIBehevior(componentName).RemoveEvent(eventTriggerType , action);

        }
        public Transform LoadUI(string uiName,Transform parent ) 
        {
           
             return   uIManager.LoadUI(uiName , parent);

        }
    }
}