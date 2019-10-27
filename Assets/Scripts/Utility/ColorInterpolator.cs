using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorInterpolator : MonoBehaviour {
    private Color startColor;
    public Color originalColor = Color.white;
    public float fadeInTime = 1f;
   
    public IEnumerator InOut(Text text, Color endColor, float timeToWait=1f)
    {
        startColor = text.color;
        StartCoroutine(ColorIn(text, text.color, endColor));
        yield return new WaitForSeconds(timeToWait);
        if(originalColor!=Color.black)
            StartCoroutine(ColorIn(text, text.color, originalColor));
        else
            StartCoroutine(ColorIn(text, text.color, startColor));
        yield return null;
    }
    public IEnumerator InOut(Image image, Color endColor, float timeToWait = 1f)
    {
        startColor = image.color;
        StartCoroutine(ColorIn(image, image.color, endColor));
        yield return new WaitForSeconds(timeToWait);
        StartCoroutine(ColorIn(image, image.color, startColor));
        yield return null;
    }
    public IEnumerator ColorIn(Text text, Color startColor, Color endColor)
    {
       
        
        for (float t = 0.01f; t < fadeInTime; t += 0.02f)
        {
            Color newColor = Color.Lerp(text.color, endColor, t/fadeInTime);
            text.color = newColor;
            yield return null;
        }
        text.color = endColor;
        yield return null;
    }
    public IEnumerator ColorIn(Image image, Color startColor, Color endColor)
    {
       

        for (float t = 0.01f; t < fadeInTime; t += 0.001f)
        {
            Color newColor = Color.Lerp(image.color, endColor, t / fadeInTime);
            image.color = newColor;
            yield return null;
        }
        yield return null;
    }

    
}
