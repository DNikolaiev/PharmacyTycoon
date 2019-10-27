using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    Vector3 startPoint;

    RectTransform thisRect;
    public void MoveToUI(Vector3 endPoint, float speed)
    {
        startPoint = GetComponent<RectTransform>().localPosition;
        thisRect = GetComponent<RectTransform>();
        StartCoroutine(MovingCoroutine(endPoint,speed));
    }
    private IEnumerator MovingCoroutine(Vector3 endPoint, float speed)
    {
        yield return new WaitForSeconds(0.5f);
        float counter = 0;
        while(counter <1)
        {
            counter += Time.deltaTime * (1/speed);
            thisRect.localPosition = Vector3.Lerp(startPoint, endPoint, counter);
            yield return null;
        }
    }
}
