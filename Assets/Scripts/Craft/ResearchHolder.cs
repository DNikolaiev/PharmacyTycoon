using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResearchHolder : TalentHolder {

    public Text remainingTime;
    public Sprite inWorkSprite;
    public GameObject lockedSprite;
    public Image touchHold;

    [SerializeField] private float timeToHoldTouch;

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPanel()
    {
        if (!Talent.isUnlocked)
        {
            picture.sprite = Talent.lockedSprite;
            lockedSprite.SetActive(true);
        }
        else { picture.sprite = Talent.description.sprite; lockedSprite.SetActive(false); }
    }
    public override void SetDescription()
    {
        if (!lockedSprite.activeInHierarchy)
            descriptionPanel.SetPanel(Talent,this);
    }

    public void GetHint()
    {
        if (Player.instance.level < 5)
            ButtonController.instance.hint.SetPanel(Input.mousePosition, "PRESS & HOLD to research");
    }
    public void Research()
    {
        if (!Researcher.instance.isResearching)
        {
            Researcher.instance.Research(Talent.id);
            picture.sprite = inWorkSprite;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && ClickedInsideHolder())
            descriptionPanel.GetComponent<PanelMask>().enabled = false;
        if (Input.GetMouseButton(0) && !Researcher.instance.isResearching && ClickedInsideHolder() && !Talent.isUnlocked
            && !lockedSprite.activeInHierarchy && ClickedInsideHolder() && Player.instance.resources.ResearchPoints >= Talent.description.buyPrice)
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

        if (remainingTime != null && Researcher.instance.researchable == this.Talent
            && Researcher.instance.isResearching)
        {
            remainingTime.gameObject.SetActive(true);
            remainingTime.text = Researcher.instance.timeToWait.ToString();
        }
        else if (remainingTime.gameObject.activeInHierarchy)
        {
            remainingTime.gameObject.SetActive(false);
        }
    }
    public void ChangeHolderPicture(bool state)
    {
        if (state == true && Talent.isUnlocked)
        {
            picture.sprite = Talent.description.sprite;
        }
        else picture.sprite = Talent.lockedSprite;
    }
}
