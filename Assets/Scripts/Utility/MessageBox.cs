using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public  class MessageBox:MonoBehaviour  {

    public Image image;
    public Text text;
    public Animation anim;
    public float waitTime = 2f;
    private RectTransform rect;
    private Dictionary<string, Sprite> queue = new Dictionary<string, Sprite>();
    private bool isShowing;
    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(waitTime);
        if(anim!=null)
            anim.Play("MessageBox_Hide");
        isShowing = false;
        if(queue.Count>0)
        {
            Show(queue.LastOrDefault().Key, queue.LastOrDefault().Value);
        }
    }

    public  void Show(string txt, Sprite sprite=null)
    {
        
        if(!queue.ContainsKey(txt))
            queue.Add(txt, sprite);
        if (isShowing) return;
        isShowing = true;
        image.enabled = (sprite == null) ? false : true;
        text.text = txt;
        image.sprite = sprite;
        if (anim != null)
            anim.Play("MessageBox_Show");
        queue.Remove(txt);
        StartCoroutine(Hide());
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        queue = new Dictionary<string, Sprite>();
    }
    private void Start()
    {
        
        anim = GetComponent<Animation>();
        ReturnToOrigin();
    }
    public void ReturnToOrigin()
    {
        rect.offsetMin = new Vector2(0, 200);
        rect.offsetMax = new Vector2(0, 200);
    }
}
