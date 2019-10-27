using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoFade : MonoBehaviour {
    private Color originalTextColor;
    private Text text;
    public float fadeDuration;
    public bool destroyOnFade;
    private void Start()
    {
        GetComponent<MaskableGraphic>().CrossFadeAlpha(0, fadeDuration, true);
    }
    private void Update()
    {
        if (destroyOnFade && GetComponent<MaskableGraphic>().canvasRenderer.GetAlpha() == 0)
            Destroy(gameObject);
      
    }

}
