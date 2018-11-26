using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {
    
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckOnClick(Input.mousePosition, "Touchable");
            
        }

    }
    public void CheckOnClick(Vector3 position, string layerName)
    {
        if (Constructor.instance.isShopEnabled)
            return;

        int layerTouchable = LayerMask.GetMask(layerName);
        RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit,Mathf.Infinity, layerTouchable))
            {
           
             if (hit.transform.GetComponent<ITouchable>() != null)
                {
                    ITouchable objectTouched = hit.transform.GetComponent<ITouchable>();
                    HelpPanel panel = GameObject.FindObjectOfType<HelpPanel>();
           
                    panel.SetPanel(objectTouched);

                }
                else return;
            
            }
            
    }
    

}
