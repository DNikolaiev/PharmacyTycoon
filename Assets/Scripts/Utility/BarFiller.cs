using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarFiller : MonoBehaviour {

    public Image toxicityBar;
    public Image healingBar;
    private Text possibleText;
    private float value;
    private void Start()
    {
        toxicityBar.fillAmount = 0;
        healingBar.fillAmount = 0;
    }
    public void SetValueToBar(float value, Image img)
    {
        this.value = value;
        img.fillAmount = value / 100;
        possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText!=null)
        {
            possibleText.text = (value + "%").ToString();
        }
            
    }
    private void OnEnable()
    {
        if(possibleText!=null)
        possibleText.text = "0 %";
        toxicityBar.fillAmount = 0;
        healingBar.fillAmount = 0;
    }
}
