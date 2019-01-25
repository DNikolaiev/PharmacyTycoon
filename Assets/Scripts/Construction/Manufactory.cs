using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manufactory : SceneObject, IUpgradable
{
    public int resourcePerTime;
    public int upgradedResourcePerTime;
    public int upgradedStorage;
    public float productionTime;
    private int upgradeCost;
    public GameObject finishedProductionImage;
    [SerializeField]  protected float timeInWork;
    [SerializeField] private ColorInterpolator interpol;
    private RaycastHit hit;
    private bool isCreated = false;
    [SerializeField] protected int currentStorage;
    protected ResourceStorage resourceStorage;
    protected abstract void GainResources(int amount);
    protected abstract void LoseResources(int amount);
    protected abstract void Expand(int amount);
    protected override void SellObject()
    {
        Debug.Log(-currentStorage);
        Expand(-currentStorage);
        base.SellObject();
    }
    protected override void Start()
    {
        base.Start();
        interpol = GetComponent<ColorInterpolator>();
        resourceStorage = GameController.instance.player.resources;
        if(!isCreated)
        {
            Expand(currentStorage);
            isCreated = true;
        }
        
    }
    private  void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBusy && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,LayerMask.GetMask("Touchable")))
        {
            if (hit.transform==transform)
            {
                GainResources(resourcePerTime);
                StartCoroutine(Wait(productionTime));
            }
        }
        if (panel != null)
        {
            panel.timetxt.text = timeInWork.ToString();
        }
    }
    protected virtual IEnumerator Wait(float time)
    {
        timeInWork = time;
        isBusy = true;
        if(finishedProductionImage!=null)
            finishedProductionImage.SetActive(false);
        while (timeInWork> 0)
        {
            timeInWork--;
            yield return new WaitForSeconds(1);
        }
        isBusy = false;
        if (finishedProductionImage != null)
            finishedProductionImage.SetActive(true);
        yield return null;
    }
    protected override void MergeAdjacentRooms(SceneObject type)
    {
        base.MergeAdjacentRooms(type);
        if (objectToMerge != null)
        {
            resourcePerTime += objectToMerge.GetComponent<Manufactory>().resourcePerTime;
            objectToMerge.GetComponent<Manufactory>().resourcePerTime = 0;
            
        }
    }
   

    public void IncreaseStats(int level)
    {
        resourcePerTime += upgradedResourcePerTime * (level - 1);
        Expand(upgradedStorage);
        currentStorage += upgradedStorage;
        SetSellPrice(2);
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel)
    {
        uPanel.upgradeBtn.onClick.RemoveAllListeners();
        base.ConfirmUpgrade(uPanel);
        uPanel.storage.text = "Storage";
        uPanel.currentProduction.text =  resourcePerTime.ToString();
        uPanel.upgradedProduction.text = " +" +upgradedResourcePerTime.ToString();
        uPanel.upgradedStorage.text =" +"+ upgradedStorage.ToString();
        upgradeCost = (int)((description.buyPrice / 2) + (lvl) * (description.buyPrice / 5));
        uPanel.cost.text = upgradeCost.ToString() + " $";
        


    }
    public override void TouchObject(HelpPanel panel)
    {
        base.TouchObject(panel);
        this.panel.resourceImage.gameObject.SetActive(true);
        this.panel.resourcetxt.gameObject.SetActive(true);
        this.panel.timetxt.gameObject.SetActive(true);
        this.panel.resourcetxt.text = resourcePerTime.ToString();
        this.panel.timetxt.text = timeInWork.ToString();
        this.panel.Nametxt.text = description.Name + " - " + lvl;
        
    }
    public void Upgrade(int level)
    {
        if (resourceStorage.money < upgradeCost)
        {
           StartCoroutine( interpol.PingPong(uPanel.cost, Color.red));
            return;
        }
        resourceStorage.ChangeBalance(-upgradeCost);
        MergeAdjacentRooms(this);
        IncreaseStats(level);
        upgradeCost = (int)((description.buyPrice / 2) + (level)* (description.buyPrice / 5));
        GetComponent<SpriteRenderer>().sprite = description.upgradedSprite;
        uPanel.Hide();
        if (lvl == 2 && hasJointedObject)
        {
            ChangeView();
            if (GetComponent<ResearchCenter>())
                currentStorage += upgradedStorage * 2;
            else
            currentStorage += upgradedStorage*5;
            
            IncreaseStats(++level);
            lvl++;

        }
    }

    
   
}
