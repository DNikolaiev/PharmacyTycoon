using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Inventory: MonoBehaviour {
    public int capacity=10; // max number of recipes in inventory
    public List<Recipe> recipes;
    [SerializeField] private Dictionary<string, int> warehouse = new Dictionary<string, int>(); //value = amount of medicine in warehouse //key = name of recipe
    public void Set(Recipe recipe, int value) // add element to dictionary(warehouse)
    {
        string key = recipe.description.Name;
      
        if (CheckIfWarehouseContains(key) )
        {
            Debug.Log("Contains");

                warehouse[key] += value;
        }
        else if (!CheckIfWarehouseContains(key) && warehouse.Count < capacity )
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
        Recipe[] toDelete = recipes.Where(x => x.description.Name == key).ToArray();
        recipes.Remove(toDelete[0]);
       
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            foreach(Recipe r in recipes)
            {
                Debug.Log(r.description.Name + " " + warehouse[r.description.Name]);
            }
        }
    }
}
