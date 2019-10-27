using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PaymentPanel : Panel
{
    public GameObject incomePanel;
    public GameObject expencesPanel;
    public GameObject textPrefab;
    public Color incomeColor;
    public Color expencesColor;
    public override void Hide()
    {
        incomePanel.SetActive(false);
        expencesPanel.SetActive(false);
    }
    public void SetPanel(int money)
    {
        GameObject instantiated;
        incomePanel.SetActive(true);
        if (money >= 0)
        {
            
           
            instantiated = Instantiate(textPrefab, incomePanel.transform);
          
            instantiated.GetComponent<Text>().text = "+" + money;
            instantiated.GetComponent<Text>().color = incomeColor;
        }
        else
        {
            
            
             instantiated = Instantiate(textPrefab, incomePanel.transform);
            
            instantiated.GetComponent<Text>().text =  money.ToString();
            instantiated.GetComponent<Text>().color = expencesColor;
        }
        
    }
    
    public override void SetPanel()
    {
        
    }
   
}
