using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DescriptionPanel :  Panel {
    [SerializeField] Text type;
    [SerializeField] Text cures;
    [SerializeField] Text toxicity;
    [SerializeField] Text healingRate;
    [SerializeField] TalentHolder[] talents;
    [SerializeField] bool startAtOffset;
    [SerializeField] Animation slideAnim;
    [SerializeField] Scrollbar scroll;
    [SerializeField] float slideTo = 1;
    private RectTransform rect;
    public bool isActive;
    public override void Hide()
    {
        isActive = false;
        if(slideAnim!=null)
            slideAnim.Play("SlideBackwards_DescriptionPanel");
    }
    public void Clear()
    {
        if (Nametxt != null)
            Nametxt.text = "Select recipe";
        if (type != null)
            type.text = "to see it";
        foreach( TalentHolder t in talents)
        {
            t.Talent = null;
            t.picture.sprite = t.defaultSprite;
           
        }

    }
    public void SetPanel(Talent talent, TalentHolder holder)
    {
        if (talent == null) return;
        ChangeView(false);
        SetMainInformation(talent);
        // show resource & time costs, images
        
    }
    
    public void SetPanel(Recipe recipe, RecipeHolder holder)
    {
        if (recipe.Talents.Count==0  || holder==null||recipe==null) return;
        ChangeView(true);
        if(scroll!=null)
            StartCoroutine(SlideValueOverTime(slideTo));
        SetMainInformation(recipe);

        TalentHolder[] primaryHolders = { talents[0], talents[1], talents[2] };
        TalentHolder[] secondaryHolders = { talents[3], talents[4], talents[5], talents[6] };
        foreach (TalentHolder t in talents)
            t.gameObject.SetActive(true);
        for (int i = 0; i < primaryHolders.Length; i++)
        {

           primaryHolders[i].Talent = null;

            if (i >= recipe.PTalents.Count)
            {
                continue;
            }
            primaryHolders[i].Talent = recipe.PTalents[i];
        }
        for (int i = 0; i < secondaryHolders.Length; i++)
        {

            secondaryHolders[i].Talent = null;

            if (i >= recipe.STalents.Count)
            {
                continue;
            }
            secondaryHolders[i].Talent = recipe.STalents[i];
        }
        foreach(TalentHolder t in talents)
        {
            t.SetPanel();
        }
    }
    IEnumerator SlideValueOverTime( float to)
    {
        if (scroll.value < to)
        {
            while (scroll.value < to)
            {
                scroll.value += 0.05f;
                yield return null;
            }
        }
        else
        {
            while(scroll.value>to)
            {
                scroll.value -= 0.05f;
                yield return null;
            }
        }
    }
    private void ChangeView(bool state)
    {
        if (toxicity == null) return;
        Transform formPanel = toxicity.transform.parent.parent.parent.GetChild(1);
        Transform mainDescription = toxicity.transform.parent;
        if (state)
        {
            mainDescription.GetComponent<RectTransform>().offsetMin = new Vector2(0, -50);

        }
        else
        {
            mainDescription.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            foreach(TalentHolder t in talents)
            {
                t.gameObject.SetActive(false);
            }
        }

        toxicity.gameObject.SetActive(!state);
        healingRate.gameObject.SetActive(!state);
        cures.gameObject.SetActive(!state);
        formPanel.gameObject.SetActive(!state);
        
    }
    private void SetMainInformation(Recipe recipe)
    {
        isActive = true;
        gameObject.SetActive(true);
        if(slideAnim!=null)
             slideAnim.Play("Slide_DescriptionPanel");
        if (type!=null)
        {
            if (recipe.isLiquid)
                type.text = "Liquid";
            else type.text = "Pills";
        }
        if (cures != null)
        {
            cures.text = "<color='red'>Сures </color> ";

            foreach (Talent tal in recipe.Talents)
            {
                if (tal == recipe.Talents[recipe.Talents.Count - 1])
                    cures.text += tal.cures + ".";
                else
                    cures.text += tal.cures + ", ";
            }
        }
        
        Nametxt.text = recipe.description.Name;
        if(toxicity!=null)
             toxicity.text = "<color='green'>Toxicity: </color>" + recipe.characteristics.toxicity.ToString() + " %" ;
        if(healingRate!=null)
            healingRate.text = "<color='red'>Healing: </color>" + recipe.characteristics.healingRate.ToString() ;
    }
    private void SetMainInformation(Talent talent)
    {
        isActive = true;
        gameObject.SetActive(true);
        if(slideAnim!=null)
            slideAnim.Play("Slide_DescriptionPanel");
        toxicity.text = "<color='lime'>Toxicity</color>: " + talent.characteristics.toxicity.ToString() + " %";
        cures.text = "<color='red'>Cures </color>: " + talent.cures;
        healingRate.text = "<color='red'>Healing</color>: " + talent.characteristics.healingRate.ToString();
        if (talent.isPrimary)
            type.text = "<color='orange'>Primary</color>";
        else type.text = "<color='orange'>Secondary</color>";
        Nametxt.text = talent.description.Name;

    }
    public override void SetPanel()
    {
        if(Nametxt!=null)
        Nametxt.text = "Select recipe";
        if(type!=null)
             type.text = "To see it";
        
        
    }

    // Use this for initialization
    void Start () {
        rect = GetComponent<RectTransform>();
        ReturnToDefault();
        SetPanel();
        if (startAtOffset)
        {
            
            ReturnToOrigin();
        }
        
	}
    public  void ReturnToOrigin()
    {
        if (rect == null) return;
            rect.offsetMin = new Vector2(300, 0);
            rect.offsetMax = new Vector2(300, 0);
        

    }
    public  void ReturnToDefault()
    {
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
    }

}
