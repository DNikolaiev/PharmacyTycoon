using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Graph  {
    public Node root;
    public List<Node> nodes = new List<Node>();
    public int?[,] adjacentMatrix;
    public Node CreateRoot(Area area, Vector3 pos)
    {
        root = CreateNode(area, pos);
        return root;
    }
    public Node CreateNode(Area area, Vector3 pos)
    {
        var n = new Node(area, pos);
        nodes.Add(n);
        return n;
    }
    public List<Node> GetAdjacentNodes(Node node)
    {
        List<Node> adjacents = new List<Node>();
        foreach(Arc arc in node.arcs)
        {
            adjacents.Add(arc.destination);
        }
        return adjacents;
    }
    public int?[,] CreateAdjacentMatrix()
    {
        adjacentMatrix = new int?[nodes.Count, nodes.Count];
        for (int i=0; i<nodes.Count; i++)
        {
            Node n1 = nodes[i];
            for(int j=0; j<nodes.Count; j++)
            {
                Node n2 = nodes[j];
                if(i==j)
                {
                    adjacentMatrix[i, j] = 0; continue;
                }
                var arc = n1.arcs.FirstOrDefault(p => p.destination == n2);
                if(arc!=null)
                {
                    adjacentMatrix[i, j] = arc.weight;
                }
            }
        }

        return adjacentMatrix;
    }
	
}
