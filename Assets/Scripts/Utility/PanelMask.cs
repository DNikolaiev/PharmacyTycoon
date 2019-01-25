using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMask : MonoBehaviour {

    private void CloseIfClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransform rectTransform =GetComponent<RectTransform>();

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
                GetComponent<Panel>().Hide();
            else return;
        }
    }
    private void Update()
    {
        CloseIfClickedOutside();
    }
}
