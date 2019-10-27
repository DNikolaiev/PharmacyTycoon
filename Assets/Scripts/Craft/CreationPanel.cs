using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int maxQuantity = 100;
    [SerializeField] private Image recipeAvatar;
    [SerializeField] private IconSetup icons;
    private ColorInterpolator interpol;
    [SerializeField] private Characteristics characteristics;
    [SerializeField] private AudioClip onCraft;
    [SerializeField] private Animator buttonAnim;
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
        if(GameController.instance.crafter.tutorial.isTutorialCompleted)
            GameController.instance.buttons.cancel.gameObject.SetActive(true);
    }
    public void StopPoppingAnim()
    {
        popAnim.Stop("IconAvatar_popUp");
        recipeAvatar.rectTransform.localScale = new Vector3(1, 1, 1);
        recipeAvatar.GetComponentInChildren<ParticleSystem>().Stop();
    }
    public  void SetPanel(Characteristics characteristics)
    {
       
        icons.gameObject.SetActive(false);
        crafter.view.DisableAllParticles();
        blackBack.enabled = true;
        gameObject.SetActive(true);
        slider.value = 0;
        quantity = 1;
        popAnim.Play("Pop_out_CreationPanel");
        
        input.text = string.Empty;
        this.characteristics = characteristics;
        rPanel.SetPanel(characteristics);
        rPanel.researchPoints.text = crafter.CalculateResearchPoints(crafter.selectedTalents).ToString();
        Recipe testRecipe = crafter.RecognizeRecipe();
        
        
        if (testRecipe != null)
        {
            crafter.recipeSelected = testRecipe;
            crafter.isPrescripted = true;
            characteristics = crafter.Recombine(characteristics);
            characteristics = crafter.GetCharacteristics();
        }
        if (crafter.isPrescripted)
        {
            ok.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            rPanel.researchPoints.gameObject.SetActive(false);
            rPanel.rpImg.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
            header.text = crafter.recipeSelected.description.Name;
            recipeAvatar.GetComponent<Button>().enabled = false;
            recipeAvatar.sprite = crafter.recipeSelected.description.sprite;
            StopPoppingAnim();
        }
        else
        {
            
            ok.GetComponent<RectTransform>().offsetMax = new Vector2(-120, 0);
            rPanel.researchPoints.gameObject.SetActive(true);
            rPanel.rpImg.gameObject.SetActive(true);
            input.gameObject.SetActive(true);
            header.text = "Create your new recipe!";
            if (crafter.isLiquid)
                icons.SetupRecipeIcons(true);
            else icons.SetupRecipeIcons(false);
            recipeAvatar.GetComponent<Button>().enabled = true;
            recipeAvatar.sprite=icons.GetRandomIcon(crafter.isLiquid);
            AddEventOnIcon();
            popAnim.Play("IconAvatar_popUp");
            recipeAvatar.GetComponentInChildren<ParticleSystem>().Play();
        }
     
    }
  
    private void AddEventOnIcon()
    {
       
        
        foreach(Image icon in this.icons.GetAllIcons())
        {
            icon.GetComponent<Button>().onClick.AddListener(delegate { SetIconToAvatar(icon); });
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
        interpol.originalColor = rPanel.herbs.color;
	}
    /*
    private int CalculateMaxCraftQuantity(ResourceStorage availiable, Characteristics needed)
    {
        characteristics /= quantity;
        quantity = 1;
        List<int> resourcesNeeded = new List<int>
        {
            needed.healingPlantsNeeded,
            needed.chemistryNeeded,
            needed.plasticNeeded
        };
        List<int> resourcesAvailiable = new List<int>
        {
            availiable.currentHealingPlants,
            availiable.currentChemistry,
            availiable.currentPlastic
        };
        int indexOfMinimalResources = resourcesAvailiable.IndexOf(resourcesAvailiable.Min());
        int indexOfMinimalCharacteristics = resourcesNeeded.IndexOf(resourcesNeeded.Min());
        int minQuantity1=0, minQuantity2=0, minQuantity3 = 0;
        if (resourcesAvailiable[indexOfMinimalResources] < resourcesNeeded[indexOfMinimalResources])
        {
            return 0;
        }
        else
        {
            minQuantity1 = (resourcesNeeded[indexOfMinimalResources]==0)?resourcesAvailiable[indexOfMinimalResources]: (int)(resourcesAvailiable[indexOfMinimalResources] / resourcesNeeded[indexOfMinimalResources]);
        }
        resourcesAvailiable[indexOfMinimalResources] = 1000000;
        indexOfMinimalResources = resourcesAvailiable.IndexOf(resourcesAvailiable.Min());
        if (resourcesAvailiable[indexOfMinimalResources] < resourcesNeeded[indexOfMinimalResources])
        {
            return 0;
        }
        else
        {
            minQuantity2 = (resourcesNeeded[indexOfMinimalResources] == 0) ? resourcesAvailiable[indexOfMinimalResources] : (int)(resourcesAvailiable[indexOfMinimalResources] / resourcesNeeded[indexOfMinimalResources]);
        }
            resourcesAvailiable[indexOfMinimalResources] = 1000000;
        indexOfMinimalResources = resourcesAvailiable.IndexOf(resourcesAvailiable.Min());
        if (resourcesAvailiable[indexOfMinimalResources] < resourcesNeeded[indexOfMinimalResources])
        {
            return 0;
        }
        else
        {
            minQuantity3 = (resourcesNeeded[indexOfMinimalResources] == 0) ? resourcesAvailiable[indexOfMinimalResources] : (int)(resourcesAvailiable[indexOfMinimalResources] / resourcesNeeded[indexOfMinimalResources]);
        }
       
        return new List<int> { minQuantity1, minQuantity2, minQuantity3 }.Min();

    } */
    private int CalculateMaxCraftQuantity(ResourceStorage availiable, Characteristics needed)
    {
        characteristics /= quantity;
        quantity = 1;
        List<int> resourcesNeeded = new List<int>
        {
            needed.healingPlantsNeeded,
            needed.chemistryNeeded,
            needed.plasticNeeded
        };
        List<int> resourcesAvailiable = new List<int>
        {
            availiable.currentHealingPlants,
            availiable.currentChemistry,
            availiable.currentPlastic
        };
        if(needed.plasticNeeded==0) { resourcesAvailiable.RemoveAt(2); resourcesNeeded.RemoveAt(2); }
       
        int minQuantity1 = 100000, minQuantity2 = 100000, minQuantity3 = 100000;
        CalculateMinimumResources(resourcesAvailiable, resourcesNeeded, out minQuantity1);
        CalculateMinimumResources(resourcesAvailiable, resourcesNeeded, out minQuantity2);
        if(resourcesNeeded.Count>2)
        {
            CalculateMinimumResources(resourcesAvailiable, resourcesNeeded, out minQuantity3);
            
        }

       return new List<int> { minQuantity1, minQuantity2, minQuantity3 }.Min();

    }
    private int CalculateMinimumResources(List<int> resourcesAvailiable, List<int> resourcesNeeded,out int minQuantity)
    {
        int indexOfMinimalResources = resourcesAvailiable.IndexOf(resourcesAvailiable.Min());
        int indexOfMinimalCharacteristics = resourcesNeeded.IndexOf(resourcesNeeded.Min());
         minQuantity = 0;
        if (resourcesAvailiable[indexOfMinimalResources] < resourcesNeeded[indexOfMinimalResources])
        {
            return 0;
        }
        else
        {
            minQuantity = (resourcesNeeded[indexOfMinimalResources] == 0) ? resourcesAvailiable[indexOfMinimalResources] : (int)(resourcesAvailiable[indexOfMinimalResources] / resourcesNeeded[indexOfMinimalResources]);
        }
        resourcesAvailiable[indexOfMinimalResources] = 1000000;
        return minQuantity;
    }
    public void SetQuantityToMax()
    {
        Characteristics tempCh = characteristics;
        int temp = CalculateMaxCraftQuantity(GameController.instance.player.resources, characteristics);
        AddQuantity(temp-1); 
    }
    public void SetIconToAvatar(Image icon)
    {
        recipeAvatar.sprite = icon.sprite;
    }
    public void QuantityChange(float value)
    {
        Debug.Log("QUANTITY "+ quantity);
        Debug.Log("CH HERBS before " + characteristics.healingPlantsNeeded);
        characteristics /=  quantity;
        Debug.Log("CH HERBS after " + characteristics.healingPlantsNeeded);
        quantity = (int)value;
        characteristics *= quantity;
    }

    public void AddQuantity(int amount)
    {
        characteristics /= quantity;
        quantity += amount;
        
        quantity = Mathf.Clamp(quantity, 1, maxQuantity);
        characteristics *= quantity;
        slider.value = quantity;
    }
    private void NotifyAboutResources()
    {
        Text[] texts = { rPanel.herbs, rPanel.chems, rPanel.plastic, rPanel.researchPoints };
        foreach(Text t in texts)
        {
            StartCoroutine(interpol.InOut(t, Color.red));
        }
    }
    public void Accept()
    {
        var messageBox = GameController.instance.buttons.messageBox;
        if (crafter.isPrescripted)
        {
            string result = crafter.Craft(quantity);
            if (result == "TRUE")
            {
                if (!crafter.tutorial.isTutorialCompleted)
                {
                    crafter.tutorial.ContinueTutorial();
                    GameController.instance.buttons.ShowCancel();
                }
                messageBox.Show("x" + quantity + " " + crafter.recipeSelected.description.Name + " crafted", crafter.recipeSelected.description.sprite);
                Hide();
                GameController.instance.audio.MakeSound(onCraft);
            }
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
            if (!validate.ValidateRecipeName(recipeName, GameController.instance.player.inventory) || recipeName=="Invalid name!")
            {
                input.textComponent.text = "";
                input.text = "Invalid name!";
                redBorder.enabled = true;
                return;
            }
            int researchPointsNeeded = crafter.CalculateResearchPoints(crafter.selectedTalents);
          
            redBorder.enabled = false;
            string result = crafter.Craft(recipeName, quantity, recipeAvatar.sprite);
            if (result == "TRUE")
            {
                if (!crafter.tutorial.isTutorialCompleted)
                {
                    crafter.tutorial.ContinueTutorial();
                    GameController.instance.buttons.ShowCancel();
                }
                messageBox.Show(crafter.recipeSelected.description.Name + " was created", crafter.recipeSelected.description.sprite);
                Hide();
                GameController.instance.audio.MakeSound(onCraft);
            }
            
            else if (result == "FALSE")
            {
                NotifyAboutResources();
                return;
            }
            else if (result == "FULL")
            {
                messageBox.Show("Inventory is full. Delete one recipe");
                return;
            }
        }
    }
    
    private string GenerateName()
    {
        return ("Recipe #" + (GameController.instance.player.inventory.recipes.Count+1).ToString());
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
