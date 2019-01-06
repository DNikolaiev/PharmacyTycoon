using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorInterpolator : MonoBehaviour {
    private Color startColor;
    
    public IEnumerator PingPong(Text text, Color color2)
    {
        startColor = text.color;
        float timer = 1.5f;
        float duration = 0;
        while (duration < 0.8)
        {
            Debug.Log(duration);
            duration = Mathf.PingPong(Time.time, 1);
            Color newColor = Color.Lerp(text.color, color2, Time.deltaTime*4);
            text.color = newColor;
            
            yield return null;
        }
        while (duration > 0.1)
        {
           
            duration = Mathf.PingPong(Time.time, 1);
            Color newColor = Color.Lerp(text.color, startColor, Time.deltaTime * 4);
            text.color = newColor;
            yield return null;
        }
        text.color = startColor;

    }
}
