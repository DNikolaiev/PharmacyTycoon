using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ResearchHolder : TalentHolder, IPointerUpHandler {

    public Text remainingTime;
    public Sprite inWorkSprite;
    public Image touchHold;
    public ParticleSystem onResearch;
    public ResourcePanel resourcePanel;
    public ResearchPanel researchPanel;
    private Researcher researcher;
    [SerializeField] Animator anim;
    [SerializeField] private float timeToHoldTouch;

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }
    private void Start()
    {
        researcher = GameController.instance.researcher;
        descriptionPanel = GameController.instance.buttons.GetDescriptionPanel(this);
       
        researchPanel = descriptionPanel.transform.GetChild(1).GetComponent<ResearchPanel>();
        resourcePanel = descriptionPanel.transform.GetChild(2).GetComponent<ResourcePanel>();
    }
    public override void SetPanel()
    {
        Debug.Log("ResearchHolder set panel");
        if (!Talent.isUnlocked && Talent.canBeUnlocked)
        {
            picture.enabled = true;
            picture.sprite = Talent.lockedSprite;
            lockedSprite.SetActive(false);
        }
        else if (!Talent.isUnlocked && !Talent.canBeUnlocked)
        {
            picture.enabled = false;
            lockedSprite.SetActive(true);
        }
      else { picture.enabled = true; picture.sprite = Talent.description.sprite; lockedSprite.SetActive(false); }
       
    }
    public override void SetDescription()
    {
        if (!lockedSprite.activeInHierarchy && !Talent.isUnlocked)
        {
            descriptionPanel.SetPanel(Talent, this);
            researchPanel.SetPanel(Talent);
            resourcePanel.Hide();
        }
        else if (!lockedSprite.activeInHierarchy && Talent.isUnlocked)
        {
            descriptionPanel.SetPanel(Talent, this);
            researchPanel.Hide();
            resourcePanel.SetPanel(Talent.characteristics);
        }
    }

    
    public void Research()
    {
        if (!researcher.isResearching)
        {
            researcher.Research(Talent.id);
            picture.sprite = inWorkSprite;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && ClickedInsideHolder())
            descriptionPanel.GetComponent<PanelMask>().enabled = false;
        if (Input.GetMouseButton(0) && !researcher.isResearching && ClickedInsideHolder() && !Talent.isUnlocked
            && !lockedSprite.activeInHierarchy && ClickedInsideHolder() && GameController.instance.player.resources.ResearchPoints >= Talent.description.buyPrice)
        {
            descriptionPanel.GetComponent<PanelMask>().enabled = false;
            this.touchHold.fillAmount += 1f / timeToHoldTouch * Time.deltaTime;
            if (touchHold.fillAmount >= 1)
                Research();
        }
        else if (Input.GetMouseButtonUp(0))
        {

            descriptionPanel.GetComponent<PanelMask>().enabled = true;
            touchHold.fillAmount = 0;
            return;
        }
        else if (!ClickedInsideHolder() && !descriptionPanel.isActive)
        {
            descriptionPanel.GetComponent<PanelMask>().enabled = false;
        }

        if (remainingTime != null && researcher.researchable == this.Talent
            && researcher.isResearching)
        {
            remainingTime.gameObject.SetActive(true);
            remainingTime.text = Mathf.RoundToInt(researcher.timeToWait).ToString();
        }
        else if (remainingTime.gameObject.activeInHierarchy)
        {
            remainingTime.gameObject.SetActive(false);
        }
    }
    public void ChangeHolderPicture(bool state)
    {
        if (Talent == null) return;
        if (state == true && Talent.isUnlocked)
        {
            picture.enabled = true;
            picture.sprite = Talent.description.sprite;
        }
        else if (state==true&&Talent.canBeUnlocked)
        {
            picture.enabled = true;
            picture.sprite = Talent.lockedSprite;
        }
        else
        {
            picture.enabled = false;
        }
    }



    
   public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        anim.SetBool("getBig", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        anim.SetBool("getBig", false);
    }
}
