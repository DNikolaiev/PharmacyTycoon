using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResearchView : MonoBehaviour {

    public Image progressImage;
    public float fillAmount;
    public ResearchHolder[] holders;
    public Text lvlTxt;
    public Text speedTxt;
    public Animation anim;
    public ParticleSystem onResearch;
    public AudioClip onResearchSound;
    public Button closeButton;
    public ParticleSystem tutorialParticles;
    private void Update()
    {
        lvlTxt.text = GameController.instance.player.level.ToString();
    }
    private void OnEnable()
    {
       if (GameController.instance!=null)
        speedTxt.text = GameController.instance.researcher.ResearchSpeed.ToString()+"%";
    }
    public void ShowCloseButton()
    {
        closeButton.gameObject.SetActive(true);
    }
}
