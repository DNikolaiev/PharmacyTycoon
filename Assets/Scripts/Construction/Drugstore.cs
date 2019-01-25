using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drugstore : Manufactory {

    public int timeReducer;
    protected override void GainResources(int amount)
    {
        resourceStorage.ChangeBalance(amount);
    }

    protected override void LoseResources(int amount)
    {
        resourceStorage.ChangeBalance(-amount);
    }
 
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {

        base.ConfirmUpgrade(uPanel);
        uPanel.upgradeBtn.interactable = false;
        uPanel.currentStorage.text = (productionTime).ToString() + " s";
        uPanel.storage.text = "Wait";
        uPanel.upgradedStorage.text = (productionTime - timeReducer).ToString() + " s";
        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }

    }

    protected override void Expand(int amount)
    {
        productionTime -= timeReducer;
    }
}
