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
    private void Start()
    {

        resources.rPanel.SetPanel(resources);
    }
   
    public void ChangeBalance(int amount)
    {
        money += amount;
        resources.rPanel.SetPanel(resources);
    }
    public void LevelUp()
    {
        level += 1;
        Researcher.instance.CheckIfNewResearchesAvailiable();
        Researcher.instance.UpdateResearchScreen();
        if (level == 3)
            Crafter.instance.UnlockNewHolders(1, 1);
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            LevelUp();
    }
}
