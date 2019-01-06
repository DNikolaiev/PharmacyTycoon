using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarFiller : MonoBehaviour {

    public Image toxicityBar;
    public Image healingBar;
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
      Text  possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText!=null)
        {
            possibleText.text = (value + "%").ToString();
        }
            
    }
    private void OnEnable()
    {
        Text possibleText = toxicityBar.transform.parent.GetComponentInChildren<Text>();
        Text possibleText2 = healingBar.transform.parent.GetComponentInChildren<Text>();
        if (possibleText!=null)
        possibleText.text = "0 %";
        if (possibleText2 != null)
            possibleText2.text = "0 %";
        toxicityBar.fillAmount = 0;
        healingBar.fillAmount = 0;
    }
}
