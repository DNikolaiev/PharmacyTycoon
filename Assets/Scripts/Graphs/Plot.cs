using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Plot : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI plotText;
    [SerializeField] private string plotName;
    [SerializeField] private int separatorCount;
    [SerializeField] RectTransform plotContainer;
    [SerializeField] RectTransform labelXTemplate;
    [SerializeField] RectTransform labelYTemplate;
    [SerializeField] RectTransform gridXTemplate;
    [SerializeField] RectTransform gridYTemplate;
    [SerializeField] HintPanel tooltip;
   [SerializeField] private List<GameObject> gameObjects;

    private void Start()
    {
        plotText.text = plotName;
    }
    // Use this for initialization
    /*void Start () {
        List<int> valueList = new List<int>() { 5000, 98000, 5600, 30000, 22000, 1700, 15000, 130000, 1700,-2000,10000,140000,9000,6000 };
        List<int> valueList2 = new List<int>() { -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000, -10000 };
        IPlotVisual visual  = new LineGraphVisual(plotContainer, dotSprite, dotSize, true, Color.red, Color.white);
        IPlotVisual barVisual = new BarChartVisual(plotContainer, Color.cyan, 0.92f);
        IPlotVisual redVisual = new LineGraphVisual(plotContainer, dotSprite, dotSize, false, Color.red, Color.white);
        List<float> extrems = ShowPlot(valueList, barVisual, 12,0,0,
            (int i) =>  GameDate.CreateDate(Month.January, 2000 + i, 1).year.ToString(),
            (float f) => String.Format("{0:n0} $", Mathf.RoundToInt(f)));
        
        ShowPlot(valueList,visual,12,extrems[0], extrems[1], displayGrid:false);
       
    }*/

    private void ShowTooltip(Vector2 position, string text)
    {
       
        
        tooltip.autoHide = true;
        tooltip.SetPanel(position, text);
    }
    public List<float> ShowPlot(List<int> values, IPlotVisual visual, int maxVisibleAmount, RectTransform container, float yMax = 0, float yMin = 0,
      Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null,
        bool displayGrid = true)
    {
        if (values.Count == 0)
            return new List<float> { 0 };
        plotContainer = container;
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int i) { return ""; };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float i) { return ""; };
        }
        if (maxVisibleAmount <= 0)
        {
            maxVisibleAmount = values.Count;
        }
        float plotHeight = plotContainer.sizeDelta.y;
        float plotWidth = plotContainer.sizeDelta.x;
        float yMaximum = values.Max();
        float yMinimum = values.Min();
        float difference = yMaximum - yMinimum;
        if (difference <= 0)
        {
            difference = 5f;
        }
        if (yMax == 0 && yMin == 0)
        {
            yMaximum = yMaximum + (difference) * 0.15f;
            yMinimum = yMinimum - (difference) * 0.15f;
            yMax = yMaximum; yMin = yMinimum;
        }
        else
        {
            yMaximum = yMax;
            yMinimum = yMin;
        }
        float xSize = plotWidth / (maxVisibleAmount + 1);

        //spawn dots
        int xIndex = 0;

        for (int i = Mathf.Max(values.Count - maxVisibleAmount, 0); i < values.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((values[i] - yMinimum) / (yMaximum - yMinimum)) * plotHeight;
            
            float currentValue = values[i];
            List<GameObject> visualObjects = new List<GameObject>();
            visualObjects.AddRange(visual.AddVisual(new Vector2(xPosition, yPosition), xSize));
            visualObjects.ForEach(x => x.AddComponent<Button>().onClick.AddListener(delegate { ShowTooltip(Input.mousePosition, getAxisLabelY(currentValue)); }));
            gameObjects.AddRange(visualObjects);
            // create x labels 
            RectTransform labelX = Instantiate(labelXTemplate, plotContainer);
            gameObjects.Add(labelX.gameObject);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -10f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            // create visual x grid
            if (gridXTemplate != null && displayGrid)
            {
                RectTransform gridX = Instantiate(gridXTemplate, plotContainer);
                gameObjects.Add(gridX.gameObject);
                gridX.transform.SetAsFirstSibling();
                gridX.gameObject.SetActive(true);
                gridX.anchoredPosition = new Vector2(xPosition, 170f);
            }

            xIndex++;
        }

        for (int i = 0; i < separatorCount; i++)
        {
            //create y labels
            RectTransform labelY = Instantiate(labelYTemplate, plotContainer);
            gameObjects.Add(labelY.gameObject);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-50, normalizedValue * plotHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            //create y grid
            if (gridYTemplate != null && displayGrid)
            {
                RectTransform gridY = Instantiate(gridYTemplate, plotContainer);
                gridY.transform.SetAsFirstSibling();
                gameObjects.Add(gridY.gameObject);
                gridY.gameObject.SetActive(true);
                gridY.anchoredPosition = new Vector2(290, normalizedValue * plotHeight);
            }
        }
        return new List<float> { yMax, yMin };
    }

    public List<float> ShowPlot(List<float> values, IPlotVisual visual, int maxVisibleAmount, RectTransform container, float yMax=0,  float yMin=0, 
      Func<int,string> getAxisLabelX=null, Func<float,string> getAxisLabelY=null,
        bool displayGrid=true)
    {
        if (values.Count == 0)
            return new List<float> { 0 };
        plotContainer = container;
        if(getAxisLabelX==null)
        {
            getAxisLabelX = delegate (int i) { return ""; };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float i) { return ""; };
        }
        if(maxVisibleAmount<=0)
        {
            maxVisibleAmount = values.Count;
        }
        float plotHeight = plotContainer.sizeDelta.y;
        float plotWidth = plotContainer.sizeDelta.x;
        float yMaximum = values.Max();
        float yMinimum = values.Min();
        float difference = yMaximum - yMinimum;
        if(difference <=0)
        {
            difference = 5f;
        }
        if (yMax == 0 && yMin == 0)
        {
            yMaximum = yMaximum + (difference) * 0.15f;
            yMinimum = yMinimum - (difference) * 0.15f;
            yMax = yMaximum; yMin = yMinimum;
        }
        else
        {
            yMaximum = yMax;
            yMinimum = yMin;
        }
        float xSize = plotWidth/(maxVisibleAmount+1);
      
        //spawn dots
        int xIndex = 0;
       
        for (int i = Mathf.Max(values.Count-maxVisibleAmount,0); i < values.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((values[i]-yMinimum) / (yMaximum-yMinimum)) * plotHeight;
           
           
            float currentValue = values[i];
            List<GameObject> visualObjects = new List<GameObject>();
            visualObjects.AddRange(visual.AddVisual(new Vector2(xPosition, yPosition), xSize));
            visualObjects.ForEach(x => x.AddComponent<Button>().onClick.AddListener(delegate { ShowTooltip(Input.mousePosition,getAxisLabelY(currentValue)); }));
            gameObjects.AddRange(visualObjects);
            // create x labels 
            RectTransform labelX = Instantiate(labelXTemplate, plotContainer);
            gameObjects.Add(labelX.gameObject);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -10f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            // create visual x grid
            if (gridXTemplate != null && displayGrid)
            {
                RectTransform gridX = Instantiate(gridXTemplate, plotContainer);
                gameObjects.Add(gridX.gameObject);
                gridX.transform.SetAsFirstSibling();
                gridX.gameObject.SetActive(true);
                gridX.anchoredPosition = new Vector2(xPosition, 170f);
            }

            xIndex++;
        }
       
        for (int i = 0; i < separatorCount; i++)
        {
            //create y labels
            RectTransform labelY = Instantiate(labelYTemplate, plotContainer);
            gameObjects.Add(labelY.gameObject);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i *1f/ separatorCount;
            labelY.anchoredPosition = new Vector2(-50, normalizedValue*plotHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum-yMinimum)));
            //create y grid
            if (gridYTemplate != null && displayGrid)
            {
                RectTransform gridY = Instantiate(gridYTemplate, plotContainer);
                gridY.transform.SetAsFirstSibling();
                gameObjects.Add(gridY.gameObject);
                gridY.gameObject.SetActive(true);
                gridY.anchoredPosition = new Vector2(290, normalizedValue * plotHeight);
            }
        }
        return new List<float> { yMax, yMin };
    }
   
    public void Clear()
    {
        gameObjects.ForEach(x => Destroy(x));
        gameObjects.Clear();
    }
}

