using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchCenter : Manufactory {
    //storage = research speed
    public ParticleSystem onGenerate;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Wait(productionTime/(GameController.instance.researcher.ResearchSpeed/100)));
    }
    private void Update()
    {
        if (!isBusy)
        {
            resourceStorage.AddResearchPoints(resourcePerTime);
            StartCoroutine(Wait(productionTime / (GameController.instance.researcher.ResearchSpeed / 100)));
        }
    }
    protected override IEnumerator Wait(float time)
    {
        timeInWork = time;
        isBusy = true;
        
        while (timeInWork > 0)
        {
            timeInWork--;
            yield return new WaitForSeconds(1);
        }
        isBusy = false;
        if (onGenerate != null && GameController.instance.IsGameSceneEnabled)
        {
            onGenerate.gameObject.SetActive(true);
            onGenerate.Play();
          
        }
        yield return null;
    }
    protected override void Expand(int amount)
    {
        GameController.instance.researcher.EnhanceResearchTime(amount);
    }
    protected override bool GainResources(int amount)
    {
        if (resourceStorage.AddResearchPoints(amount) != 0)
            return true;
        return false;
    }
    public override void TouchObject(HelpPanel panel)
    {
        base.TouchObject(panel);
        this.panel.timetxt.text = (GameController.instance.researcher.ResearchSpeed).ToString() + " %";
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {

        base.ConfirmUpgrade(uPanel);
        uPanel.upgradeBtn.interactable = false;
        uPanel.currentStorage.text = (GameController.instance.researcher.ResearchSpeed).ToString() + " %";
        uPanel.storage.text = "Speed";
        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }
        

    }
  
    protected override void LoseResources(int amount)
    {
        resourceStorage.AddResearchPoints(-amount);
    }
  
}
