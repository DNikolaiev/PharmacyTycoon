using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingsPanel : Panel {
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    private const string link = "https://coldspike37.wixsite.com/pharmacy-tycoon/privacy-policy";
    public override void Hide()
    {
        gameObject.SetActive(false);
        if(GameController.instance!=null)
        GameController.instance.IsGameSceneEnabled = true;
    }
    
    public override void SetPanel()
    {
        gameObject.SetActive(true);
        if(GameController.instance!=null)
        GameController.instance.IsGameSceneEnabled = false;
    }
    public void ShowLeaderboard()
    {
        PlayGameScript.ShowLeaderBoardsUI();
    }
    public void ShowAchievments()
    {
        PlayGameScript.ShowAchievementsUI();
    }
    public void OpenLink()
    {
        Application.OpenURL(link);
    }
    public void BackToMenu()
    {
        Progress.Save();
        SceneManager.LoadScene("Start");
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
