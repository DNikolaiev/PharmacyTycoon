using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
[System.Serializable]
public class WorldSaveData
{
    public List<int> areasHealth;
    public WorldSaveData(List<Area> areas)
    {
        areasHealth = new List<int>();
        foreach(Area a in areas)
        {
            areasHealth.Add(a.health);
            
        }
    }
}

public class World : MonoBehaviour {
    public List<Area> areas;
    public Area home;
    public Graph graph;
    public WorldSaveData saveData;
    [SerializeField] private AreaPanel areaPanel;
    [SerializeField] private List<Transform> transforms;
    [SerializeField] private Transform connections;
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Color lineColor;
    [SerializeField] private Color activeLineColor;
    [SerializeField] private Material lineMaterial;
    private LineRenderer line;
    #region transfer costs
    [Header("Transfer costs")]
    [SerializeField] private int africaAsia;
    [SerializeField] private int africaEurope;
    [SerializeField] private int africaNorthAmerica;
    [SerializeField] private int africaSouthAmerica;
    [SerializeField] private int asiaEurope;
    [SerializeField] private int asiaNorthAmerica;
    [SerializeField] private int asiaSouthAmerica;
    [SerializeField] private int europeNorthAmerica;
    [SerializeField] private int europeSouthAmerica;
    [SerializeField] private int americaAmerica;
    #endregion
    public static World instance;
    
    private void OnSave()
    {
        saveData = new WorldSaveData(areas);
    }
    private void OnLoad()
    {
       for (int i =0; i<areas.Count; i++)
        {
            areas[i].health = saveData.areasHealth[i];
            areas[i].CalculateHealthDecrease();
        }
      
    }
   
