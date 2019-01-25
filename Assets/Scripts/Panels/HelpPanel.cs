using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelpPanel : Panel {

    
    public Text resourcetxt;
    public Text timetxt;
    public Image timeImage;
    public Image resourceImage;
    public Button sell;
    public Button upgrade;
    
    private RectTransform rect;
    public SceneObject lastTouched;
    public Animation slideAnim;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        Hide();
    }
    public void SetPanel(ITouchable objectInfo)
    {
        
        gameObject.SetActive(true);
        upgrade.interactable = true;
        slideAnim.Play();
        sell.onClick.RemoveAllListeners();
        
        lastTouched = (SceneObject)objectInfo;
        if (lastTouched.GetComponent<IUpgradable>()==null || lastTouched.lvl>2)
        {
            upgrade.interactable = false;
        }
        objectInfo.TouchObject(GetComponent<HelpPanel>());
        sell.onClick.AddListener(lastTouched.ConfirmSale);
        
    }
   
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    public override void Hide()
    {
        
        rect.offsetMin = new Vector2(0, -200);
        rect.offsetMax = new Vector2(0, -200);
       
    }
   
    
}
