using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticFactory : Manufactory {

    protected override void Expand(int amount)
    {
        resourceStorage.ExpandPlasticStorage(amount);
    }
    protected override bool GainResources(int amount)
    {
        if (resourceStorage.AddPlastic(amount) != 0)
            return true;
        return false;
    }

    protected override void LoseResources(int amount)
    {
        resourceStorage.AddPlastic(-amount);
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {

        base.ConfirmUpgrade(uPanel);
        uPanel.upgradeBtn.interactable = false;
        uPanel.currentStorage.text = resourceStorage.MaxPlastic.ToString();

        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }

    }
}
