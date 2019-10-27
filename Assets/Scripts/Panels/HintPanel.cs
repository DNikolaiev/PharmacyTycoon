using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HintPanel : Panel
{
    
    public Text hintText;
    public Image showImage;
    public bool autoHide = true;
    public int textPadding;
    public float timeBeforeDisappear = 1.75f;
    public Image background;
    RectTransform rect;
    Image img;
    [SerializeField] Canvas canv;
    [SerializeField] Animator anim;
    private void Awake()
    {
        gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        if (canv == null)
            canv = transform.parent.GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        
    }
   
    public override void Hide()
    {
        if (autoHide)
        {
            StopAllCoroutines();
            StartCoroutine(Disappear(timeBeforeDisappear));
        }
        else { gameObject.SetActive(false); background.gameObject.SetActive(false); }
       
    }
   
    IEnumerator Disappear(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    public  void SetPanel(string text, string Name, Sprite toShow=null)
    {
        if (showImage != null)
            showImage.enabled = false;
        gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        hintText.text = text;
        if (toShow != null)
        {
            showImage.enabled = true;
            showImage.sprite = toShow;
        }
        if(Nametxt!=null)
        {
            Nametxt.text = Name;
        }
        anim.Play("HintPanel_Appear");
        if (autoHide)
            Hide();
    }
    public void SetPanel(Vector3 position,  string text, Sprite toShow=null)
    {
        if(showImage!=null)
            showImage.enabled = false;
        gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canv.transform as RectTransform, Input.mousePosition, canv.worldCamera, out pos);
        pos = new Vector2(pos.x-20, pos.y-20);
        transform.position = canv.transform.TransformPoint(pos);

        hintText.text = text;
        Vector2 backgroundSize = new Vector3(
            hintText.preferredWidth + textPadding*2f,
            hintText.preferredHeight + textPadding*2f);
        background.GetComponent<RectTransform>().sizeDelta = backgroundSize;
        if (toShow != null)
        {
            showImage.enabled = true;
            showImage.sprite = toShow;
        }
       
        if(autoHide)
            Hide();
    }
}
