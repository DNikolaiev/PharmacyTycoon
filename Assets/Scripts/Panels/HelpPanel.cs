using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] Tutorial tutorial;
    public  bool IsPointerOverPanel()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
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
        if (lastTouched.GetComponent<IUpgradable>()==null || lastTouched.lvl>=2 || !GameController.instance.generalTutorial.isTutorialCompleted)
        {
            upgrade.interactable = false;
            upgrade.GetComponentInChildren<ParticleSystem>().Stop();
            
        }
        if(lastTouched.lvl<2) {  upgrade.GetComponentInChildren<ParticleSystem>().Play();  }
        objectInfo.TouchObject(GetComponent<HelpPanel>());
        sell.onClick.AddListener(lastTouched.ConfirmSale);
        
    }
    public void StartTutorial()
    {
        if(!tutorial.isTutorialCompleted)
        {
            tutorial.StartTutorial();
        }
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
