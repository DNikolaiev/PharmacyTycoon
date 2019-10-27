using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(BarFiller))]
public class PlayerPanel : Panel
{
    public Text lvlText;
    public Text expText;
    public Text expNeededText;
    public Text expExpectedText;
    public Transform expPanel;
    public GameObject textPrefab;
    public BarFiller expBar;
    public Color expColor;
    int expExpected;
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        Invoke("SetPanel", .1f);
    }
    public void PrognoseExperience(int value)
    {
        expExpected = value;
        expExpectedText.text = "+ " + value.ToString();
    }
    public  void SetPanel(int experienceAmount)
    {
        if (GameController.instance == null) return;
        Player player = GameController.instance.player;
        
        lvlText.text = player.level.ToString();
        if (textPrefab != null && experienceAmount != 0 && expPanel != null)
        {
            GameObject instantiated = Instantiate(textPrefab, expPanel);

            instantiated.GetComponent<Text>().text ="+ "+ experienceAmount.ToString();
            instantiated.GetComponent<Text>().color = expColor;
        }
        if (Nametxt!=null)
        {
            Nametxt.text = player.Name;
        }
        if (expNeededText != null)
        {
            expNeededText.text = player.experienceNeeded.ToString();
            expText.text = player.experience.ToString() + " / " + expNeededText.text;
        }
        else
        {
            if (expText != null)
                expText.text = player.experience.ToString();
        }

        if (expBar != null && player.experienceNeeded != 0)
        {
            expBar.SetValueToBarScalar(player.experience, expBar.healingBar, player.experienceNeeded);
        }
        if(expBar.toxicityBar!=null && expExpected!=0 && expBar!=null)
        {
            expBar.SetValueToBarScalar(player.experience+expExpected, expBar.toxicityBar, player.experienceNeeded);
        }
    }
    private void OnEnable()
    {
        Invoke("SetPanel", 0.4f);
    }

    public override void SetPanel()
    {
        SetPanel(0);
    }
}
