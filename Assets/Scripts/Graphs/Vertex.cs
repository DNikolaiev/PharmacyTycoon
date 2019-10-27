using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Vertex : MonoBehaviour, IMultitouchable
{
    public Area area;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void MultiTouch()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            World.instance.DestroyConnections();
        }
        else if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            World.instance.DestroyConnections();

        World.instance.ShowConnections(area);
        anim.Play("ZonePointer_Click");
    }

    



}
