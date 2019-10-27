using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Constructor : MonoBehaviour {

    
    public Constructible[] objectsToConstruct;
    public Cell selectedCell;
    public bool isShopEnabled=false;
    public bool inConfirmation = false;
    public bool isActive = false;
    [SerializeField] Image shopPanel;
    [SerializeField] Builder builder;
    [SerializeField] List<InfoPanel> objectPanels;
    [SerializeField] Tutorial tutorial;
    [SerializeField] Color cellHighlightedColor;
    private Button constructBtn;
    private Button cancelBtn;
    private ButtonController buttons;
    private ConfirmationWindow confirm;
    private ColorInterpolator interpol;
    public List<GameObject> roomCells;
    
    private void Awake()
    {
        interpol = GetComponent<ColorInterpolator>();
       
    }
    private void Start()
    {
        buttons = GameController.instance.buttons;
        confirm = buttons.confirm;
        constructBtn = buttons.constructor;
        cancelBtn = buttons.cancel;
        shopPanel.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        interpol.originalColor = confirm.confirmText.color;
       
    }
    public void RefreshShop()
    {
        foreach(InfoPanel panel in objectPanels)
        {
            panel.SetPanel();
        }
       
        
    }
    private void ShowCells()
    {
        
        roomCells.RemoveAll(item => item == null);
        foreach (Room room in GameController.instance.roomOverseer.rooms)
        {
            foreach (Cell cell in room.info.cells)
            {
                cell.gameObject.SetActive(true);
                cell.UpdateColor();
                
            }
        }
        
        if (!tutorial.isTutorialCompleted)
        {
            StartCoroutine(GameController.instance.cam.FocusCamera(new Vector2(0, 0))); 
            if (roomCells.Count > 0)
            {
                foreach (GameObject room in roomCells)
                {

                    room.SetActive(false);

                }

            }
            //??
            Cell cell = GameController.instance.roomOverseer.rooms[0].info.cells.Where(x => !x.isOccupied).ToList().FirstOrDefault();
            StartCoroutine(GetComponent<Scaler>().Scale(cell.gameObject));
            cell.GetComponent<MeshRenderer>().material.color = cellHighlightedColor;
        }
        else
        {
            if (roomCells.Count > 0)
            {
                foreach (GameObject room in roomCells)
                {
                   
                    room.SetActive(true);
                }
            }
            
        }
        Camera.main.cullingMask = -1; // -1 means "Everything"}
       
    }
    
    private void HideCells()
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Cells"));
    }
    public void ToggleShop(bool state)
    {
        if (GameController.instance.buttons.tPanel.gameObject.activeInHierarchy && state) return;
        GameController.instance.IsGameSceneEnabled =!state;
        
        isShopEnabled = state;
        shopPanel.gameObject.SetActive(state);
        RefreshShop();
        if (state == true)
        {
            buttons.SwitchButtons(constructBtn, cancelBtn);
            GameController.instance.time.Pause();
        }
        if (!tutorial.isTutorialCompleted)
        {
            cancelBtn.gameObject.SetActive(false);
            GameController.instance.IsGameSceneEnabled = false;
            // ??
            StopAllCoroutines();
            Cell scaledCell = GameController.instance.roomOverseer.rooms[0].info.cells.Where(x => !x.isOccupied).ToList().FirstOrDefault();

            scaledCell.transform.localScale = GameController.instance.roomOverseer.rooms[0].info.cells[3].transform.localScale;



        }
       
        if (!tutorial.isTutorialCompleted) tutorial.ContinueTutorial();

    }
    public void Construct(int id)
    {
        if (selectedCell.transform.childCount >= 1)
            return;
        if (GameController.instance.player.resources.money < objectsToConstruct[id].description.buyPrice)
        {
            
            StartCoroutine(interpol.InOut(confirm.confirmText, Color.red));
            return;
        }
        Build(id);
        GameController.instance.player.resources.ChangeBalance(-objectsToConstruct[id].description.buyPrice);
        if (!GameController.instance.generalTutorial.isTutorialCompleted) { GameController.instance.generalTutorial.isBlocked = false; GameController.instance.generalTutorial.ContinueTutorial(); GameController.instance.generalTutorial.ContinueTutorial(); }
            Abort();
        
        ConstructOFF();
        
    }
    public void Build(int id, bool playEffects = true)
    {
        Constructible selectedObject = builder.Build(id, objectsToConstruct, selectedCell, playEffects);
        if (selectedCell.CompareTag("RoomCell"))
            roomCells.Remove(selectedCell.gameObject);
        selectedCell.AddObject(selectedObject);
    }
    public void SelectObject(int id)
    {
        
        inConfirmation = true;
        confirm.SetPanel("Purchase " + objectsToConstruct[id].description.Name + " for " + objectsToConstruct[id].description.buyPrice+ "$ ?");
        confirm.Activate(true);
        confirm.ok.onClick.AddListener(delegate { Construct(id); });
        confirm.cancel.onClick.AddListener(Abort);
        cancelBtn.gameObject.SetActive(false);

    }
    private void Abort()
    {
        confirm.Activate(false);
        inConfirmation = false;
        confirm.Hide();
        cancelBtn.gameObject.SetActive(true);     
    }
    public void ConstructON()
    {
        isActive = true;
        ShowCells();
        buttons.HideAllButtons();
        buttons.SwitchButtons(constructBtn, cancelBtn);
        if (!tutorial.isTutorialCompleted) tutorial.StartTutorial();
        if (!GameController.instance.generalTutorial.isTutorialCompleted) GameController.instance.buttons.HideCancel();

    }
    public void ConstructOFF()
    {

        isActive = false;
        GameController.instance.time.UnPause();
        GameController.instance.IsGameSceneEnabled = true;
        HideCells();
        ToggleShop(false);

        buttons.HideAllButtons();
        buttons.ShowAllButtons();
        

    }
}
