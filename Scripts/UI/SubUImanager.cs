using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Doudizhu
{ 
    public class SubUImanager : MonoBehaviour
    {
        UIManager uIManager;
        EventTrigger eventTrigger;
        Dictionary<string, Transform> allSubUI;

        private void Awake( )
        {
            uIManager = UIManager.Instance;

            Initial();
           
        }
        void Initial( )
        {
            if(name.EndsWith("_N") && GetComponent<UIBehevior>()==null)
                gameObject.AddComponent<UIBehevior>();
            allSubUI = new Dictionary<string, Transform>();

            Transform[] temp = transform.GetComponentsInChildren<Transform>();
            foreach ( var item in temp )
            {

                if ( item.name.EndsWith("_S") )
                {
                    item.gameObject.AddComponent<UIBehevior>();
                    Register(item.name , item.transform);
                }

            }
        }
        public void Register(string componentName , Transform transform )
        {

            if ( !allSubUI.ContainsKey(componentName) )
            {
                allSubUI.Add(componentName , transform);
            }
            if ( allSubUI.ContainsKey(componentName) )
            {
                Debug.LogWarning(componentName + "UI组件已经存在");
                return;

            }
            allSubUI.Add(componentName , transform);

        }

        public void UnRegister( string componentName )
        {

            if ( !allSubUI.ContainsKey(componentName) )
                return;
            if ( allSubUI.ContainsKey(componentName) )
                allSubUI.Remove(componentName);

        }
        public UIBehevior GetUIBehevior( string componentName )
        {
            UIBehevior uiBehevior = null;
          
            if ( allSubUI.ContainsKey(componentName) )
                uiBehevior = allSubUI[componentName].GetComponent<UIBehevior>();
            return uiBehevior;

        }
        public Transform GetUITrans( string componentName )
        {
            Transform trans = null;
            if ( allSubUI.ContainsKey(componentName) )
                trans = allSubUI[componentName];
            return trans;

        }
    }
}
