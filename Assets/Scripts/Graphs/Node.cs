using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Node {
    public bool isVisited;
    public Area area;
    public Vector3 position;
    public List<Arc> arcs = new List<Arc>();
    public Node(Area newArea, Vector3 pos)
    {
        area = newArea;
        position = pos;
    }

    public Node AddArc(Node dest, int weight)
    {
        arcs.Add(new Arc
        {
            source = this,
            destination = dest,
            weight = weight
        });
        if (!dest.arcs.Exists(x=>x.source==dest && x.destination==this))
        {
            dest.AddArc(this, weight);
        }
        return this;
    }
}
