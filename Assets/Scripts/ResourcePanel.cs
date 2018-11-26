using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourcePanel : Panel
{
    public Text herbs, chems, plastic, researchPoints, money;
    public Image herbsImg, chemsImg, plasticImg, rpImg, moneyImg;
    public BarFiller bar;
    public bool autoGenerate;
   [SerializeField] private CraftHolder craftHolder;
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdatePanel()
    {
        if(craftHolder!=null)
        SetPanel(craftHolder.Talent.characteristics);
    }
    public  void SetPanel(ResourceStorage storage)
    {
        gameObject.SetActive(true);

    }
    public void SetPanel(Characteristics ch)
    {
        if(!gameObject.activeInHierarchy)
         gameObject.SetActive(true);
        if (bar!=null)
        {
            bar.SetValueToBar(ch.toxicity, bar.toxicityBar);
            bar.SetValueToBar(ch.healingRate, bar.healingBar);
        }
        else if(Nametxt!=null && craftHolder!=null)
            Nametxt.text = craftHolder.Talent.description.Name;
        herbs.text = ch.healingPlantsNeeded.ToString();
        chems.text = ch.chemistryNeeded.ToString();
        plastic.text = ch.plasticNeeded.ToString();
        

    }
    private void Start()
    {
        
        if (autoGenerate)
        {
           
            craftHolder = GetComponentInChildren<CraftHolder>();
            SetPanel(craftHolder.Talent.characteristics);
        }
        else
        SetPanel();

    }
    public override void SetPanel()
    {
        if (herbs == null || chems == null || plastic == null)
            return;
        Text[] txtarray = { herbs, chems, plastic };
        foreach (Text txt in txtarray)
            txt.text = "0";
    }
}
