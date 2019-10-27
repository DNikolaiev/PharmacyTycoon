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
    public Text cost;
    private Constructor constructor;
    private void Start()
    {
        constructor = GameController.instance.constructor;
        Hide();
    }
    public override void Hide()
    {
        GameController.instance.IsGameSceneEnabled = true;
        gameObject.SetActive(false);
        
    }
    private void Update()
    {
        if (constructor.isShopEnabled)
            Hide();
    }
    public override void SetPanel()
    {
        if (constructor.isActive)
            constructor.ConstructOFF();
        if (descriptionPanel.lastTouched.lvl >= 2)
        {
            upgradeBtn.interactable = false;
            return;
        }
        descriptionPanel.Hide();
        gameObject.SetActive(true);
        GameController.instance.IsGameSceneEnabled = false;
        descriptionPanel.lastTouched.ConfirmUpgrade(this);
    }
}
