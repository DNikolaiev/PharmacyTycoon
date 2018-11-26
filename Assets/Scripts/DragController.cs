using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isDragging;
    public delegate void Delegate();
    public Delegate onDragMethod, onBeginMethod, onEndMethod;

    private GameObject dragItem;
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (onBeginMethod != null)
            onBeginMethod();
    }

    public void OnDrag(PointerEventData eventData)
    {

        onDragMethod();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (onEndMethod != null)
            onEndMethod();
    }
    public void SetDragItem(GameObject obj)
    {
        dragItem = obj;
    }
    private void FollowCursor()
    {
        dragItem.transform.position = Input.mousePosition;
    }
    private void Start()
    {
        onDragMethod += FollowCursor;
    }
}
