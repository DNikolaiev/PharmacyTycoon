using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Validator  {

	public bool ValidateRecipeName(string name, Inventory inventory)
    {
        if (!inventory.CheckIfWarehouseContains(name) && !inventory.CheckIfWasCreated(name))
            return true;
        return false;
    }
}
