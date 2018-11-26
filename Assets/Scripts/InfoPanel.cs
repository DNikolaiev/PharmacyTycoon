using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public class InfoPanel: Panel, IPointerDownHandler, IPointerUpHandler {
    
    public Text descriptiontxt;
    public Text pricetxt;
    public Image image;
    public Image resourceImage;
    public SceneObject objectInfo;
    [Range(1,5)]
    [SerializeField] float scaleSpeed;
    [SerializeField] float scaleModifier;
    private Vector3 originalScale;
    private RectTransform rect;
  
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        SetPanel();
    }
    public override void SetPanel()
    {
        Nametxt.text = objectInfo.description.Name;
        descriptiontxt.text = objectInfo.description.description;
        pricetxt.text = objectInfo.description.buyPrice.ToString() + " $";
        image.sprite = objectInfo.description.sprite;
    }

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }
    private void FixedUpdate()
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScale(scaleSpeed, scaleModifier));
    }
    IEnumerator ChangeScale(float speed, float desiredScale)
    {
        originalScale = rect.localScale;
        while (rect.localScale.y >= desiredScale)
        {
            rect.localScale = Vector3.Lerp(rect.localScale, rect.localScale * (1/scaleSpeed), scaleSpeed * Time.deltaTime);

            yield return null;
        }
        
    }
    IEnumerator ReturnScale(float speed)
    {
        while (rect.localScale.y<=originalScale.y)
        {
            rect.localScale = Vector3.Lerp(rect.localScale, originalScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ReturnScale(scaleSpeed));
    }
}
