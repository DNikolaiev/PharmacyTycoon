using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
public class Progress : MonoBehaviour
{
    public SceneProgress scene;
    private bool restarting;
    private void PreLoad()
    {
        SaveController saveController = new SaveController();
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, saveController.directoryName))) {// NewGame();
            NewGame(); ; return; }

        Load();
        if (!GameController.instance.generalTutorial.isTutorialCompleted)
        {
            Restart(); return;
        }
        scene.Load();
        Invoke("EnableAutoSave", 0.5f);

        GameController.instance.player.Initialize();

        
    }
    private void EnableAutoSave()
    {
        StopCoroutine(AutoSave());
        StartCoroutine(AutoSave());
    }
    private void OnDisable()
    {
        if (!restarting)
        {
            try
            {
                Save();
                scene.Save();
               
            }
            catch (System.Exception e)
            {
                Debug.Log("error");
            }
        }
        restarting = false;
    }
    private void OnApplicationQuit()
    {
        OnDisable();
    }
    private IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForSeconds(2);
            Debug.Log("SAVING......");
            Save();
            scene.Save();
        }
    }
    private void OnEnable()
    {
       
        Invoke("PreLoad", 0.2f);
        
        
    }
    public void NewGame()
    {
        Debug.Log("NEW GAME");
        DeleteSaves();
        GameController.instance.player.canPlay = true;
        foreach (Talent t in GameController.instance.talentTree.talents)
        {
            t.isUnlocked = false;
            t.canBeUnlocked = false;
            
        }
        foreach(ResearchHolder h in GameController.instance.researcher.view.holders)
        {
            h.ChangeHolderPicture(false);
        }
        GameController.instance.talentTree.talents[0].canBeUnlocked = true;
        GameController.instance.researcher.view.holders[0].ChangeHolderPicture(true);
        foreach (Quest q in FindObjectOfType<QuestGiver>().allQuests)
        {
            q.isActive = false;
        }
        GameController.instance.player.activeQuests.Clear();
        GameController.instance.player.inventory.Clear();
        foreach (Area a in World.instance.areas)
        {
            a.ResetAll();
        }
        GameController.instance.player.Name = "Player";
        GameController.instance.player.inventory.capacity = 3;
        GameController.instance.tutorialSettings.EnableTutorial();
        Transform[] childObjects = GameController.instance.roomOverseer.rooms[0].GetComponentsInChildren<Transform>();
        
        List<Cell> cells = GameController.instance.roomOverseer.rooms[0].GetCells(childObjects).Where(x => x.CompareTag("Cell")).ToList();
        GameController.instance.constructor.selectedCell = cells[1];
        GameController.instance.constructor.Build(1,false);
        GameController.instance.constructor.selectedCell = cells[cells.Count - 2];
        GameController.instance.constructor.Build(2,false); // 1 - research center, 2 - greengarden
        StartCoroutine(GameController.instance.time.TimeStart());
        Invoke("EnableAutoSave", 0.5f);

    }
    private void DeleteSaves()
    {
        StopAllCoroutines();
        SaveController saveController = new SaveController();
        
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, saveController.directoryName)))
            Directory.Delete(Path.Combine(Application.persistentDataPath, saveController.directoryName), true);
        
    }
    public void Restart()
    {
        Debug.Log("RESTARTIIIIIIIIIIIIING");
        SaveController saveController = new SaveController();
        restarting = true;
        DeleteSaves();
        PreLoad();
        
        Application.LoadLevel("SampleScene");
    }
    public void UnlockAllContent()
    {
        GameController.instance.player.canPlay = true;
        GameController.instance.generalTutorial.isTutorialCompleted = true;
        foreach (Talent t in GameController.instance.talentTree.talents)
        {
            t.isUnlocked = true;
            t.canBeUnlocked = true;
        }
        GameController.instance.player.resources.ChangeBalance(100000);
        GameController.instance.player.resources.AddResearchPoints(100);
        GameController.instance.player.resources.AddCoins(300);
        GameController.instance.player.experience = 1000000;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            GameController.instance.player.LevelUp();
        }
        
    }
    public void Load()
    {

        SaveController saveController = new SaveController();


        if (GameController.instance != null)
        {

            GameController.instance.talentTree.savedData = (SavedUnlockmentsData)saveController.LoadData("talentsData.json", GameController.instance.talentTree.savedData);

        }
       
        // Player block 
        GameController.instance.player.playerData = (PlayerData)saveController.LoadData("player.json", GameController.instance.player.playerData);
        // load inventory 
        GameController.instance.player.inventory = (Inventory)saveController.LoadData("inventory.json", GameController.instance.player.inventory);
        GameController.instance.player.inventory.OnLoad();

        GameController.instance.researcher.saveData = (SaveResearchData)saveController.LoadData("research.json", GameController.instance.researcher.saveData);
        GameController.instance.buttons.engineeringPanel.saveData = (SaveResearchData)saveController.LoadData("enhancement.json", GameController.instance.buttons.engineeringPanel.saveData);
        GameController.instance.buttons.engineeringPanel.OnLoad();
        World.instance.saveData = (WorldSaveData)saveController.LoadData("world.json", World.instance.saveData);
        GameController.instance.time.date = (GameDate)saveController.LoadData("date.json", GameController.instance.time.date);
        GameController.instance.tutorialSettings.LoadTutorialState((List<bool>)saveController.LoadData("tutorial.json", GameController.instance.tutorialSettings.GetTutorialsState()));
        GameController.instance.buttons.market.SetTimers((List<int>)saveController.LoadData("timers.json", GameController.instance.buttons.market.GetTimers()));
        

        saveController.FireLoadEvent();

        World.instance.GenerateEpidemic();
        
    }
    public static void Save()
    {

        SaveController saveController = new SaveController();
        saveController.FireSaveEvent();
        saveController.SaveData("talentsData.json", GameController.instance.talentTree.savedData);
        saveController.SaveData("player.json", GameController.instance.player.playerData);
        saveController.SaveData("inventory.json", GameController.instance.player.inventory);
        saveController.SaveData("enhancement.json", GameController.instance.buttons.engineeringPanel.saveData);
        saveController.SaveData("research.json", GameController.instance.researcher.saveData);
        saveController.SaveData("world.json", World.instance.saveData);
        saveController.SaveData("date.json", GameController.instance.time.date);
        saveController.SaveData("tutorial.json", GameController.instance.tutorialSettings.GetTutorialsState());
        saveController.SaveData("timers.json", GameController.instance.buttons.market.GetTimers());
    }

   

}


