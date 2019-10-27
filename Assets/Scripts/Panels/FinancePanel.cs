using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
public class FinancePanel : Panel, IVisualize {
    Finances finances;
    #region inspector
    [SerializeField] Plot plotBuilder;
    [SerializeField] TextMeshProUGUI salary;
    [SerializeField] TextMeshProUGUI workers;
    [SerializeField] TextMeshProUGUI heating;
    [SerializeField] TextMeshProUGUI energy;
    [SerializeField] TextMeshProUGUI tax;
    [SerializeField] TextMeshProUGUI totalExpencesMonth;
    [SerializeField] TextMeshProUGUI revenueMonth;
    [SerializeField] TextMeshProUGUI totalRevenue;
    [SerializeField] TextMeshProUGUI bestMonth;
    [SerializeField] Text margin;
    [SerializeField] BarFiller barfillerQuartal;
    [SerializeField] BarFiller barFillerMonth;
    [SerializeField] TextMeshProUGUI productionYear;
    [SerializeField] TextMeshProUGUI productionAll;
    [SerializeField] TextMeshProUGUI breakEvenPoint;
    [SerializeField] TextMeshProUGUI investments;
    
    #endregion
    public override void Hide()
    {
        
    }
    private void Start()
    {
        
        Invoke("Repeat", 0.1f);
        
    }
    private void OnEnable()
    {
        EventManager.StartListening(TurnController.OnTurnEvent, SetPanel);
    }
    private void OnDisable()
    {
        EventManager.StopListening(TurnController.OnTurnEvent, SetPanel);
    }
    private void Repeat()
    {
        RefreshPanel(finances);
    }
    public void RefreshPanel(Finances finances)
    {
        Debug.Log(finances);
        this.finances = finances;
        SetPanel();
    }
    public override void SetPanel()
    {
        finances = GameController.instance.player.finances;
        salary.text = (GameController.instance.roomOverseer.GetAllSceneObjects().Count>0)? (finances.expences.GetMonthSallary/(finances.expences.workersOnObject*GameController.instance.roomOverseer.GetAllSceneObjects().Count)).ToString() + " $" : 0.ToString() +" $";
        workers.text = (finances.expences.workersOnObject * GameController.instance.roomOverseer.GetAllSceneObjects().Count).ToString();
        energy.text = finances.expences.GetEnergyCost.ToString()+" $";
        heating.text = finances.expences.GetHeatingCost.ToString() + " $";
        tax.text = finances.expences.GetTaxes.ToString() + " $";
        totalExpencesMonth.text = finances.GetTotalMonthExpences().ToString() + " $";
        revenueMonth.text = finances.revenue.LastOrDefault().ToString() + " $"; 
        totalRevenue.text = finances.revenue.Sum().ToString() + " $";
        if (finances.revenue.Count >= 1 && finances.dates.Count>=1)
        {
            int maxrevenue;
            GameDate bestDate = FindBestMonth(out maxrevenue);
            bestMonth.text = bestDate.month.ToString() + ",\n" + bestDate.year.ToString();
        }
        else bestMonth.text = "None";
        breakEvenPoint.text = finances.GetBreakEvenPointMoney() + " $";
        
        investments.text = (finances.activeExpences.Count>1)? finances.activeExpences.Sum().ToString()+" $":"0 $";
        productionAll.text = (finances.itemsProduced.Count>1)?finances.itemsProduced.Sum().ToString()+" units":" 0 units";
        productionYear.text = finances.GetYearProduction() + " units";
        margin.text = finances.GetMarginSafetyPercentage() + " %\n"+finances.GetMarginSafetyMoney()+" $";
        DrawBar();
    
    }

    private GameDate FindBestMonth(out int maxRevenueIndex)
    {
        List<int> differences = new List<int>();

        if (finances.revenue.Count <= 1 || finances.dates.Count<=1)
        {
            maxRevenueIndex = 0;
            return GameDate.CreateDate(Month.December, 2000, 1);
        }
        for(int i=0; i<finances.revenue.Count-1;i++)
        {
            differences.Add(finances.revenue[i] - finances.staticExpences[i]);
        }
         maxRevenueIndex = differences.IndexOf(differences.Max());
        return finances.dates[maxRevenueIndex];
        
    }
   
    public void DrawBar()
    {
        DrawMonthBar();
        DrawQuartalBar();
    }
    private void DrawQuartalBar()
    {
        if (!gameObject.activeInHierarchy) return;
        // draw revenue/ expences  for the last 3 months
        StartCoroutine(barfillerQuartal.IncreaseOverTime(barfillerQuartal.healingBar,finances.GetQuarterRevenue(), Mathf.Max(10000, finances.revenue.Max() * 2), isPercentage: false));
        //  barfillerQuartal.SetValueToBarScalar(finances.GetQuarterRevenue(), barfillerQuartal.healingBar, Mathf.Max(10000, finances.revenue.Max()*2));
        StartCoroutine(barfillerQuartal.IncreaseOverTime(barfillerQuartal.toxicityBar, finances.GetQuarterExpences(), Mathf.Max(10000, finances.revenue.Max() * 2), isPercentage: false));
       // barfillerQuartal.SetValueToBarScalar(finances.GetQuarterExpences(), barfillerQuartal.toxicityBar, Mathf.Max(10000, finances.revenue.Max()*2));

    }
    private void DrawMonthBar()
    {
        int maxRevenue = 0;
        FindBestMonth(out maxRevenue);
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(barFillerMonth.IncreaseOverTime(barFillerMonth.healingBar, finances.revenue[maxRevenue], Mathf.Max(10000, finances.revenue.Max()), isPercentage: false));
        //barFillerMonth.SetValueToBarScalar(finances.revenue[maxRevenue], barFillerMonth.healingBar, Mathf.Max(10000,finances.revenue.Max()));
        //barFillerMonth.SetValueToBarScalar(finances.staticExpences[maxRevenue], barFillerMonth.toxicityBar, Mathf.Max(10000, finances.revenue.Max()));
        StartCoroutine(barFillerMonth.IncreaseOverTime(barFillerMonth.toxicityBar, finances.staticExpences[maxRevenue], Mathf.Max(10000, finances.revenue.Max()), isPercentage: false));
    }
   
}
