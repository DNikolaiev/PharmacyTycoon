using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Constructor : MonoBehaviour {

    public Image shopPanel;
    public Constructible[] objectsToConstruct;
    public Cell selectedCell;
    public Builder builder;
    
    public bool isShopEnabled=false;
    public bool inConfirmation = false;
    public bool isActive = false;

    private Button constructBtn;
    private Button cancelBtn;
    private ButtonController buttons;
    private ConfirmationWindow confirm;
    private ColorInterpolator interpol;
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
    }
    public void RefreshShop()
    {
        //здесь обработать логику открытия постепенного объектов
       
        
    }
    private void ShowCells()
    {

        foreach (Room room in GameController.instance.roomOverseer.rooms)
        {
            foreach (Cell cell in room.info.cells)
            {
                cell.UpdateColor();
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
        isShopEnabled = state;
        shopPanel.gameObject.SetActive(state);
        RefreshShop();
        if (state == true)
            buttons.SwitchButtons(constructBtn, cancelBtn);
        else buttons.SwitchButtons(cancelBtn, constructBtn);
        
    }
    public void Construct(int id)
    {
        if (selectedCell.transform.childCount >= 1)
            return;
        if (GameController.instance.player.resources.money < objectsToConstruct[id].description.buyPrice)
        {
            StartCoroutine(interpol.PingPong(confirm.confirmText, Color.red));
            return;
        }
        Constructible selectedObject=builder.Build(id, objectsToConstruct, selectedCell);
        selectedCell.AddObject(selectedObject);
        GameController.instance.player.resources.ChangeBalance(-objectsToConstruct[id].description.buyPrice);
       
        Abort();
        ConstructOFF();
        
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
        Camera.main.GetComponent<CameraController>().isActive = false;
   
    }
    public void ConstructOFF()
    {

        isActive = false;
        Camera.main.GetComponent<CameraController>().isActive = true;
        HideCells();
        ToggleShop(false);

        buttons.HideAllButtons();
        buttons.ShowAllButtons();
        

    }
}
