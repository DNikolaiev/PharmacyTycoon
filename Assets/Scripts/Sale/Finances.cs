using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
[System.Serializable]
public class Finances {

    public Expences expences;
    public List<int> investments;
    public List<int> revenue;
    public List<int> staticExpences;
    public List<int> activeExpences;
    public List<int> itemsProduced;
    public List<GameDate> dates;
    public int bankruptState;
    public int TotalRevenue
    {
        get { return revenue.Sum(); }
    }
  
    public void Initialize()
    {
        revenue.Add(0);
        staticExpences.Add(0);
        itemsProduced.Add(0);
        activeExpences.Add(0);
    }
    
    public void GenerateReport()
    {
        revenue.Add(0);
        staticExpences.Add(0);
        activeExpences.Add(0);
        itemsProduced.Add(0);
        dates.Add(GameController.instance.time.date);
        investments.Add(GameController.instance.player.resources.money);
        int productivityEfficiency = Mathf.RoundToInt(GetProductivity()*100);
        
        PlayGameScript.AddToLeaderboard(GPGSIds.leaderboard_productivity_factor, productivityEfficiency);
        PlayGameScript.AddToLeaderboard(GPGSIds.leaderboard_healed_humans, GetHealedAmount());
        
        if (productivityEfficiency >= 75) { PlayGameScript.UnlockAchievement(GPGSIds.achievement_finally_some_good_management); }

    }
    private int GetHealedAmount()
    {
        int sum = 0;
        foreach(Area a in World.instance.areas)
        {
            sum += a.cured;
        }
        return sum;
    }
    public int GetTotalMonthExpences()
    {
        return (expences.GetBills+expences.GetMonthSallary);
    }
    public List<int> GetQuantityData()
    {
        List<int> quantityData = new List<int>();
        List<int> sortedRevenue = new List<int>();
        sortedRevenue.AddRange(revenue);
        sortedRevenue.Sort();
        foreach (int n in sortedRevenue)
        {
            quantityData.Add(n / GetAverageProductPrice());
        }
        return quantityData;
    }
    public int GetLabor()
    {
        return 296 * (expences.workersOnObject * GameController.instance.roomOverseer.GetAllSceneObjects().Count * 8);
    }
    public float GetProductivity()
    {
        float alpha = 0.7f;
        float beta = 0.3f;
        int K = SumLastElements(activeExpences, 12);
        int L = GetLabor();
        if (K == 0 || L == 0) return 0;
        Debug.Log("Q = "+itemsProduced.Sum() + "PRODUCITVITY: "+(Mathf.Pow(K, alpha) * Mathf.Pow(L, beta)));
        return ((itemsProduced.Sum()*10) / (Mathf.Pow(K, alpha) * Mathf.Pow(L, beta)));
    }
    public float GetMarginSafetyPercentage()
    {
       
        return ((float)Math.Round(((float)SumLastElements(revenue,12)/12 - GetBreakEvenPointMoney()) / ((float)SumLastElements(revenue,12)/12) * 100)/100 * 100);
    }
    public float GetMarginSafetyMoney()
    {
        return (SumLastElements(revenue, 12) / 12 - GetBreakEvenPointMoney());
    }
  
    public int GetBreakEvenPointMoney()
    {
        int revenueNotNull = (SumLastElements(revenue, 12) != 0) ? (SumLastElements(revenue, 12) / 12) : 1;
        if (revenueNotNull == 0) revenueNotNull++;
        return (((int)SumLastElements(revenue, 12) / 12) * (int)(SumLastElements(staticExpences, 12) / 12)) / revenueNotNull;
    }
    public int GetBreakEvenPointUnits()
    {
        return (((int)SumLastElements(staticExpences,12)/12) / (GetAverageProductPrice()));
    }
    public void AddToRevenue(int amount)
    {
      var lastRevenue =  revenue.LastOrDefault();
        lastRevenue += amount;
        revenue[revenue.Count - 1] = lastRevenue;
    }
    public void AddToProducedItems(int amount)
    {
        var lastRevenue = itemsProduced.LastOrDefault();
        lastRevenue += amount;
        itemsProduced[itemsProduced.Count-1] = lastRevenue;
    }
    public void AddToStaticExpences(int amount)
    {
        var lastExpences = staticExpences.LastOrDefault();
        lastExpences += amount;
         staticExpences[staticExpences.Count - 1] = lastExpences;
    }
    public void AddToActiveExpences(int amount)
    {
        var lastExpences = activeExpences.LastOrDefault();
        lastExpences += amount;
        activeExpences[activeExpences.Count - 1] = lastExpences;
    }
    public int GetAverageProductPrice()
    {
        if (GameController.instance.player.inventory.recipes.Count == 0) return 1;
        int sum = 0;
        foreach(Recipe recipe in GameController.instance.player.inventory.recipes)
        {
            sum += recipe.price.Value;
        }
        sum /= GameController.instance.player.inventory.recipes.Count;
        return sum;
    }
    public int GetQuarterRevenue()
    {
        return (SumLastElements(revenue, 4)-revenue.LastOrDefault());
    }
    public int GetQuarterExpences()
    {
        return (SumLastElements(staticExpences,4)-staticExpences.LastOrDefault());
    }
    public int GetYearProduction()
    {
        return SumLastElements(itemsProduced, 12);
    }
    public List<int> GetOverYearExpences()
    {
        List<int> resList = new List<int>();
        int n = 12;
        for (int i=staticExpences.Count-1;i>0;i--)
        {
            if (n == 0 || i < 0) break;
            resList.Add(staticExpences[i]);
            n--;
        }
        resList.Reverse();
        return resList;
    }
    private int SumLastElements(List<int> list, int n)
    {
        if (list.Count == 0) return 0;
        int result = 0;
        for(int i=list.Count-1;i>=0;i--)
        {
            if (n == 0) break;
            result += list[i];
            n--;
        }
        return result;
    }
    
}
