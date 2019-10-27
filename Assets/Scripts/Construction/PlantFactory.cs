using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFactory : Manufactory{
  
    protected override void Expand(int amount)
    {
        resourceStorage.ExpandPlantsStorage(amount);
    }
    protected override bool GainResources(int amount)
    {
        if (resourceStorage.AddHealingPlants(amount) != 0)
            return true;
        return false;

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
