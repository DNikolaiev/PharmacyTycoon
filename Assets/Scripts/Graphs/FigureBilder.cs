using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class FigureBilder : MonoBehaviour {
    [SerializeField] Sprite dotSprite;
    [SerializeField] Plot plot1;
    [SerializeField] Plot plot2;
    [SerializeField] Plot plot3;
    [SerializeField] Plot plot4;
    private int index = 0;
    [SerializeField] List<RectTransform> containers;
    
    private void Start()
    {
        DrawVisualInfo();
        EventManager.StartListening(TurnController.OnTurnEvent, DrawVisualInfo);
        
    }
    private void OnEnable()
    {
        DrawVisualInfo();
    }
    private void DrawVisualInfo()
    {
        PlotAreaData();
        PlotInvestments();
        if(GameController.instance.player.finances.revenue.Count>4)
        PlotBreakEvenPoint();
    }
    private void PlotAreaData()
    {
        plot1.Clear();
        List<string> areaNames = new List<string>() { "Africa", "Africa", "Asia", "Asia", "Europe", "Europe", "\nNorth\n America","\nNorth\n America","\nSouth\n America", "\nSouth\n America" };
        // temporary
        List<Area> areas = World.instance.areas;
        
        List<int> data = new List<int>();
        foreach (Area a in areas)
        {
            Debug.Log(a.cured);
            data.Add(a.dead);
            data.Add(a.cured);
        }
        
        BarChartVisual barVisual = new BarChartVisual(containers[0], Color.white, 0.9f, Color.black);
        plot1.ShowPlot(data, barVisual, -1,containers[0],0, 0, 
            (int i) => areaNames[i].ToString(), (float f) => Mathf.RoundToInt(f).ToString());
    }
    private void PlotInvestments()
    {
        plot2.Clear();
        plot3.Clear();
        index = 0;
        Finances finances = GameController.instance.player.finances;
        if (finances.investments.Count == 0 || finances.dates.Count == 0) return;
        LineGraphVisual lineVisual = new LineGraphVisual(containers[1], dotSprite, 12, true, Color.green, Color.white);
        LineGraphVisual bankruptVisual = new LineGraphVisual(containers[1], dotSprite, 12, false, Color.red, Color.red);
       List<float> extrems = plot2.ShowPlot(finances.investments, lineVisual, 12, containers[1],
            0, 0, (int i) => finances.dates[i].month.ToString().Substring(0,3),
            (float f)=> String.Format("{0:n0} $", Mathf.RoundToInt(f)));
        List<int> bankruptStates = new List<int>();
        for(int i=0;i<12;i++)
        {
            bankruptStates.Add(finances.bankruptState);
        }
        plot2.ShowPlot(bankruptStates, bankruptVisual, -1, containers[1], extrems[0], extrems[1], displayGrid: false);

        // build plot in years
        LineGraphVisual yearLineVisual = new LineGraphVisual(containers[2], dotSprite, 12, true, Color.yellow, Color.green);
        List<int> yearData = new List<int>();
        List<int> years = new List<int>();
        while (index < finances.investments.Count)
        {
            int sum = 0;
            for (int i = index; i < index + 12; i++)
            {
                if (i >= finances.investments.Count) break;
                sum += finances.investments[i];
            }
            sum /= 12; yearData.Add(sum);
            years.Add(finances.dates[index].year);
            index += 12;
        }
        
        
        plot3.ShowPlot(yearData, yearLineVisual, 12, containers[2], 0, 0,
            (int i) => years[i].ToString(),
            (float f) => String.Format("{0:n0} $", Mathf.RoundToInt(f)));
    }
    private void PlotBreakEvenPoint()
    {
        plot4.Clear();
        Finances finances = GameController.instance.player.finances;

        List<int> priceData = new List<int>();
        priceData.AddRange(finances.revenue);
        priceData.Sort();

        List<int> quantityData = finances.GetQuantityData();

        Regression regression = new Regression();
        float a = 0; float b = 0; float rSq = 0;
        //regression.LinearRegression(quantityData.ToArray(), finances.revenue.ToArray(),out a, out b, out rSq);

        List<float> regressionData = new List<float>();

        float xSize = (quantityData.Max() - quantityData.Min()) / quantityData.Count;

        List<float> quantityNormalizedData = new List<float>() { 0 };
        for (int i = 0; i < quantityData.Count; i++)
        {
            quantityNormalizedData.Add(quantityNormalizedData.LastOrDefault() + xSize);
        }
        float ySize = (priceData.Max() - priceData.Min()) / priceData.Count;
        List<float> regressionDataNormalized = new List<float>() { 0 };
        for (int i = 0; i < priceData.Count; i++)
        {
            regressionDataNormalized.Add(regressionDataNormalized.LastOrDefault() + ySize);
        }
        regressionDataNormalized.Remove(regressionDataNormalized.LastOrDefault());
        //  regressionData.AddRange(regression.Function(quantityNormalizedData, a, b));
        List<int> x = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        LineGraphVisual originalVisual = new LineGraphVisual(containers[3], dotSprite, 12, false, new Color(1, 1, 1, 0.0f), Color.white);
        LineGraphVisual revenueVisual = new LineGraphVisual(containers[3], dotSprite, 12, false, Color.cyan, Color.white);
        LineGraphVisual regressionVisual = new LineGraphVisual(containers[3], dotSprite, 12, false, Color.yellow, Color.white);
        LineGraphVisual expencesRegressionVisual = new LineGraphVisual(containers[3], dotSprite, 12, false, Color.red, Color.red);
        List<float> extrems = new List<float>();
        List<float> expencesRegression = new List<float>();
        if (finances.revenue.Max() >= finances.staticExpences.Max())
        {

            extrems = plot4.ShowPlot(finances.revenue, originalVisual, 14, containers[3], 0, 0, null, displayGrid: false);
        }
        else
        {


            extrems = plot4.ShowPlot(finances.staticExpences, originalVisual, -1, containers[3], 0, 0, displayGrid: false);
        }
        plot4.ShowPlot(regressionDataNormalized, revenueVisual, 14, containers[3], extrems[0], extrems[1],
            (int i) => Mathf.RoundToInt(quantityNormalizedData[i]).ToString(), (float f) => String.Format("{0:n0} $", Mathf.RoundToInt(f)), displayGrid: true);

        List<int> yearExpences = finances.GetOverYearExpences();
        if (yearExpences.Count < x.Count)
        {
            for (int i = yearExpences.Count; i < x.Count; i++)
            {
                yearExpences.Add(yearExpences.LastOrDefault());
            }
      
        }
        regression.LinearRegression(x.ToArray(), yearExpences.ToArray(), out a, out b, out rSq);
        // take last 12 elements and delete the last one in order to make regression. Adjust x accordingly
        expencesRegression = regression.Function(x.ConvertAll(p => (float)p), a, b);

        Debug.Log("-----Linear Regression-----");
        Debug.Log("A: " + a + " B: " + b + "Rsquare: " + rSq);
        plot4.ShowPlot(expencesRegression, expencesRegressionVisual, -1, containers[3], extrems[0], extrems[1], displayGrid: false);

    }



}
