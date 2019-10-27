using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scaler : MonoBehaviour {

    public float maxSize;
    public float minSize;
    public float scaling;
    public float growFactor;
    public float waitTime;
    public float slideFactor;
    public bool scaleOnAwake;
    
    private Vector3 scaleOffset;
    private Vector3 tempScale;
    
    private void Start()
    {
        scaleOffset = transform.localScale;
    }
    private void Update()
    {
        if(scaleOnAwake)
        {
            tempScale = scaleOffset;
            tempScale.y += Mathf.Sin(Time.fixedTime * growFactor) * scaling;
            tempScale.x += Mathf.Sin(Time.fixedTime * growFactor) * scaling;
            
            transform.localScale= tempScale;
        }
    }
    public IEnumerator Scale(GameObject obj)
    {
        float timer = 0;


        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        float startSize = obj.transform.localScale.x;
        while (true)
        {
            while (maxSize > obj.transform.localScale.x)
            {
                timer += Time.deltaTime;
                obj.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(waitTime);

            timer = 0;
            while (minSize < obj.transform.localScale.x)
            {
                timer += Time.deltaTime;
                obj.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(waitTime);
        }
       
    }
    public IEnumerator Scale(Text obj)
    {
        float timer = 0;
        Vector3 position = obj.GetComponent<RectTransform>().localPosition;
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (maxSize > obj.transform.localScale.x)
        {
            timer += Time.deltaTime;
            obj.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            obj.GetComponent<RectTransform>().localPosition+= new Vector3(1, 0, 0) * Time.deltaTime * slideFactor;
            yield return null;
        }
        // reset the timer

        yield return new WaitForSeconds(waitTime);

        timer = 0;
        while (1 < obj.transform.localScale.x)
        {
            timer += Time.deltaTime;
            obj.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            obj.GetComponent<RectTransform>().localPosition-= new Vector3(1, 0,0) * Time.deltaTime * slideFactor;
            yield return null;
        }
        obj.GetComponent<RectTransform>().localPosition = position;
        obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        timer = 0;
        yield return new WaitForSeconds(waitTime);
        yield break;
    }

}
