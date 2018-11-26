using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradePanel : Panel
{
    public HelpPanel descriptionPanel;
    public Text currentProduction;
    public Text currentStorage;
    public Text upgradedProduction;
    public Text upgradedStorage;
    public Text production;
    public Text storage;
    public Image resourceIcon;
    public Button upgradeBtn;
    private void Start()
    {
        Hide();
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Constructor.instance.isShopEnabled)
            Hide();
    }
    public override void SetPanel()
    {
        if (Constructor.instance.isActive)
            Constructor.instance.ConstructOFF();
        if (descriptionPanel.lastTouched.lvl >= 2)
        {
            upgradeBtn.interactable = false;
            return;
        }
        descriptionPanel.Hide();
        gameObject.SetActive(true);
        
        descriptionPanel.lastTouched.ConfirmUpgrade(this);
    }
}
