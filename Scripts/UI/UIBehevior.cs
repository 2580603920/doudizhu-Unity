using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Doudizhu
{
    public class UIBehevior : MonoBehaviour
    {
        UIManager uIManager;
        EventTrigger eventTrigger;
        public Text textUI;
        public InputField inputField;
        public Button button;
        public Image image;
        private void Awake( )
        {
            uIManager = UIManager.Instance;

            Initial();
        }
        public void Delete( ) 
        {
            Destroy(this.gameObject);
        }
        void Initial( )
        {
            if(name.EndsWith("_N")) 
                uIManager.Register(GetComponentInParent<PanelCTRLBase>().name , name , transform);
            //if(name.EndsWith("_S") )
            //    GetComponentInParent<SubUImanager>().Register(name,transform);


           eventTrigger = gameObject.AddComponent<EventTrigger>();
            

            textUI = GetComponent<Text>();
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            inputField = GetComponent<InputField>();
        }
       
        public void AddButtonEventListener(UnityAction action ) 
        {
            if ( button == null ) return;

            button.onClick.AddListener(action);

        }
        public void AddEvent( EventTriggerType eventTriggerType , UnityAction<BaseEventData> action )
        {
            foreach ( EventTrigger.Entry item in eventTrigger.triggers )
            {

                if ( item.eventID == eventTriggerType )
                {

                    item.callback.AddListener(action);
                    return;
                }
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();

            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }

        public void RemoveEvent( EventTriggerType eventTriggerType , UnityAction<BaseEventData> action )
        {
            foreach ( EventTrigger.Entry item in eventTrigger.triggers )
            {

                if ( item.eventID == eventTriggerType )
                {

                    item.callback.RemoveListener(action);
                    return;
                }
            }

        }
       
    }
}