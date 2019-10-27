using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Laboratory : SceneObject, IUpgradable
{
    public int startEfficiency;
    public int upgradedEfficiency;
    private int currentEfficiency;
    private int upgradeCost;
    protected ResourceStorage resourceStorage;
    [SerializeField] ColorInterpolator interpol;
    [SerializeField] Tutorial tutorial;
    

    public void IncreaseStats(int lvl, bool expand=true)
    {
        currentEfficiency += upgradedEfficiency;
        if(expand)
        GameController.instance.player.skills.toxicityReducer += upgradedEfficiency; 
        SetSellPrice(2);
    }

    public void Upgrade(int level, bool chargePlayer = true)
    {
        if (chargePlayer)
        {
            if (resourceStorage.money < upgradeCost)
            {
                lvl--;
                StartCoroutine(interpol.InOut(uPanel.cost, Color.red));
                return;
            }
            GameController.instance.player.GainExperience(GameController.instance.roomOverseer.GetAllSceneObjects().Where(x => x.GetType() == this.GetType()).ToList().Count * 50);
            resourceStorage.ChangeBalance(-upgradeCost);
            if (onUpgrade != null)
            {
                onUpgrade.gameObject.SetActive(true);
                onUpgrade.Play();
            }
            StartCoroutine(GameController.instance.cam.FocusCamera(this.transform.position));
            EventManager.TriggerEvent("OnUpgrade");
            GameController.instance.audio.MakeSound(onUpgradeSound[Random.Range(0, onUpgradeSound.Count)]);
        }
        MergeAdjacentRooms(this);
        IncreaseStats(level, chargePlayer);
        upgradeCost = (int)((description.buyPrice / 2) + (level) * (description.buyPrice / 5));
        GetComponent<SpriteRenderer>().sprite = description.upgradedSprite;
        if(uPanel!=null)
        uPanel.Hide();
        if (lvl == 2 && hasJointedObject)
        {
            ChangeViewToDouble();

            currentEfficiency += upgradedEfficiency * 2;
            IncreaseStats(++level,chargePlayer);
            lvl++;
        }
    }
    protected override void SellObject()
    {
        GameController.instance.player.skills.toxicityReducer -= currentEfficiency;
        base.SellObject();
    }
   
    public override void ConfirmUpgrade(UpgradePanel uPanel) //view
    {
        uPanel.upgradeBtn.onClick.RemoveAllListeners();
        base.ConfirmUpgrade(uPanel);
        uPanel.storage.text = "Efficiency";
        uPanel.production.text = string.Empty;
        uPanel.currentProduction.text = string.Empty ;
        uPanel.upgradedProduction.text = string.Empty;
        uPanel.upgradedStorage.text = " +" + upgradedEfficiency.ToString() ;
        uPanel.currentStorage.text = GameController.instance.player.skills.toxicityReducer.ToString();
        upgradeCost = (int)((description.buyPrice / 2) + (lvl) * (description.buyPrice / 5));
        uPanel.cost.text = upgradeCost.ToString() + " $";

        uPanel.upgradeBtn.interactable = false;
        if (lvl < 2)
        {
            uPanel.upgradeBtn.interactable = true;
            uPanel.upgradeBtn.onClick.AddListener(delegate { Upgrade(++lvl); });
        }
    }
    
    public override void TouchObject(HelpPanel panel)
    {
        base.TouchObject(panel);
        this.panel.resourceImage.gameObject.SetActive(true);
        this.panel.resourcetxt.gameObject.SetActive(true);
        this.panel.timetxt.gameObject.SetActive(false);
        this.panel.resourcetxt.text = GameController.instance.player.skills.toxicityReducer.ToString();
        this.panel.timetxt.text = string.Empty;
        this.panel.Nametxt.text = description.Name + " - " + lvl;

    }
    private void Awake()
    {
        
    }
    protected override void Start()
    {
        base.Start();

        interpol = GetComponent<ColorInterpolator>();
        resourceStorage = GameController.instance.player.resources;
        if (!isCreated)
        {
            currentEfficiency += startEfficiency;
            GameController.instance.player.skills.toxicityReducer += startEfficiency;
            isCreated = true;
            if (!GameController.instance.tutorialSettings.GetTutorialState("Laboratory"))
            {
                tutorial.tPanel = GameController.instance.buttons.tPanel;
                tutorial.highlightedObjects.Add(GameController.instance.buttons.laboratory.gameObject);
                tutorial.StartTutorial();
            }
        }

    }
}
