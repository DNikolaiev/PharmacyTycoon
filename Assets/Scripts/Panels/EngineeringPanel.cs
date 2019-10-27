using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EngineeringPanel : Panel {
    public SaveResearchData saveData;
    [SerializeField] TextMeshProUGUI liquidMoney;
    [SerializeField] TextMeshProUGUI pillsMoney;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] BarFiller liquidBar;
    [SerializeField] BarFiller pillsBar;
    [SerializeField] ParticleSystem onEnhance;
    [SerializeField] private int counter;
    [SerializeField] ColorInterpolator interpol;
    [SerializeField] Animation anim;
    [SerializeField] Tutorial tutorial;
    [SerializeField] AudioClip clip;
    public int startPrice;
    public int healingPointsPerClick;
    private void Awake()
    {
        interpol = GetComponent<ColorInterpolator>();
    }
    private void OnEnable()
    {
        EventManager.StartListening(Player.OnLevelEvent, ResetCounter);
        SaveController.OnLoadEvent += OnLoad;
        SaveController.OnSaveEvent += OnSave;
    }
    private void OnDisable()
    {
        EventManager.StopListening(Player.OnLevelEvent, ResetCounter);
        SaveController.OnLoadEvent -= OnLoad;
    }
    public override void Hide()
    {
        GameController.instance.IsGameSceneEnabled = true;
       // gameObject.SetActive(false);
        GameController.instance.buttons.ShowAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(false);
        anim.Play("Engineering_Disappear");
    }
    
    public override void SetPanel()
    {
       
       
        gameObject.SetActive(true);
        anim.Play("Engineering_Appear");
        StartCoroutine(GameController.instance.cam.ResetCamera());
        
        GameController.instance.IsGameSceneEnabled = false;
        gameObject.SetActive(true);
        GameController.instance.buttons.HideAllButtons();
        //     GameController.instance.buttons.cancel.gameObject.SetActive(true);
        Refresh();
        if (!tutorial.isTutorialCompleted) tutorial.StartTutorial();
    }
    private void Refresh()
    {
       
        
        infoText.text = "Enhancements " + counter + " / " + GameController.instance.player.skills.enhancementCounter;
        pillsMoney.text = CalculatePillsEnhancementPrice().ToString();
        liquidMoney.text = CalculateLiquidEnhancementPrice().ToString();

        SetValueToBar();
    }
    public void ResetCounter()
    {
        counter = 0;
        Refresh();
    }
    private void SetValueToBar()
    {
        var crafter = GameController.instance.crafter;
        if (liquidBar != null && pillsBar != null)
        {

            liquidBar.SetValueToBarScalar(GameController.instance.player.skills.liquidHealing, liquidBar.healingBar, 100);
            pillsBar.SetValueToBarScalar(GameController.instance.player.skills.pillsHealing, pillsBar.healingBar, 100);
            
        }
      
        else return;

    }
    private int CalculateLiquidEnhancementPrice()
    {
        return (startPrice + GameController.instance.player.skills.liquidHealing * startPrice);
    }
    private int CalculatePillsEnhancementPrice()
    {
        return (startPrice + GameController.instance.player.skills.pillsHealing * startPrice);
    }
    private void Start()
    {
        SaveController.OnSaveEvent += OnSave;
        Invoke("SetValueToBar", .1f);
        if(gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
   
    public void EnhanceLiquid()
    {
        if (counter >= GameController.instance.player.skills.enhancementCounter)
        {
            infoText.text = "Out of order. Return after next level";
            return;
        }
        bool result = false;
        GameController.instance.player.skills.EnhanceHealing(true, healingPointsPerClick, CalculateLiquidEnhancementPrice(), out result);
        if (result)
            counter++;
        onEnhance.Play();
        GameController.instance.audio.MakeSound(clip);
        Refresh();
    }
    public void EnhancePills()
    {
        if (counter >= GameController.instance.player.skills.enhancementCounter)
        {
            infoText.text = "Out of order. Return after next level";
            return;
        }

        bool result = false;
        GameController.instance.player.skills.EnhanceHealing(false, healingPointsPerClick, CalculatePillsEnhancementPrice(), out result);
        if (result)
            counter++;
        onEnhance.Play();
        GameController.instance.audio.MakeSound(clip);
        Refresh();
    }
    public void OnLoad()
    {
       
        counter = saveData.researchSpeed;
        liquidBar.healingBar.fillAmount+= saveData.filled1;
        pillsBar.healingBar.fillAmount+= saveData.filled2;
        
       
    }
    public void OnSave()
    {
        saveData = new SaveResearchData(counter, 0, filled1:liquidBar.healingBar.fillAmount,filled2:pillsBar.healingBar.fillAmount);

    }
}
