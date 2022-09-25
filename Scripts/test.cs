using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class test : MonoBehaviour
{
    GraphicRaycaster raycaster;
    [SerializeField]EventSystem eventSystem;
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = eventSystem.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    private void Update( )
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = Input.mousePosition;
            raycaster.Raycast(eventData , raycastResults);
            if ( raycastResults.Count != 0 )
            Debug.Log(raycastResults[0].gameObject.name);
        }
    }
}
