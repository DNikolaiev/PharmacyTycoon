using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class MessageBox:MonoBehaviour  {

    public Image image;
    public Text text;
    public Animation animation;
    private RectTransform rect;
    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(1.75f);
        animation.Play("MessageBox_Hide");
    }

    public  void Show(string txt, Sprite sprite=null)
    {
        image.enabled = (sprite == null) ? false : true;
        text.text = txt;
        image.sprite = sprite;
        animation.Play("MessageBox_Show");
        StartCoroutine(Hide());
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        ReturnToOrigin();
    }
    public void ReturnToOrigin()
    {
        rect.offsetMin = new Vector2(0, 200);
        rect.offsetMax = new Vector2(0, 200);
    }
}
