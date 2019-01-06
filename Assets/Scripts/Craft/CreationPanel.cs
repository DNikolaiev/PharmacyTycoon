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
    private void Awake()
    {
        interpol = GetComponent<ColorInterpolator>();
    }
    public override void Hide()
    {
        blackBack.enabled = false;
      
         gameObject.SetActive(false);
        ButtonController.instance.cancel.gameObject.SetActive(true);
    }

    public  void SetPanel(Characteristics characteristics)
    {
        blackBack.enabled = true;
        gameObject.SetActive(true);
        slider.value = 0;
        quantity = 1;
        popAnim.Play("Pop_out_CreationPanel");
        this.characteristics = characteristics;
        rPanel.SetPanel(characteristics);
        rPanel.researchPoints.text = Crafter.instance.CalculateResearchPoints(Crafter.instance.selectedTalents).ToString();
        Recipe testRecipe = Crafter.instance.RecognizeRecipe();

        if (testRecipe != null)
        {
            Crafter.instance.recipeSelected = testRecipe;
            Debug.Log(Crafter.instance.recipeSelected.description.Name);
            Crafter.instance.isPrescripted = true;
        }
        if (Crafter.instance.isPrescripted)
        {
            ok.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            rPanel.researchPoints.gameObject.SetActive(false);
            rPanel.rpImg.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
            header.text = Crafter.instance.recipeSelected.description.Name;
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

        if (Crafter.instance.isPrescripted)
        {
            string result = Crafter.instance.Craft(quantity);
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
            redBorder.enabled = false;
            string result = Crafter.instance.Craft(recipeName, quantity);
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
        return ("Recipe #" + (Crafter.instance.inventory.GetNumberOfElements()+1).ToString());
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
