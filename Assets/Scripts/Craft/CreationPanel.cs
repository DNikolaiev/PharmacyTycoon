using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ColorInterpolator))]
public class CreationPanel : Panel {
    #region inspector
    public Animation popAnim;
    public Image blackBack;
    public Image redBorder;
    public Text quantityText;
    public InputField input;
    public Slider slider;
    public ResourcePanel rPanel;
    public Text header;
    public Button ok;
    #endregion
    public int quantity;
    public int maxQuantity = 500;
    private ColorInterpolator interpol;
    private Characteristics characteristics;
    private Crafter crafter;
    private void Awake()
    {

        interpol = GetComponent<ColorInterpolator>();
        gameObject.SetActive(true);
    }
    public override void Hide()
    {
        blackBack.enabled = false;
        crafter.view.EmitLastParticles();
         gameObject.SetActive(false);
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
    }

    public  void SetPanel(Characteristics characteristics)
    {
        crafter.view.DisableAllParticles();
        blackBack.enabled = true;
        gameObject.SetActive(true);
        slider.value = 0;
        quantity = 1;
        popAnim.Play("Pop_out_CreationPanel");
        this.characteristics = characteristics;
        rPanel.SetPanel(characteristics);
        rPanel.researchPoints.text = crafter.CalculateResearchPoints(crafter.selectedTalents).ToString();
        Recipe testRecipe = crafter.RecognizeRecipe();

        if (testRecipe != null)
        {
            crafter.recipeSelected = testRecipe;
            crafter.isPrescripted = true;
        }
        if (crafter.isPrescripted)
        {
            ok.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            rPanel.researchPoints.gameObject.SetActive(false);
            rPanel.rpImg.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
            header.text = crafter.recipeSelected.description.Name;
        }
        else
        {
            ok.GetComponent<RectTransform>().offsetMax = new Vector2(-120, 0);
            rPanel.researchPoints.gameObject.SetActive(true);
            rPanel.rpImg.gameObject.SetActive(true);
            input.gameObject.SetActive(true);
            header.text = "Create your new recipe!";
        }

    }
    

    // Use this for initialization
    void Start () {
        crafter = GameController.instance.crafter;
        blackBack.enabled = false;
        redBorder.enabled = false;
        gameObject.SetActive(false);
        rPanel.researchPoints.gameObject.SetActive(false);
        rPanel.rpImg.gameObject.SetActive(false);
	}

    public void QuantityChange(float value)
    {
        characteristics /=  quantity;
        quantity = (int)value;
        characteristics *= quantity;
    }

    public void AddQuantity(int amount)
    {
        characteristics /= quantity;
        quantity += amount;
        quantity = Mathf.Clamp(quantity, 1, maxQuantity);
        characteristics *= quantity;
    }
    private void NotifyAboutResources()
    {
        Text[] texts = { rPanel.herbs, rPanel.chems, rPanel.plastic, rPanel.researchPoints };
        foreach(Text t in texts)
        {
            StartCoroutine(interpol.PingPong(t, Color.red));
        }
    }
    public void Accept()
    {

        if (crafter.isPrescripted)
        {
            string result = crafter.Craft(quantity);
            if (result == "TRUE")
                Hide();
            else
            {
                NotifyAboutResources();
                return;
            }
        }
        else
        {
            // get name from input and craft new recipe with this name
            string recipeName = input.textComponent.text;
            if (recipeName == string.Empty)
            {
                recipeName = GenerateName();
            }
            Validator validate = new Validator();
            if (!validate.ValidateRecipeName(recipeName) && recipeName!="Invalid name!")
            {
                input.textComponent.text = "";
                input.text = "Invalid name!";
                redBorder.enabled = true;
                return;
            }
            int researchPointsNeeded = crafter.CalculateResearchPoints(crafter.selectedTalents);
          
            redBorder.enabled = false;
            string result = crafter.Craft(recipeName, quantity);
            if (result == "TRUE") 
                Hide();
            else if (result=="FALSE")
            {
                NotifyAboutResources();
                return;
            }
            else if (result=="FULL")
            {
                //alert player here
                return;
            }
        }
    }
    
    private string GenerateName()
    {
        return ("Recipe #" + (crafter.inventory.GetNumberOfElements()+1).ToString());
    }
    private void Update()
    {
        quantityText.text = quantity.ToString();
        rPanel.SetPanel(characteristics);
        
    }

    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
}
