using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextPanel : Panel
{
    public Text text;
    [SerializeField] GameObject background;
   
    public override void Hide()
    {
        GameController.instance.time.UnPause();
        GameController.instance.IsGameSceneEnabled = true;
        background.gameObject.SetActive(false);
  
    }

    public override void SetPanel()
    {
        if (GameController.instance.IsGameSceneEnabled)
        {
            background.gameObject.SetActive(true);
            GameController.instance.time.Pause();
            
        }

    }
    public void SetPanel(Area area, string header=null)
    {
       gameObject.SetActive(true);
        Nametxt.text = (header == null) ? string.Empty : header;
        text.text = string.Empty;
        foreach(string s in area.passiveSkills)
        {
           
            text.text= text.text + "\n" +s + "\n";
        }
    }
}
