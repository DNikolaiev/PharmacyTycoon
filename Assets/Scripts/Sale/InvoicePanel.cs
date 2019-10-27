using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class InvoicePanel : Panel
{
    public Text transfertxt;
    public Text incometxt;
    public Text bonustxt;
    public Text summarytxt;
    public Text quantitytxt;
    public Text quotumText;
    [SerializeField] PlayerPanel pPanel;
    [SerializeField] Slider slider;
    [SerializeField] BarFiller bar;
    [SerializeField] Invoice invoice;
    Area area;
    [SerializeField] GameObject backGround;
    [SerializeField] Animation anim;
    
    public override void Hide()
    {
        for (int i = 0; i < backGround.transform.childCount; i++)
        {
            backGround.transform.GetChild(i).gameObject.SetActive(false);
        }

        backGround.GetComponent<Image>().enabled = false;
        slider.value = slider.minValue;
    }
    private void Start()
    {
        Hide();

    }
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    private void UpdateInvoiceView(Invoice invoice)
    {
        
        transfertxt.text = "- "+invoice.transferCost.ToString();
        incometxt.text = (invoice.price * invoice.quantity).ToString();
        bonustxt.text = "x"+invoice.bonus.ToString();
        summarytxt.text = invoice.Summary.ToString();
        quantitytxt.text = "x" + invoice.quantity;
        if (RecipeSelector.IsSelected)
            bar.SetValueToBarScalar(area.health + RecipeSelector.recipeHolderSelected.recipe.characteristics.healingRate * invoice.quantity, bar.toxicityBar, area.maxHealth);
    }
    public void SetPanel(Invoice invoice, Area area)
    {
        if (!GameController.instance.generalTutorial.isTutorialCompleted) invoice.transferCost = 0;
        this.invoice=invoice;
        this.area = area;
        invoice.quantity = (area.soldItems == area.maxQuotum) ? 0 : 1;
        backGround.SetActive(true);
        
        for (int i = 0; i < backGround.transform.childCount; i++)
        {
           backGround.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        Nametxt.text = area.Name;
        UpdateInvoiceView(invoice);
        int maxToSell = Mathf.Min(GameController.instance.player.inventory.GetQuantity(RecipeSelector.recipeHolderSelected.recipe.description.Name), (area.maxQuotum - area.soldItems));
        slider.maxValue = maxToSell;
        slider.minValue = (area.soldItems == area.maxQuotum) ? 0 : 1;
        slider.value =  slider.minValue;
        if (!FindObjectOfType<Seller>().GetTutorialState()) GetComponent<PanelMask>().enabled = false;
        else GetComponent<PanelMask>().enabled = true;
       
        if (bar != null )
        {
            //hold still
            bar.SetValueToBarScalar(area.health, bar.healingBar, area.maxHealth);
            bar.healingBar.color = FindObjectOfType<Seller>().view.areaHealthBar.healingBar.color;
            bar.SetValueToBarScalar(area.health + RecipeSelector.recipeHolderSelected.recipe.characteristics.healingRate * invoice.quantity, bar.toxicityBar,area.maxHealth);
        }
        pPanel.PrognoseExperience(invoice.quantity * 10 * (area.experienceMultiplier + (RecipeSelector.recipeHolderSelected.recipe.Talents.Count) / 4));
        pPanel.SetPanel();
        if (area.maxQuotum < 2000)
        {
         
            quotumText.text = "Items sold " + area.soldItems + " / " + area.maxQuotum;
            quotumText.gameObject.transform.parent.gameObject.SetActive(true);
            anim.Play("InvoicePanel_Appear");
        }
        else
        {
            quotumText.gameObject.transform.parent.gameObject.SetActive(false);
            anim.Play("InvoicePanel_Appear2");
        }

    }
    public void QuantityChange(float value)
    {
        
    
        invoice.quantity = (int)value;

        if (invoice.quantity > 1)
        {
            pPanel.PrognoseExperience(invoice.quantity * 10 * area.experienceMultiplier);
        }
        pPanel.SetPanel();
        UpdateInvoiceView(this.invoice);
    }

    public void AddQuantity(int amount)
    {
        if ((invoice.quantity == slider.maxValue && amount > 0) || (invoice.quantity == slider.minValue && amount < 0)) return;
        Debug.Log("added");
        invoice.quantity+= amount;
        slider.value = invoice.quantity;
        Debug.Log(invoice.quantity);
        UpdateInvoiceView(this.invoice);
        bar.SetValueToBarScalar(area.health + RecipeSelector.recipeHolderSelected.recipe.characteristics.healingRate * invoice.quantity, bar.toxicityBar, area.maxHealth);
        pPanel.PrognoseExperience(invoice.quantity * 10*area.experienceMultiplier);
        pPanel.SetPanel();
    }
    public void Accept()
    {
       FindObjectOfType<Seller>().Sell(RecipeSelector.recipeHolderSelected.recipe, invoice);
        
        RecipeSelector.UnSelectRecipe();
        Hide();
    }

}
