using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ResourcePanel : Panel
{
    public Text herbs, chems, plastic, researchPoints, money;
    public Image herbsImg, chemsImg, plasticImg, rpImg, moneyImg;
    public BarFiller bar;
    public bool autoGenerate;
   [SerializeField] private CraftHolder craftHolder;
    [SerializeField] private RecipeHolder recipeHolder;
    [SerializeField] private ColorInterpolator interpol;
   
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
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        if (herbs != null)
        {
            CheckForChanges(herbs, storage.currentHealingPlants, Color.green);
           
         
            herbs.text = storage.currentHealingPlants.ToString() + " / " + storage.MaxHealingPlants.ToString();
           
           
        }
        if(chems != null )
        {
            CheckForChanges(chems, storage.currentChemistry, Color.magenta);
            chems.text = storage.currentChemistry.ToString() + " / " + storage.MaxChemistry.ToString();
        }
        if(plastic != null)
        {
            CheckForChanges(plastic, storage.currentPlastic, Color.white);
            plastic.text = storage.currentPlastic.ToString() + " / " + storage.MaxPlastic.ToString();
        }
        if (researchPoints != null)
        {
            CheckForChanges(researchPoints, storage.ResearchPoints, Color.cyan);
            
            researchPoints.text = storage.ResearchPoints.ToString();
        }
        if (money != null)
        {

            CheckForChanges(money,storage.money,Color.green);
            money.text = storage.money.ToString();
        }

        
    }
    private int ParseResource(string text)
    {
        int res = 0;
        if(!text.Contains("/"))
        {
            Int32.TryParse(text, out res);
        }
        else
        {
            int found = text.IndexOf("/");
            Int32.TryParse(text.Substring(0, found), out res);
        }

        return res;
    }
    public bool CheckForChanges(Text text, int value, Color addColor)
    {
        int oldValue = ParseResource(text.text);
      
        if (oldValue - value < 0)
        {
            StartCoroutine(interpol.InOut(text, addColor));
            StartCoroutine(GetComponent<Scaler>().Scale(text));
            text.transform.parent.GetComponentInChildren<ParticleSystem>().Play();
            return true;
        }
        else if (oldValue - value >0)
        {
            StartCoroutine(interpol.InOut(text, Color.red));
            StartCoroutine(GetComponent<Scaler>().Scale(text));
            return true;
        }
         return false;
    }
   
    public void SetPanel(Characteristics ch)
    {
        if(!gameObject.activeInHierarchy)
         gameObject.SetActive(true);
        if (bar!=null)
        {
            bar.SetValueToBarPercent(ch.toxicity, bar.toxicityBar);
            bar.SetValueToBarScalar(ch.healingRate, bar.healingBar, 100);
        }
        else if(Nametxt!=null && craftHolder!=null)
            Nametxt.text = craftHolder.Talent.description.Name;
        else if (Nametxt != null && recipeHolder != null)
            Nametxt.text = recipeHolder.recipe.description.Name;
        herbs.text = ch.healingPlantsNeeded.ToString();
        chems.text = ch.chemistryNeeded.ToString();
        plastic.text = ch.plasticNeeded.ToString();
        

    }
    
    private void Start()
    {
        if (herbsImg != null && chemsImg != null && plasticImg != null)
        {
            herbsImg.sprite = Resources.Load<Sprite>("Icons/herbs");
            chemsImg.sprite = Resources.Load<Sprite>("Icons/chemistry");
            plasticImg.sprite = Resources.Load<Sprite>("Icons/plastic");
        }
        if (rpImg != null)
            rpImg.sprite = Resources.Load<Sprite>("Icons/researchPoint");
        if (moneyImg != null)
        {
            moneyImg.sprite = Resources.Load<Sprite>("Icons/money");
            interpol.originalColor = money.color;
        }
        if (autoGenerate)
        {
           
            craftHolder = GetComponentInChildren<CraftHolder>();
            recipeHolder = GetComponentInChildren<RecipeHolder>();
            if (craftHolder != null)
                SetPanel(craftHolder.Talent.characteristics);
            else if (recipeHolder != null)
                SetPanel(recipeHolder.recipe.characteristics);
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
