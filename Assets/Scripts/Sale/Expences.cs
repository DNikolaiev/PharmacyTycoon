using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Expences  {

    public int salaryPerWorker;
    public int workersOnObject;
    public int energyCost;
    public int heatingCost;
    
    public int taxPerBuilding;

    public int GetMonthSallary
    {
        get { return GameController.instance.roomOverseer.GetAllSceneObjects().Count * salaryPerWorker * workersOnObject; }
    }
    public void ChargeSallary()
    {
        GameController.instance.player.resources.ChangeBalance(-GetMonthSallary);
      
    }
    public int GetBills
    {
        get { return GameController.instance.roomOverseer.GetAllSceneObjects().Count*(energyCost + heatingCost); }
    }
    public int GetHeatingCost
    {
        get { return GameController.instance.roomOverseer.GetAllSceneObjects().Count * ( heatingCost); }
    }
    public int GetEnergyCost
    {
        get { return GameController.instance.roomOverseer.GetAllSceneObjects().Count * (energyCost); }
    }
    public void ChargeBills()
    {
        GameController.instance.player.resources.ChangeBalance(-(GetBills));
        
    }
    public int GetTaxes
    {
       get { return GameController.instance.roomOverseer.rooms.Count * taxPerBuilding; }
    }
    public void ChargeTaxes()
    {
        GameController.instance.player.resources.ChangeBalance(-GetTaxes);
      //  GameController.instance.player.finances.AddToStaticExpences(GetTaxes);
       
    }
    public void ChargeMonthlyExpences()
    {
        var finances = GameController.instance.player.finances;
        ChargeBills();
        ChargeSallary();
        finances.AddToStaticExpences(GetMonthSallary + GetBills);
        finances.GenerateReport();
    }
}
