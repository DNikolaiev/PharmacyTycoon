using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory:MonoBehaviour  {
    //value = amount of medicine in warehouse
   [SerializeField] private Dictionary<Recipe, int> warehouse;
    public void Set(Recipe key, int value)
    {
        if (Mathf.Abs(value) > warehouse[key] && value<0)
            return;
        if (warehouse.ContainsKey(key))
        {
                warehouse[key] += value;
        }
        else
        {
            warehouse.Add(key,value);
        }
    }
    public bool CheckIfWarehouseContains(Recipe key)
    {
        if (warehouse.ContainsKey(key))
        {
            return true;
        }
        return false;
    }
    public int Get(Recipe key)
    {
        int result = 0;

        if (warehouse.ContainsKey(key))
        {
            result = warehouse[key];
        }

        return result;
    }

}
