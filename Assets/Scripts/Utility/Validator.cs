using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validator  {

	public bool ValidateRecipeName(string name)
    {
        if (!Crafter.instance.inventory.CheckIfWarehouseContains(name))
            return true;
        return false;
    }
}
