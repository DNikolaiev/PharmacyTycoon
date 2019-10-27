
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class GameController : MonoBehaviour {
    private bool isGameSceneEnabled;
    public bool IsGameSceneEnabled
    {
        get
        {
            
            return isGameSceneEnabled;
        }
        set
        {
            isGameSceneEnabled = value;
            if (value == true)
            {
                EventManager.TriggerEvent("OnGameEnable");
            }
            else
            {
                EventManager.TriggerEvent("OnGameDisable");
            }
            
        }
    }
    public TalentTree talentTree;
    public Market market;
    public Player player;
    public Crafter crafter;
    public Constructor constructor;
    public Researcher researcher;
    public ButtonController buttons;
    public CameraController cam;
    public AudioController audio;
    public RoomManager roomOverseer;
    public TurnController time;
    public bool isInitialized;
    public Tutorial generalTutorial;
    public Tutorial gameOverHelp;
    public TutorialSettings tutorialSettings;
    [SerializeField] GameObject gameOverPanel;
    public GameOverConditions gameOver;
    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            isInitialized = true;
        }
        else Destroy(instance);
    }
    private void OnEnable()
    {
        IsGameSceneEnabled = true;
        SaveController.OnLoadEvent += talentTree.OnLoad;
        SaveController.OnSaveEvent += talentTree.OnSave;
        EventManager.StartListening(TurnController.OnTurnEvent, CheckGameOver);
    }
    private void CheckGameOver()
    {

        if (gameOver.CheckCondition())
        {
            time.Pause();
            if(player.resources.SecondChances > 0)
            {
                player.ApplySecondChance();
                var messageBox = buttons.messageBox;
                messageBox.Show("Second Chance used. You are saved!", Resources.Load<Sprite>("Icons/2ndChance")); return;
            }
            else
            {
                time.Pause();
                buttons.market.SetPanel();
                gameOverHelp.StartTutorial(); 
            }
            
        }

    }
    public void EnableGameOver(bool state)
    {
        player.canPlay = !state;
        if (state==true)
        {
            time.Pause();
            cam.ToggleCameraWithDelay(0);
            buttons.HideAllButtons();
        }
        else { time.UnPause(); buttons.ShowAllButtons(); }
        isGameSceneEnabled = !state;
       
        gameOverPanel.SetActive(state);
    }
    private void Start()
    {
        IsGameSceneEnabled = true;
        gameOverPanel.SetActive(false);
        gameOver = new GameOverConditions(player, World.instance.areas);
        Invoke("StartTutorial", 0.6f);
        
    }
    public void StartTutorial()
    {
        buttons.market.CheckNewOffers();
        buttons.market.OnLoad();
        if (!generalTutorial.isTutorialCompleted) {  generalTutorial.StartTutorial(); }
        
    }
    
    public void _StartCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public GameObject Instantiate(GameObject obj, Transform transform)
    {
        return Instantiate(obj, transform);
    }
}
