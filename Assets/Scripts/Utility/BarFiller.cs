using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarFiller : MonoBehaviour {

    public Image toxicityBar;
    public Image healingBar;
    public bool  coroutineRunning;
    private void Start()
    {
        toxicityBar.fillAmount = 0;
        healingBar.fillAmount = 0;
    }
    public void SetValueToBarPercent(float value, Image img)
    {
        float  percentValue = value / 100;
        percentValue = Mathf.Clamp(percentValue, 0, 1);
        value = Mathf.Clamp(value, 0, 100);
        img.fillAmount = percentValue;
       
      Text  possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText!=null)
        {
            possibleText.text = (value + "%").ToString();
        }
            
    }
    public IEnumerator FillWithDelay(Image img, float endAmount) // img - delayed image, endAmount - final fillAmount 
    {
        coroutineRunning = true;
        Text possibleText = img.transform.parent.GetComponentInChildren<Text>();
        while (img.fillAmount > endAmount)
        {
            if (possibleText != null)
            {
                possibleText.text = ((int)(img.fillAmount * 100) + "%").ToString();
            }
            img.fillAmount -= 0.005f;
           
            yield return null;
        }
        coroutineRunning = false;
    }
    public void SetValueToBarScalar(float value, Image img)
    {

        img.fillAmount = value / 100;
        Text possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText != null)
        {
            possibleText.text = (value).ToString();
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
