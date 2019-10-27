using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ColorInterpolator))]
public class SellView : MonoBehaviour, IVisualize {
    #region inspector
    [SerializeField] Text peopleKilled;
    [SerializeField] Text peopleCured;
    #endregion
    public Transform recipeView;
    public GameObject recipePrefab;
    public InvoicePanel invoicePanel;
    public ParticleSystem onSell;
    public List<TalentHolder> epidemicHolders;
    [SerializeField] BarFiller recipeBar;
    public BarFiller areaHealthBar;
    public Color fullHpColor;
    public Button returnBtn;
    public Button cancelBtn;
    [SerializeField] Text Name;
    ColorInterpolator interpol;
     RecipeSelector selector;
    private void Awake()
    {
        interpol = GetComponent<ColorInterpolator>();
    }
    private void OnEnable()
    {
        areaHealthBar.healingBar.color = Color.red;
    }
    private void Update()
    {
        if (!areaHealthBar.coroutineRunning)
        {
            
            StopAllCoroutines();
        }
    }
    public void SetEpidemic(List<Talent> epidemics)
    {
        EpidemicGenerator epidemicGenerator = new EpidemicGenerator();
        
        foreach (TalentHolder t in epidemicHolders)
        {
            t.Talent = null;
            t.picture.sprite = t.defaultSprite;
            if(t.GetComponent<Button>())
            t.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i=0; i<epidemics.Count;i++)
        {
            epidemicHolders[i].Talent = null;
            epidemicHolders[i].Talent = epidemics[i];
            epidemicHolders[i].SetPanel();
            Talent currentTalent = epidemicHolders[i].Talent;
            epidemicHolders[i].gameObject.GetComponent<Button>().onClick.AddListener
                (
                delegate
                 {
                     GameController.instance.buttons.GetHint(epidemicGenerator.GetDisease(currentTalent));
                 }
                );
        }
    }
    public void CureEpidemic(List<Talent> epidemics)
    {
        RemoveCheckmarks();
        foreach(TalentHolder holder in epidemicHolders)
        {
            foreach(Talent e in epidemics)
            {
                if(holder.Talent==e)
                {
                    holder.picture.enabled = false;
                    holder.lockedSprite.SetActive(true);
                }
            }
        }
    }
    public void RemoveCheckmarks()
    {
        foreach(TalentHolder t in epidemicHolders)
        {
            t.lockedSprite.SetActive(false);
            if (t.Talent != null)
            {
                t.picture.enabled = true;
                t.picture.sprite = t.Talent.description.sprite;
                t.SetPanel();
            }
        }
    }
    public void IncreaseHpOverTime(Area area)
    {
        StartCoroutine(areaHealthBar.IncreaseOverTime(areaHealthBar.healingBar, area.health, area.maxHealth, false));
        StartCoroutine(interpol.ColorIn(areaHealthBar.healingBar, areaHealthBar.healingBar.color, fullHpColor));
    }
    public void SetViewToArea(Area area)
    {
        RemoveCheckmarks();
        Name.text = area.Name;
        if(area.backgroundSprite!=null)
        GetComponent<Image>().sprite = area.backgroundSprite;
        SetEpidemic(area.activeEpidemies);
        IncreaseHpOverTime(area);
        peopleCured.text = area.cured.ToString();
        peopleKilled.text = area.dead.ToString();
        if(selector!=null)
            selector.SetVisualizer(this);
        // areaHealthBar.SetValueToBarScalar(area.health, areaHealthBar.healingBar, area.maxHealth);
    }
    public void DrawBar()
    {
        recipeBar.SetValueToBarScalar(RecipeSelector.recipeHolderSelected.recipe.characteristics.healingRate, recipeBar.healingBar,100);
        recipeBar.SetValueToBarPercent(RecipeSelector.recipeHolderSelected.recipe.characteristics.toxicity, recipeBar.toxicityBar);
        var epidemies = FindObjectOfType<Seller>().EpidemiesCured;
    }
    private void Start()
    {
        selector = GetComponent<RecipeSelector>();
        selector.SetVisualizer(this);
       
    }
}