public class BarChartVisual: IPlotVisual
{
    private RectTransform container;
    private Color barColor;
    private Color secondColor;
    private float widthMultiplier;
    private int counter=0;
    public BarChartVisual(RectTransform container, Color barColor, float barWidthMultiplier, Color secondColor)
    {
        this.container = container;
        this.barColor = barColor;
        this.secondColor = secondColor;
        widthMultiplier = barWidthMultiplier;
       
    }
    public List<GameObject> AddVisual(Vector2 position, float width)
    {
        GameObject bar = CreateBar(position, width);
        return new List<GameObject>() { bar };
    }
   
    private GameObject CreateBar(Vector2 anchoredPosition, float barWidth)
    {
        GameObject gameObj = new GameObject("bar", typeof(Image));
        gameObj.transform.SetParent(container, false);
        if (counter % 2 != 0)
            gameObj.GetComponent<Image>().color = barColor;
        else gameObj.GetComponent<Image>().color = secondColor;
        RectTransform rect = gameObj.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(anchoredPosition.x, 0);
        rect.sizeDelta = new Vector2(barWidth *widthMultiplier, anchoredPosition.y);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(.5f, 0f);
        counter++;
        return gameObj;
    }
}
public class LineGraphVisual: IPlotVisual
{
    private RectTransform container;
    private Sprite dotSprite;
    private int dotSize;
    private GameObject lastDot;
    private bool displayDots;
    private Color connectionColor;
    private Color dotColor;
    public LineGraphVisual(RectTransform container, Sprite dotSprite, int dotSize, bool displayDots, 
        Color connectionColor, Color dotColor)
    {
        this.container = container;
        this.dotSprite = dotSprite;
        this.dotSize = dotSize;
        this.displayDots = displayDots;
        this.connectionColor = connectionColor;
        this.dotColor = dotColor;
        lastDot = null;
    }
    public List<GameObject> AddVisual(Vector2 position, float width)
    {
        List<GameObject> gameObjects = new List<GameObject>();
          GameObject dotObject = CreateDot(position,displayDots);
          gameObjects.Add(dotObject);
          
          if(lastDot!=null)
          {
             GameObject connection = CreateConnection(lastDot.GetComponent<RectTransform>().anchoredPosition, 
                 dotObject.GetComponent<RectTransform>().anchoredPosition, connectionColor);
            gameObjects.Add(connection);
          }
          lastDot = dotObject;
        return gameObjects;
    }
    private GameObject CreateDot(Vector2 anchoredPosition, bool displayDot)
    {
        GameObject gameObj = new GameObject("dot", typeof(Image));
        gameObj.transform.SetParent(container, false);
        gameObj.GetComponent<Image>().sprite = dotSprite;
        gameObj.GetComponent<Image>().color = dotColor;
        RectTransform rect = gameObj.GetComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(dotSize, dotSize);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);

        if (!displayDot)
            gameObj.GetComponent<Image>().enabled = false;
        return gameObj;
    }
    private GameObject CreateConnection(Vector2 dotPositionA, Vector2 dotPoitionB, Color color)
    {
        GameObject gameObj = new GameObject("connection", typeof(Image));
        gameObj.transform.SetParent(container, false);
        gameObj.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObj.GetComponent<RectTransform>();
        Vector2 direction = (dotPoitionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPoitionB);
        rectTransform.anchoredPosition = (dotPositionA + dotPoitionB) / 2;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        return gameObj;

    }
    public static float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
