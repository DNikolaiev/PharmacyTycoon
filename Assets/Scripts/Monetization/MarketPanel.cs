using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MarketPanel : Panel
{
    [SerializeField] Tutorial tutorial;
    [SerializeField] List<MarketHolder> holders;
    [SerializeField] Text coinsText;
    [SerializeField] Text secondChances;
    public Button controlButton;
    public List<int> GetTimers()
    {
        List<int> timers = new List<int>();
        foreach(MarketHolder h in holders)
        {
            timers.Add(h.timer);
        }
        return timers;
    }
    public void SetTimers(List<int> timers)
    {
       for (int i=0; i<timers.Count;i++)
        {
            holders[i].timer = timers[i];
        }
    }
    public void CheckNewOffers()
    {
        foreach (MarketHolder h in holders)
        {
            if (h.timer <= 0 && h.mObject.canBeWatched && GameController.instance.generalTutorial.isTutorialCompleted)
            {
                controlButton.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
        Invoke("CheckNewOffers", 60);
    }
    public void OnLoad()
    {
        foreach(MarketHolder h in holders)
        {
            h.OnLoad();
        }
        CheckNewOffers();
    }
    
    private void OnDisable()
    {
        controlButton.GetComponentInChildren<ParticleSystem>().Stop();
    }
    private void OnEnable()
    {
        controlButton.GetComponentInChildren<ParticleSystem>().Stop();
    }
    public void StartAllTimers()
    {
        foreach(MarketHolder h in holders)
        {
            GameController.instance.StartCoroutine(h.ReduceTimer());
        }
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
        GameController.instance.time.UnPause();
        GameController.instance.IsGameSceneEnabled = true;
        if (GameController.instance.gameOver.CheckCondition())
        {
            GameController.instance.EnableGameOver(true);
        }
        else
        {
            GameController.instance.EnableGameOver(false);
         }
        
    }

    public override void SetPanel()
    {
        
        GameController.instance.IsGameSceneEnabled = false;
        gameObject.SetActive(true);
        GameController.instance.time.Pause();
        foreach(MarketHolder holder in holders)
        {
            holder.SetPanel();
        }
        coinsText.text = GameController.instance.player.resources.medCoins.ToString();
        secondChances.text = GameController.instance.player.resources.SecondChances.ToString();
        if (!tutorial.isTutorialCompleted)
        {
            tutorial.StartTutorial();
        }
    }
    private void Start()
    {
        foreach (MarketHolder h in holders) {
            EventManager.StartListening("OnDollarsPurchase", h.BuyForRealMoney);
            
                }
        
    }
}
