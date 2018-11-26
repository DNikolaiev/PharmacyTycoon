using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryFactory : Manufactory {


    protected override void Expand(int amount)
    {
        resourceStorage.ExpandChemistryStorage(amount);
    }
    protected override void GainResources(int amount)
    {
        resourceStorage.AddChemistry(amount);
    }

    protected override void LoseResources(int amount)
    {
        resourceStorage.AddChemistry(-amount);
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {

        base.ConfirmUpgrade(uPanel);
        uPanel.upgradeBtn.interactable = false;
        uPanel.currentStorage.text = resourceStorage.MaxChemistry.ToString();

        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }

    }
}
