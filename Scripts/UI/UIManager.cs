using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Doudizhu
{
    public class UIManager : MonoBehaviour
    {
        Dictionary<string , Dictionary<string , Transform>> allUIComponent;

        public Stack<PanelCTRLBase> PanelStack;
        Dictionary<string , PanelCTRLBase> allCTRL;

        static UIManager instance;
        string prefabPath = "Prefabs/UI/";

        public static UIManager Instance
        {
            get
            {
                if ( instance == null ) 
                {
                    instance = GameObject.FindWithTag("MainCanvas").AddComponent<UIManager>();
                }
                    
                return instance;
            }
        }

        private void Awake( )
        {
            
            allUIComponent = new Dictionary<string , Dictionary<string , Transform>>();
            PanelStack = new Stack<PanelCTRLBase>();
            allCTRL = new Dictionary<string , PanelCTRLBase>();
        }
        public void Initial( ) 
        {

            PushPanel("Login");
        }
        private void Start( )
        {

           
        }
        private void Update( )
        {
            if ( PanelStack.Count != 0 ) 
            {
                PanelStack.Peek().Stay(Time.deltaTime);

            }
        }
        public void AddPanel( string panelName , PanelCTRLBase panelCTRLBase )
        {
            if ( allCTRL.ContainsKey(panelName) )
            {
                Debug.LogWarning(panelName + "面板已存在");
                return;
            }
            allCTRL.Add(panelName , panelCTRLBase);

        }
        public PanelCTRLBase GetPanel( string panelName )
        {
            if ( allCTRL.ContainsKey(panelName) )
            {
                return allCTRL[panelName];
            }

            return null;
        }
        public void RemovePanel( string panelName )
        {
            if ( allCTRL.ContainsKey(panelName) )
            {
                allCTRL.Remove(panelName);
            }


        }
        public void Register( string panelName , string componentName , Transform transform )
        {

            if ( !allUIComponent.ContainsKey(panelName) )
            {
                allUIComponent.Add(panelName , new Dictionary<string , Transform>());
            }
            if ( allUIComponent[panelName].ContainsKey(componentName) )
            {
                Debug.LogWarning(componentName + "UI组件已经存在");
                return;

            }
            allUIComponent[panelName].Add(componentName , transform);

        }

        public void UnRegister( string panelName , string componentName )
        {

            if ( !allUIComponent.ContainsKey(panelName) )
                return;
            if ( allUIComponent[panelName].ContainsKey(componentName) )
                allUIComponent[panelName].Remove(componentName);

        }
        public Transform GetUITrans( string panelName , string componentName )
        {
            Transform trans = null;
            if ( allUIComponent.ContainsKey(panelName) )
                trans = allUIComponent[panelName][componentName];
            return trans;

        }
        public  void PushPanel(string panelName)
        {
               
                if ( !allCTRL.ContainsKey(panelName) )
                {
                    LoadUI(panelName , transform);
                }
          
                  

                if ( allCTRL.ContainsKey(panelName) )
                    PanelStack.Push(allCTRL[panelName]);
            
            PanelStack.Peek().Enter();                
        }
        public void PopPanel( ) 
        {

           
            if ( PanelStack.Count == 0 ) return;
            PanelCTRLBase topItem = PanelStack.Pop();
            topItem.Exit();    
        }
        public void ClearStack( ) 
        {
            while ( PanelStack.Count != 0 ) 
            {
                PopPanel();
            
            }
        }

        //显示或隐藏
        void HidePanel( string panelName , bool isHide = true )
        {
            allCTRL[panelName].transform.parent.gameObject.SetActive(!isHide);

        }

        public Transform LoadUI( string name,Transform parent)
        {

            GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>(prefabPath + name));
            temp.name = name;
            temp.transform.SetParent(parent , false);
            temp.SetActive(true);
            
            return temp.transform;
        }
        public void RenameUI(string panelName,string oldname,string newName ) 
        {
            Transform tempTrans = GetUITrans(panelName , oldname);
            tempTrans.name = newName;
            UnRegister(panelName , oldname);
            Register(panelName,newName,tempTrans);
        }
        public void ShowMessage(string content ) 
        {

            if ( !allCTRL.ContainsKey("MessageBox") )
                LoadUI("MessageBox" , transform);

            
             ( GetPanel("MessageBox") as MessageBoxCTRL ).transform.parent.SetAsLastSibling();
            
            ( GetPanel("MessageBox") as MessageBoxCTRL ).ShowInfo(content);
                
           
        }

    }
}