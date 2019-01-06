using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RecipeHolder: Holder, IPointerDownHandler, IDescription, IPointerEnterHandler {
    public Recipe recipe;
    public Text quantitytxt;
    [SerializeField] Button action;
    [SerializeField] ConfirmationWindow confirm;
    [SerializeField] DragHandler dragger;
    
    public  void SetDescription()
    {
        descriptionPanel.SetPanel(recipe, this);
    }

    private void Start()
    {
        confirm = ButtonController.instance.confirm;
        if (isUnlocked)
            lockedSprite.SetActive(false);
        if (dragger != null)
            InitializeDrag();
        descriptionPanel = Crafter.instance.craftDescriptionPanel.GetComponent<DescriptionPanel>();
        if (action != null)
            action.onClick.AddListener(SetDescription);
        if (recipe != null)
            SetPanel();
    }
    public override void SetPanel()
    {
        if (Crafter.instance.inventory.CheckIfWarehouseContains(recipe.description.Name))
            quantitytxt.text = "x" + Crafter.instance.inventory.GetQuantity(recipe.description.Name).ToString();
        else return;
        Image[] images = GetComponentsInChildren<Image>();
        for (int i=1; i<images.Length; i++)
        {
            if (recipe.PTalents == null) return;
                if (i > recipe.PTalents.Count)
                {
                    images[i].sprite = defaultSprite; continue;
                }
                else
                    images[i].sprite = recipe.PTalents[i - 1].description.sprite;
        }
    }
    private void InitializeDrag()
    {
        dragger.SetCanvas(Crafter.instance.craftPanel.transform.parent.GetComponent<Canvas>());
        dragger.onBeginMethod += OnBeginDrag;
        dragger.onEndMethod += OnEndDrag;
    }
    private void OnBeginDrag()
    {
        if (dragger.dragItem != null) return;
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity, action.transform.parent.parent.parent.parent);
        copy.GetComponent<Image>().raycastTarget = false;

        dragger.SetDragItem(copy);
    }
    private void OnEndDrag()
    {
      
        if (!Crafter.instance.holderSelected.isUnlocked || (Crafter.instance.holderSelected == null && dragger.dragItem != null))
        {
            dragger.ResetDragItem();
            return;
        }


        if (Crafter.instance.holderSelected != null)
        {
            Crafter.instance.controller.OnAddRecipe(this);
            dragger.ResetDragItem();
        }

        else dragger.ResetDragItem();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragger != null)
            return;
    }


    public  void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        SetDescription();
        
    }
    public void OnDelete()
    {
        confirm.SetPanel("Delete " + recipe.description.Name + " ?");
        confirm.Activate(true);
        confirm.ok.onClick.AddListener(delegate { Delete(); });
        confirm.cancel.onClick.AddListener(Abort);
        ButtonController.instance.cancel.gameObject.SetActive(false);
    }
    private void Abort()
    {
        confirm.Activate(false);
        confirm.Hide();
        ButtonController.instance.cancel.gameObject.SetActive(true);
    }
    private void Delete()
    {
        var crafter = Crafter.instance;
        crafter.isPrescripted = false;
        crafter.recipeSelected = null;
        crafter.inventory.Delete(recipe.description.Name);
        crafter.PopulateRecipeList();
        crafter.capacity.text = crafter.inventory.GetNumberOfElements().ToString() + " / " + crafter.inventory.capacity.ToString(); // refresh capacity text
        Abort();
        Destroy(gameObject);
    }
    private bool IsMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.GetComponent<CraftHolder>() != null)
        {
            return true;
        }
        return false;
    }
    
}
