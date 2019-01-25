using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int level = 1;
    public Skills skills;
    public ResourceStorage resources;
    
    private void Start()
    {

        resources.rPanel.SetPanel(resources);
    }
   
   
    public void LevelUp()
    {
        Researcher researcher = GameController.instance.researcher;
        level += 1;
        researcher.CheckIfNewResearchesAvailiable();
        researcher.UpdateResearchScreen();
        if (level == 3)
            GameController.instance.crafter.view.UnlockNewHolders(1, 1);
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            LevelUp();
    }
}
