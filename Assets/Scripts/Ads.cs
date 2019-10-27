using UnityEngine.Advertisements;
using UnityEngine;
using System.Collections;
public class Ads : MonoBehaviour
{
    public bool testMode;
    public int secondsBeforeAd;
    private readonly string gameId = "3272882";
    void Start()
    {
       // EventManager.StartListening(TurnController.OnTurnEvent, ShowAds);
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(StartCountDown());

    }
    public void ShowAds()
    {
        if ((int)GameController.instance.time.date.month % 2 != 0 && !GameController.instance.player.premiumState)
        {
            if (Advertisement.IsReady())
            {
                StartCoroutine(ShowAdWhenReady());
            }
        }
        
    }
    IEnumerator StartCountDown()
    {
       
            Debug.Log("coroutine has started");
            yield return new WaitForSeconds(secondsBeforeAd);
            ShowAds();
        yield break;
           
        
    }
    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady())
        {
            yield return null;
        }
        if(!GameController.instance.time.isPaused)
             GameController.instance.time.Pause();
        ShowOptions options = new ShowOptions
        {
            resultCallback = HandleShowResult
        };
        Advertisement.Show("video");
        StartCoroutine(StartCountDown());
        yield break;
    }
    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
           


        }
        else if (result == ShowResult.Skipped)
        {
            
          
        }
        else if (result == ShowResult.Failed)
        {
            
     
        }
        
    }
}
