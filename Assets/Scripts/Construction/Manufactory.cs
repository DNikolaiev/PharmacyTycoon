using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public abstract class Manufactory : SceneObject, IUpgradable
{
    public int resourcePerTime;
    public int upgradedResourcePerTime;
    public int upgradedStorage;
    public float productionTime;
    private int upgradeCost;
    public GameObject finishedProductionImage;
    [SerializeField] protected  AudioClip pickUpClip;
    [SerializeField]  protected float timeInWork; // preferably save
    [SerializeField] private ColorInterpolator interpol;
    private HintPanel notification;
    private RaycastHit hit;
    [SerializeField] protected int currentStorage;
    protected ResourceStorage resourceStorage;
    protected abstract bool GainResources(int amount);
    protected abstract void LoseResources(int amount);
    protected abstract void Expand(int amount);
    public int GetCurrentStorage()
    {
        return currentStorage;
    }
    public void SetCurrentStorage(int value)
    {
        currentStorage = value;
    }
    protected void Notify(bool isFull=false)
    {
        GameController.instance.audio.MakeSound(pickUpClip);
        if (GameController.instance.player.hasAutoClicker) return;
       var newMessage= Instantiate(notification.gameObject, notification.transform.parent);
    
        newMessage.GetComponent<HintPanel>().autoHide = false;
        if (!isFull)
        {
            newMessage.GetComponent<HintPanel>().SetPanel(Input.mousePosition, "+" + resourcePerTime, resource);
        }
        else
        {
            newMessage.GetComponent<HintPanel>().SetPanel(Input.mousePosition, "FULL", resource);
        }
       
        for (int i = 0; i < newMessage.transform.childCount;i++) {
           Transform child = newMessage.transform.GetChild(i);
            child.gameObject.AddComponent<AutoFade>().fadeDuration = 2f;
            if (child.childCount > 0)
            {
                child.GetChild(0).GetComponent<Text>().fontSize += 12; child.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
                if (!isFull) child.GetChild(0).GetComponent<Text>().color = Color.white;
                else child.GetChild(0).GetComponent<Text>().color = Color.red;
                child.GetChild(0).gameObject.AddComponent<AutoFade>().fadeDuration = 2f;
            }
        }
        newMessage.SetActive(true);
        
        newMessage.AddComponent<AutoFade>().destroyOnFade = true;
        newMessage.GetComponent<AutoFade>().fadeDuration = 3f;
        newMessage.GetComponent<HintPanel>().background.GetComponent<Image>().enabled = false;
        

    }
    protected override void SellObject()
    {
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
        notification = GameController.instance.buttons.hint;
        
    }
    public override void StartWorking()
    {
        base.StartWorking();
        StartCoroutine(Wait(productionTime));
    }
    private  void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isBusy && canWork && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,LayerMask.GetMask("Touchable")))
        {
            if (panel == null) return;
            if (hit.transform==transform && GameController.instance.IsGameSceneEnabled && !panel.IsPointerOverPanel())
            {
                if (GainResources(resourcePerTime))
                {
                    Notify();
                    StartCoroutine(Wait(productionTime));
                }
                else { Notify(true); return; }
            }
        }
        else if (!isBusy && GameController.instance.player.hasAutoClicker)
        {
            if (GainResources(resourcePerTime))
            {
                Notify();
                StartCoroutine(Wait(productionTime));
            }
        }
        if (panel != null && panel.lastTouched==this)
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
    public override void MergeAdjacentRooms(SceneObject type)
    {
        base.MergeAdjacentRooms(type);

        if (objectToMerge != null)
        {
            
            resourcePerTime += (int)(objectToMerge.GetComponent<Manufactory>().resourcePerTime/1.75f);
            objectToMerge.GetComponent<Manufactory>().resourcePerTime = 0;

            
            
            if (objectToMerge.GetComponent<Manufactory>() != null)
            {
                if (objectToMerge.GetComponent<Manufactory>().finishedProductionImage != null)
                {
                    objectToMerge.GetComponent<Manufactory>().finishedProductionImage.gameObject.SetActive(false);
                    objectToMerge.GetComponent<Manufactory>().finishedProductionImage = null;
                }
                if(objectToMerge.GetComponent<ResearchCenter>()!=null)
                {
                    objectToMerge.GetComponent<ResearchCenter>().onGenerate = null;
                }
            }

        }
    }
    public override void ConfirmUpgrade(UpgradePanel uPanel) //view
    {
        uPanel.upgradeBtn.onClick.RemoveAllListeners();
        base.ConfirmUpgrade(uPanel);
        uPanel.storage.text = "Storage";
        uPanel.production.text = "Production";
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
    public void IncreaseStats(int level, bool expand=true)
    {
        resourcePerTime += upgradedResourcePerTime * (level - 1);
        if(expand)
            Expand(upgradedStorage);
        currentStorage += upgradedStorage;
        SetSellPrice(2);
    }
    public void Upgrade(int level, bool chargePlayer=true)
    {
        if (chargePlayer)
        {
           
            if (resourceStorage.money < upgradeCost)
            {
                lvl--;
                StartCoroutine(interpol.InOut(uPanel.cost, Color.red));
                return;
            }
            resourceStorage.ChangeBalance(-upgradeCost);
            GameController.instance.player.GainExperience(GameController.instance.roomOverseer.GetAllSceneObjects().Where(x => x.GetType() == this.GetType()).ToList().Count*50);
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
        IncreaseStats(level,chargePlayer);
        upgradeCost = (int)((description.buyPrice / 2) + (level)* (description.buyPrice / 5));
        GetComponent<SpriteRenderer>().sprite = description.upgradedSprite;
        if(uPanel!=null)
        uPanel.Hide();
        if (lvl == 2 && hasJointedObject)
        {
            ChangeViewToDouble();
            if (GetComponent<ResearchCenter>())
                currentStorage += upgradedStorage * 2;
            else
            currentStorage += upgradedStorage*5;
            
            IncreaseStats(++level,chargePlayer);
            lvl++;

        }

    }

    
   
}
