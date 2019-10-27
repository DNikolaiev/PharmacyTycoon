using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarFiller : MonoBehaviour {

    public Image toxicityBar;
    public Image healingBar;
    public bool grabText = true;
    public bool initializeWithZero = true;
    public bool  coroutineRunning;
    public float fillSpeed;
    private void Start()
    {
        if(toxicityBar!=null && initializeWithZero)
        toxicityBar.fillAmount = 0;
        if(healingBar!=null && initializeWithZero)
        healingBar.fillAmount = 0;
    }
    public void SetValueToBarPercent(float value, Image img)
    {
        float  percentValue = value / 100;
        percentValue = Mathf.Clamp(percentValue, 0, 1);
        value = Mathf.Clamp(value, 0, 100);
        img.fillAmount = percentValue;
       
      Text  possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText!=null && grabText)
        {
            possibleText.text = ((int)value + "%").ToString();
        }
            
    }
    public void SetValueToBarScalar(float value, Image img, float maxValue)
    {
        img.fillAmount = (float)(value / maxValue);

        Text possibleText = img.transform.parent.GetComponentInChildren<Text>();
        if (possibleText != null && grabText)
        {
            possibleText.text = (value).ToString();
        }

    }
    public IEnumerator ReduceOverTime(Image img, float endAmount, float maxValue, bool isPercentage=true) // img - delayed image, endAmount - final fillAmount 
    {
        coroutineRunning = true;
        endAmount = endAmount / maxValue;
        Text possibleText = img.transform.parent.GetComponentInChildren<Text>();
        while (img.fillAmount > endAmount)
        {
            if (possibleText != null)
            {
                if(isPercentage)
                possibleText.text = ((int)(img.fillAmount * maxValue) + "%").ToString();
                else
                {
                    possibleText.text = ((int)(img.fillAmount * maxValue)).ToString();
                }
            }
            img.fillAmount -= fillSpeed*Time.deltaTime;
           
            yield return null;
        }
        if(isPercentage)
        possibleText.text = (endAmount * maxValue).ToString() + " %";
        else possibleText.text = (endAmount * maxValue).ToString();
        coroutineRunning = false;
    }
    public IEnumerator IncreaseOverTime(Image img, float endAmount, float maxValue, bool isPercentage=true) // img - delayed image, endAmount - final fillAmount 
    {
        coroutineRunning = true;
        endAmount = endAmount / maxValue;
        Text possibleText = img.transform.parent.GetComponentInChildren<Text>();
        while (img.fillAmount <= endAmount)
        {
            if (possibleText != null)
            {
                if (isPercentage)
                    possibleText.text = ((int)(img.fillAmount * maxValue) + "%").ToString();
                else
                {
                    possibleText.text = ((int)(img.fillAmount * maxValue)).ToString();
                }
            }
            img.fillAmount += fillSpeed*Time.deltaTime;

            yield return null;
        }
        if (possibleText != null)
        {
            if (isPercentage)
                possibleText.text = (endAmount * maxValue).ToString() + " %";
            else possibleText.text = (endAmount * maxValue).ToString();
        }
        coroutineRunning = false;
    }
   
    private void OnEnable()
    {
        if (toxicityBar != null && initializeWithZero)
        {
            SetValueToBarPercent(0, toxicityBar);
        }
        if (healingBar != null && initializeWithZero)
        {
            SetValueToBarScalar(0, healingBar,100);
        }
        
       
    }
}
