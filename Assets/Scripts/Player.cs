using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int level = 1;
    public int money;
    public ResourceStorage resources;
    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
    }
    
    public void ChangeBalance(int amount)
    {
        money += amount;
    }
    public void LevelUp()
    {
        level += 1;
            Researcher.instance.CheckIfNewResearchesAvailiable();
        Researcher.instance.UpdateResearchScreen();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            LevelUp();
    }
}
