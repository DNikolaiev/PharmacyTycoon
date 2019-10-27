using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AsyncLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public BarFiller filler;
    
    public void LoadLevel(string level)
    {
        StartCoroutine(LoadAsynchronously(level));
    }
    IEnumerator LoadAsynchronously(string level)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
           
            filler.SetValueToBarPercent(progress*100, filler.healingBar);
            yield return null;
        }
    }
}
