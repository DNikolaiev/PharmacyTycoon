using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SearchEngine  {
    public Graph graph;
    public SearchEngine(Graph graph)
    {
        this.graph = graph;
        graph.CreateAdjacentMatrix();
       
    }
   
    private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < verticesCount; ++v)
        {
            if (shortestPathTreeSet[v] == false && distance[v] <= min)
            {
                min = distance[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

  
    public List<Node> DijkstraSearch( int verticesCount, int source, int end)
    {
        
        int[] prev = new int[verticesCount];
        int[] distance = new int[verticesCount];
        bool[] shortestPathTreeSet = new bool[verticesCount];

        for (int i = 0; i < verticesCount; ++i)
        {
            distance[i] = int.MaxValue;
            shortestPathTreeSet[i] = false;
        }

        distance[source] = 0;

        for (int count = 0; count < verticesCount - 1; ++count)
        {
            
            int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
            shortestPathTreeSet[u] = true;
            if (u == end) break;
            for (int v = 0; v < verticesCount; ++v)
                if (!shortestPathTreeSet[v] && graph.adjacentMatrix[u, v]!=0 && distance[u] != int.MaxValue && distance[u] + graph.adjacentMatrix[u, v] < distance[v])
                {
                    prev[v] = u;
                    distance[v] = distance[u] + (int)graph.adjacentMatrix[u, v];
                }
            
        }
      
        var res = new Stack<int>();
        var cur = end;
        while (cur != source)
        {
            res.Push(cur);
            cur = prev[cur];
        }
        res.Push(source);
        List<Node> resultList = new List<Node>();
        while(res.Count>0)
        {
            resultList.Add(graph.nodes[res.Pop()]);
            
        }
        
        return resultList;
    }


}
