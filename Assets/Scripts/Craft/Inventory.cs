using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
[CreateAssetMenu()]
public class Inventory: ScriptableObject {
    public int capacity; // max number of recipes in inventory
    public List<Recipe> recipes;
    [SerializeField] private Dictionary<string, int> warehouse = new Dictionary<string, int>(); //value = amount of medicine in warehouse //key = name of recipe

    public void Clear()
    {
        warehouse.Clear();
        recipes.Clear();
       
    }
    public void Set(Recipe recipe, int value, bool canBeOversized=false) // add element to dictionary(warehouse)
    {
        
        SaveController.OnSaveEvent -= OnSave;
        SaveController.OnSaveEvent += OnSave;
        if (recipe == null) return;
           string key = recipe.description.Name;
        if (CheckIfWarehouseContains(key) )
        {
            Debug.Log("Contains");

                warehouse[key] += value;
        }
        else if ((!CheckIfWarehouseContains(key) && warehouse.Count < capacity ) || canBeOversized )
        {
            recipes.Add(recipe);
            warehouse.Add(key,value);
            Debug.Log("Set"+" " +key+" "+ warehouse[key]);
            
        }
        else if (value < 0 || warehouse.Count == capacity)
        {
            Debug.Log("FULL");
            //run notifictation here "You have too many recipes, go and delete one"
            return;
        }

    }
    public bool CheckIfWarehouseContains(string key)
    {
        if (warehouse.ContainsKey(key))
        {
            
            return true;
        }
        return false;
    }
    public bool CheckIfWasCreated(string key)
    {
        return recipes.Where(x => x.description.Name == key).ToList().Count > 0;
    }
    public int GetQuantity(string key)
    {
        int result = 0;

        if (warehouse.ContainsKey(key))
        {
            result = warehouse[key];
        }

        return result;
    }
    public int GetNumberOfElements()
    {
        return warehouse.Count;
    }
   public void Delete(string key)
    {
        
        warehouse.Remove(key);
        Recipe toDelete = recipes.Where(x => x.description.Name == key).FirstOrDefault();
        Remove(toDelete.description.Name, GetQuantity(toDelete.description.Name));
        //recipes.Remove(toDelete[0]); <- delete permanently
        toDelete.isDeleted = true;
        
       
    }
    public bool Remove(string key, int value)
    {
        SaveController.OnSaveEvent -= OnSave;
        SaveController.OnSaveEvent += OnSave;
        if (warehouse.ContainsKey(key))
        {
            if (warehouse[key] - value >= 0)
            {
                warehouse[key] -= value;
                if (value == 0) return false;
                
                return true;
            }
         
        }
        return false;
    }
    public void RemoveAllMedicine(string key)
    {
        if (warehouse.ContainsKey(key))
        {
            warehouse[key] = 0; 
        }
    }
   private void OnSave()
   {
        foreach(Recipe recipe in recipes)
        {
            SaveController.OnSaveEvent += recipe.OnSave;
        }
   }
   public void OnLoad()
   {
        foreach (Recipe recipe in recipes)
        {
            SaveController.OnLoadEvent += recipe.OnLoad;
        }
    }
}
