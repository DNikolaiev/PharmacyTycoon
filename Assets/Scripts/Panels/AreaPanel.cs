using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AreaPanel : Panel
{
    [SerializeField] TextPanel textPanel;
    [SerializeField] GameObject close;
    [SerializeField] GameObject info;
    [SerializeField] Text deadText;
    [SerializeField] Text aliveText;
    [SerializeField]Text transferText;
    [SerializeField] Animation anim;
    Area area;
    int transferCost;
    public override void Hide()
    {
        
        World.instance.DestroyConnections();
        gameObject.SetActive(false);
    }

    public override void SetPanel()
    {
        
    }
    
 
    private void Start()
    {
       
        Hide();
    }
    public void SetPanel(Area area, int transferCost)
    {
        anim.Play("AreaPanel_Appear");
        info.SetActive(true);
        close.SetActive(false);
        gameObject.SetActive(true);
        this.transferCost = transferCost;
        this.area = area;
        this.Nametxt.text = area.Name;
        transferText.text = transferCost.ToString();
        deadText.text = area.dead.ToString();
        aliveText.text = area.cured.ToString();
        GetComponent<BarFiller>().SetValueToBarScalar(area.health, GetComponent<BarFiller>().healingBar, area.maxHealth);
        
    }
    public void GoToArea()
    {
       FindObjectOfType<Seller>().SaleOn(area, transferCost);
       
    }
    public void OpenInfo()
    {
        Debug.Log("openinfo");
        textPanel.SetPanel(area,"Traits");
    }
}
