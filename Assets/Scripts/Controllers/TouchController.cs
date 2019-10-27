using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {
    
    
    private void Update()
    {
        
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                CheckOnClick(Input.mousePosition, "Touchable");

            }
        }
        if (Application.isMobilePlatform)
        {
            foreach(Touch t in Input.touches)
            {
                if(t.phase==TouchPhase.Began)
                CheckOnClick(t.position, "Touchable");
            }
        }

    }
    public void CheckOnClick(Vector3 position, string layerName)
    {
        if (!GameController.instance.IsGameSceneEnabled)
            return;

        int layerTouchable = LayerMask.GetMask(layerName);
        RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit,Mathf.Infinity, layerTouchable))
            {
           
             if (hit.transform.GetComponent<ITouchable>() != null && GameController.instance.IsGameSceneEnabled)
                {
                    ITouchable objectTouched = hit.transform.GetComponent<ITouchable>();
            
                    HelpPanel panel = GameObject.FindObjectOfType<HelpPanel>();
                    if (!panel.IsPointerOverPanel())
                    panel.SetPanel(objectTouched);

                }
             else if (hit.transform.GetComponent<IMultitouchable>()!=null && GameController.instance.IsGameSceneEnabled)
            {
                IMultitouchable multitouchable = hit.transform.GetComponent<IMultitouchable>();
                multitouchable.MultiTouch();
            }
                else return;
            
            }
            
    }
    

}
