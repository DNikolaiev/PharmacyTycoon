
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryPanel : Panel, IVisualize {
    [SerializeField] Transform recipeListView;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] BarFiller bar;
    [SerializeField] Text cost;
    [SerializeField] Text efficiency;
    [SerializeField] RecipeHolder holder;
    [SerializeField] RecipeSelector selector;
    [SerializeField] Animation anim;
    [SerializeField] AudioClip clip;
    [SerializeField] ParticleSystem onReduceToxicity;
    [SerializeField] Tutorial tutorial;
    public override void Hide()
    {
        if (!gameObject.activeInHierarchy) return;
        RecipeSelector.UnSelectRecipe();
        GameController.instance.IsGameSceneEnabled = true;
        GameController.instance.time.UnPause();
        EventManager.StopListening("OnRecipeRemove", ResetBar);
        anim.Play("Laboratory_Disappear");
     
    }
  
    public override void SetPanel()
    {
        
        gameObject.SetActive(true);
        anim.Play("Laboratory_Appear");
        GameController.instance.time.Pause();
        GameController.instance.IsGameSceneEnabled = false;
        efficiency.text = "<color='yellow'>Efficiency" + " - "+GameController.instance.player.skills.toxicityReducer+"</color>";
        GameController.instance.buttons.HideAllButtons();
        StartCoroutine(GameController.instance.cam.ResetCamera());
        GameController.instance.crafter.PopulateRecipeList(recipeListView, recipePrefab);
        holder.picture.sprite = holder.defaultSprite;
        ResetBar();
        EventManager.StartListening("OnRecipeRemove", ResetBar);
        selector.SetVisualizer(this);
        if (!tutorial.isTutorialCompleted) tutorial.StartTutorial();

    }
    private void ResetBar()
    {
        bar.SetValueToBarPercent(0, bar.toxicityBar);
        bar.SetValueToBarScalar(0, bar.healingBar, 100);
        Image greenBar = bar.toxicityBar.transform.parent.Find("GreenImage").GetComponent<Image>();
        bar.SetValueToBarPercent(0, greenBar);
    }
    public void DrawBar() // display toxicity and healing in bars
    {
        cost.text = CalculateCost().ToString();

        var crafter = GameController.instance.crafter;
        if (bar != null && RecipeSelector.recipeHolderSelected!= null)
        {
            Image greenBar = bar.toxicityBar.transform.parent.Find("GreenImage").GetComponent<Image>();
            bar.SetValueToBarPercent(RecipeSelector.recipeHolderSelected.recipe.characteristics.toxicity - GameController.instance.player.skills.toxicityReducer, greenBar); // toxicity after reducing
            if(!bar.coroutineRunning)
                bar.SetValueToBarPercent(RecipeSelector.recipeHolderSelected.recipe.characteristics.toxicity, bar.toxicityBar); // true toxicity
            bar.SetValueToBarScalar(RecipeSelector.recipeHolderSelected.recipe.characteristics.healingRate, bar.healingBar, 100); // healing bar
        }
        else  return;
    }
    public void ReduceToxicity()
    {
        bool result = true;
        if (RecipeSelector.recipeHolderSelected == null) return;
        if (RecipeSelector.recipeHolderSelected.recipe == null) return;
        GameController.instance.player.skills.ReduceToxicity(RecipeSelector.recipeHolderSelected.recipe, CalculateCost(), out result); // logic
        if (result)
        {
            if (!tutorial.isTutorialCompleted) tutorial.ContinueTutorial();
            StartCoroutine(bar.ReduceOverTime(bar.toxicityBar, (float)RecipeSelector.recipeHolderSelected.recipe.characteristics.toxicity,100));
            onReduceToxicity.Play();
            DrawBar();
            GameController.instance.audio.MakeSound(clip);
        }
    }
    private int CalculateCost()
    {
        if ( RecipeSelector.recipeHolderSelected == null || RecipeSelector.recipeHolderSelected.recipe == null )
            return 0;
        Debug.Log(RecipeSelector.recipeHolderSelected == null?true:false);
        
            return RecipeSelector.recipeHolderSelected.recipe.Talents.Count * 250;
       
    }
    
    private void Start()
    {
        EventManager.StartListening("ContinueTutorial", tutorial.ContinueTutorial);
        selector = GetComponent<RecipeSelector>();
       // selector.SetVisualizer(this);
        if(gameObject.activeInHierarchy)
             gameObject.SetActive(false);
    }
   
}
