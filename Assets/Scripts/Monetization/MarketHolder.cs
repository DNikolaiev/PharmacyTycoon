using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;
public class MarketHolder : Panel
{

    public MonetizedObject mObject;

    [SerializeField] Text priceDollars;
    [SerializeField] Text priceInCoins;
    [SerializeField] Image image;
    [SerializeField] Button watchAdd;
    [SerializeField] Button buyDollars;
    [SerializeField] Button buyCoins;
    [SerializeField] HintPanel hintPanel;
    [SerializeField] PurchasePanel purchasePanel;
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject timerPanel;
    [SerializeField] AudioClip purchaseSound;
    public int timer;

    public override void Hide()
    {

    }
    public IEnumerator ReduceTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameController.instance.time.DayDuration);
            timer--;
            timer = Mathf.Clamp(timer, -1, 999);
            if(timer==0 && mObject.canBeWatched )
            {
                GameController.instance.buttons.market.controlButton.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
    
    private void OnEnable()
    {
        GameController.instance.StopAllCoroutines();
        timerPanel.SetActive(false);
    }
    public void OnLoad()
    {
        if(mObject.canBeWatched)
        GameController.instance._StartCoroutine(ReduceTimer());
      
    }
    private void OnDisable()
    {
        
        timerPanel.SetActive(false);
    }
    private void LeaveBlank()
    {
        image.sprite = null;
        priceDollars.text = string.Empty;
        priceInCoins.text = string.Empty;
    }
    public override void SetPanel()
    {
        if (mObject == null) { LeaveBlank(); return; }
        timerText.text = "Available in " + timer +" days";
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        //buyDollars.onClick.RemoveAllListeners();
        buyCoins.onClick.RemoveAllListeners();
        watchAdd.onClick.RemoveAllListeners();
        Nametxt.text = mObject.description.Name;
        priceInCoins.text = mObject.priceInCoins.ToString();
        priceDollars.text = mObject.priceInDollars.ToString() + " $";
        image.sprite = mObject.description.sprite;
        watchAdd.gameObject.SetActive(false);
        buyCoins.gameObject.SetActive(false);
        if (mObject.canBeWatched)
        {
            watchAdd.gameObject.SetActive(true);
            if (timer <= 0 && GetComponentInChildren<ParticleSystem>() != null) watchAdd.GetComponentInChildren<ParticleSystem>().Play();
            //watch add here, if successfull then
            watchAdd.onClick.AddListener(delegate { ConfirmPurchase("ad"); });
        }
        else if (mObject.priceInCoins > 0)
        {
            buyCoins.gameObject.SetActive(true);
            buyCoins.onClick.AddListener(delegate { ConfirmPurchase("coins"); });
        }
       // buyDollars.onClick.AddListener(delegate { ConfirmPurchase("dollars"); });
       
        GetComponent<Button>().onClick.AddListener(delegate { hintPanel.SetPanel(mObject.description.description, mObject.description.Name, mObject.description.sprite); });

    }
    public void BuyForRealMoney(object id)
    {
        if((string)id==mObject.id)
        {
            ConfirmPurchase("dollars");
        }
    }
    private void ConfirmPurchase(string type)
    {
        switch(type)
        {
            case "coins":
                {
                    if(GameController.instance.player.resources.medCoins >= mObject.priceInCoins)
                    {
                        GameController.instance.player.resources.AddCoins(-mObject.priceInCoins);
                        mObject.OnSell();
                        GameController.instance.audio.MakeSound(purchaseSound);
                        if (mObject.GetComponent<Chest>())
                            purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite, mObject.GetComponent<Chest>().reward);
                        else purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite);
                    }
                    break;
                }
            case "dollars":
                {
                   
                    mObject.OnSell();
                    GameController.instance.audio.MakeSound(purchaseSound);
                    GameController.instance.player.premiumState = true;
                    if (mObject.GetComponent<Chest>())
                        purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite, mObject.GetComponent<Chest>().reward);
                    else purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite);
                    break;
                }
            case "ad":
                {
                    if (timer > 0)
                    {
                        anim.Play("Monetized_Cooldown");
                        watchAdd.GetComponentInChildren<ParticleSystem>().Stop();
                        return;
                    }
                    else
                    {
                        
                        ShowOptions options = new ShowOptions
                        {
                            resultCallback = HandleShowResult
                        };
                        Advertisement.Show("rewardedVideo", options);
                    }
                    // if ads can be shown
                   
                    
                    //
                    break;
                }
            default: break;
        }

    }
    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            // Reward your player here.
            timer = mObject.coolDownInDays;
            mObject.OnSell();
            GameController.instance.audio.MakeSound(purchaseSound);
            if (mObject.GetComponent<Chest>())
                purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite, mObject.GetComponent<Chest>().reward);
            else purchasePanel.SetPanel(mObject.afterPurchaseText, mObject.description.Name, mObject.description.sprite);
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show("Advertisement was skipped. No reward, mate", Resources.Load<Sprite>("Icons/tv_icon"));
            return;

        }
        else if (result == ShowResult.Failed)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show("Something went wrong. Try again please.", Resources.Load<Sprite>("Icons/tv_icon"));
            return;
        }
    }
    
}
    
