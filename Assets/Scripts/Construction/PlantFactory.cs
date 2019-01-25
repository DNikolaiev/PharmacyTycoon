using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFactory : Manufactory{
  
    protected override void Expand(int amount)
    {
        resourceStorage.ExpandPlantsStorage(amount);
    }
    protected override void GainResources(int amount)
    {
        resourceStorage.AddHealingPlants(amount);
    }

    protected override void LoseResources(int amount)
    {
        resourceStorage.AddHealingPlants(-amount);
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {
        
        base.ConfirmUpgrade(uPanel);
        uPanel.upgradeBtn.interactable = false;
        uPanel.currentStorage.text = resourceStorage.MaxHealingPlants.ToString();
       
        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }
        
    }
   
}