    private void OnEnable()
    {
        EventManager.StartListening(TurnController.OnTurnEvent, GenerateEpidemic);
        foreach(Area area in areas)
        {
            EventManager.StartListening(TurnController.OnTurnEvent, area.ReduceHealth);
            EventManager.StartListening(TurnController.OnTurnEvent, area.RefreshQuotum);
        }
    }
    private void OnDisable()
    {
        EventManager.StopListening(TurnController.OnTurnEvent, GenerateEpidemic);
        foreach (Area area in areas)
        {
            EventManager.StopListening(TurnController.OnTurnEvent, area.ReduceHealth);
            EventManager.StopListening(TurnController.OnTurnEvent, area.RefreshQuotum);
        }
        
    }
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (instance == null)
        {
            instance = this;
           
        }
        else Destroy(instance);
    }
    
    private void Start()
    {
       CreateWorldConnections();
        GenerateEpidemic();
        SaveController.OnSaveEvent += OnSave;
        SaveController.OnLoadEvent += OnLoad;

        EventManager.StopListening(TurnController.OnTurnEvent, CreateWorldConnections);
        EventManager.StartListening(TurnController.OnTurnEvent, CreateWorldConnections);

    }
    public void GenerateEpidemic()
    {
        Debug.Log("Generated epidemic");
        foreach (Area area in areas)
        {
            area.activeEpidemies.Clear();
            EpidemicGenerator generator = new EpidemicGenerator();
            int min = Mathf.Min(4, area.maxEpidemicCount);
            area.activeEpidemies.AddRange(generator.Generate(min));
           
        }
    }
    private void CreateWorldConnections()
    {
         graph = new Graph();
     
        Node europe = graph.CreateRoot(home, transforms.Find(x => x.name == "Europe").position);
        Node africa = graph.CreateNode(areas.Find(x => x.Name == "Africa"), transforms.Find(x => x.name == "Africa").position);
        Node asia = graph.CreateNode(areas.Find(x => x.Name == "Asia"), transforms.Find(x => x.name == "Asia").position);
        Node nAmerica = graph.CreateNode(areas.Find(x => x.Name == "North America"), transforms.Find(x => x.name == "North America").position);
        Node sAmerica = graph.CreateNode(areas.Find(x => x.Name == "South America"), transforms.Find(x => x.name == "South America").position);

        africa.AddArc(asia, africaAsia).AddArc(europe, africaEurope).AddArc(nAmerica, africaNorthAmerica).AddArc(sAmerica, africaSouthAmerica);
        asia.AddArc(europe, asiaEurope).AddArc(nAmerica, asiaNorthAmerica).AddArc(sAmerica, asiaSouthAmerica);
        europe.AddArc(nAmerica, europeNorthAmerica).AddArc(sAmerica, europeSouthAmerica);
        nAmerica.AddArc(sAmerica, americaAmerica); 
    }
    
    private void ShowCost(Node start, Node destination)
    {
        Canvas instantiated = Instantiate(worldCanvas, Vector3.zero, Quaternion.identity);
        instantiated.transform.SetParent(connections);
        // place instantiated canvas right in the middle of the line
        instantiated.transform.position = new Vector3
            (
            (destination.position.x + start.position.x) / 2,
            (destination.position.y + start.position.y) / 2,
             0);
        Vector3 test = instantiated.transform.localPosition;
        instantiated.transform.localPosition = new Vector3(test.x, test.y, 0);
        instantiated.GetComponentInChildren<TextMeshProUGUI>().text = CalculatePathCost(new List<Node> { start, destination }).ToString();
    }
    private GameObject DrawLine(Vector3 position1, Vector3 position2, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.SetParent(connections);
        myLine.transform.position = Vector3.zero;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.alignment = LineAlignment.TransformZ;
        lr.useWorldSpace = false;
        lr.SetColors(color, color);
        lr.startWidth = .6f;
        lr.SetPosition(0, position1);
        lr.SetPosition(1, position2);
        return myLine;
    }
    public void ShowConnections(Area area)
    {
        areaPanel.GetComponent<PanelMask>().enabled = false;
        Node node = graph.nodes.Find(p => p.area == area);
        DrawPath(node, node);
        areaPanel.SetPanel(area, CalculatePathCost(FindShortestPath(graph.nodes.Where(x => x.area == area).ToList().FirstOrDefault())));
        areaPanel.GetComponent<PanelMask>().enabled = true;
    }
    private void DrawPath(Node start, Node end=null)
    {
        
        for (int i = 0; i<start.arcs.Count; i++)
        {

            DrawLine(start.position, start.arcs[i].destination.position, lineColor);
            ShowCost(start, start.arcs[i].destination);
        }
        if(end!=null && end!=graph.root)
        {

            List<Node> pathNodes = FindShortestPath(end);
            for(int i =1; i<pathNodes.Count; i++)
            {
               GameObject line = DrawLine(pathNodes[i].position, pathNodes[i - 1].position, activeLineColor);
                line.GetComponent<LineRenderer>().sortingOrder += 1;
                ShowCost(pathNodes[i-1], pathNodes[i] );
            }
        }
        
        
    }
    private List<Node> FindShortestPath( Node end)
    {
        SearchEngine search = new SearchEngine(graph);
        return search.DijkstraSearch(graph.nodes.Count, graph.nodes.IndexOf(graph.root), graph.nodes.IndexOf(end));
    }
    public void DestroyConnections()
    {
        Transform[] children = GetTopLevelChildren(connections);
        foreach (Transform child in children) Destroy(child.gameObject);
    }
    public int CalculatePathCost(List<Node> nodes)
    {
        if (nodes.Count == 0) return 0;
        int result = 0;
        for (int i =0; i<nodes.Count-1; i++)
        {
            foreach(Arc arc in nodes[i].arcs)
            {
                if (arc.source==nodes[i] && arc.destination==nodes[i+1])
                {
                    result += arc.weight;
                }
            }
        }
        return result;
    }
    private Transform[] GetTopLevelChildren(Transform Parent)
    {
        Transform[] Children = new Transform[Parent.childCount];
        for (int ID = 0; ID < Parent.childCount; ID++)
        {
            Children[ID] = Parent.GetChild(ID);
        }
        return Children;
    }

 
}
