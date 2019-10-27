using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct Invoice {

    public int transferCost;
    public int price;
    public int quantity;
    public float bonus;
    
    public Invoice(int transferCost, int price, int quantity, float bonus)
    {
        this.transferCost = transferCost;
        this.price = price;
        quantity = Mathf.Clamp(quantity, 0, Int32.MaxValue);
        this.quantity = quantity;
        this.bonus = bonus;
        bonus = Mathf.Clamp(bonus, 1, 3);
    }
    public int Summary
    {
        get
        {
            return (int)((price * quantity * bonus) - transferCost);
        }
    }
	
}
