using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragHandler : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler {
    public bool isDragging;
    public delegate void Delegate();
    public Delegate onDragMethod, onBeginMethod, onEndMethod;
    public GameObject dragItem;

    private Canvas mainCanvas;
    
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
    public void ResetDragItem()
    {
        Destroy(dragItem);
        dragItem = null;
    }
    private void FollowCursor()
    {
        if (mainCanvas == null)
            return;
        Vector2 pos;
       RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, mainCanvas.worldCamera, out pos);
       dragItem.transform.position = mainCanvas.transform.TransformPoint(pos);
        

    }
    public void SetCanvas(Canvas canvas)
    {
        mainCanvas = canvas;
    }
    private void Start()
    {
        onDragMethod += FollowCursor;
        
    }

}
