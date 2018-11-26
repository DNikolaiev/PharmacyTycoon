using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HintPanel : Panel
{
    
    public Text hintText;
    private Color originalTextColor;
    private Color originalHeaderColor;
    private Color originalImgColor;
    [SerializeField] private float speedRate;
    RectTransform rect;
    Image img;
    Canvas canv;
    private void Start()
    {
        canv = transform.parent.GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
       // GetOriginalColors();
        gameObject.SetActive(false);
       
       
    }

    private void GetOriginalColors()
    {
        originalTextColor = hintText.color;
        originalHeaderColor = Nametxt.color;
        originalImgColor = img.color;
    }
    private void SetToOriginalColors()
    {
        Nametxt.color = originalHeaderColor;
        hintText.color = originalTextColor;
        img.color = originalImgColor;
    }
    public override void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Disappear(1.7f));
       // StartCoroutine(Fade(speedRate));
    }
    IEnumerator Disappear(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    IEnumerator Fade(float speedRate)
    {
        SetToOriginalColors();
        float _alphaValue = 0;
        float time = 2;
       
        while(time>0)
        {
            time -= 0.001f;
            _alphaValue = img.color.a;
            _alphaValue = Mathf.Lerp(_alphaValue, 0, Time.deltaTime * speedRate);
            img.color = new Color(img.color.r, img.color.g, img.color.b, _alphaValue);
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, _alphaValue);
            Nametxt.color = new Color(Nametxt.color.r, hintText.color.g, hintText.color.b, _alphaValue);
            
            if (_alphaValue < 0.1)
            {
                gameObject.SetActive(false);
                img.color = originalImgColor;
                hintText.color = originalTextColor;
                Nametxt.color = originalHeaderColor;
                yield return null;
            }
            yield return null;
        }
        
           
       
    }
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    public void SetPanel(Vector3 position,  string text)
    {
        gameObject.SetActive(true);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canv.transform as RectTransform, Input.mousePosition, canv.worldCamera, out pos);
        pos = new Vector2(pos.x-100, pos.y-100);
        transform.position = canv.transform.TransformPoint(pos);

        hintText.text = text;
        Hide();
    }
}
