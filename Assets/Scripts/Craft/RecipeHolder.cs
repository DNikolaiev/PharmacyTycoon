using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RecipeHolder: Holder, IPointerDownHandler, IDescription, IPointerEnterHandler {
    public Recipe recipe;
    public Text quantitytxt;
    public Text deathText;
    [SerializeField] Button action;
    [SerializeField] ConfirmationWindow confirm;
    [SerializeField] DragHandler dragger;
    public  void SetDescription()
    {
        descriptionPanel.SetPanel(recipe, this);
    }

    private void Start()
    {
        
        confirm = GameController.instance.buttons.confirm;
        if (isUnlocked)
            lockedSprite.SetActive(false);
        if (dragger != null)
            InitializeDrag();
        descriptionPanel = GameController.instance.buttons.GetDescriptionPanel(this);
        if (action != null)
            action.onClick.AddListener(SetDescription);
        if (recipe != null)
            SetPanel();
    }
    public override void SetPanel()
    {
        if (GameController.instance.crafter.inventory.CheckIfWarehouseContains(recipe.description.Name))
        {
            if (quantitytxt != null)
                quantitytxt.text = "x" + GameController.instance.crafter.inventory.GetQuantity(recipe.description.Name).ToString();
        }
        else return;
        if(deathText!=null)
             deathText.text = "Death Rating - " + recipe.DeathRating + " %";
        recipe.description.sprite = Resources.Load<Sprite>("Icons/Recipe/Liquid-1");
        picture.sprite = recipe.description.sprite;
        if(Nametxt!=null)
        Nametxt.text = recipe.description.Name;

    }
    private void InitializeDrag()
    {
        dragger.SetCanvas(GameController.instance.crafter.view.craftPanel.transform.parent.GetComponent<Canvas>());
        dragger.onBeginMethod += OnBeginDrag;
        dragger.onEndMethod += OnEndDrag;
    }
    private void OnBeginDrag()
    {
        if (dragger.dragItem != null) return;
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity, action.transform.parent.parent.parent.parent);
        copy.GetComponent<RecipeHolder>().picture.raycastTarget = false;
        copy.transform.SetParent(GameController.instance.crafter.view.transform.parent);

        dragger.SetDragItem(copy);
    }
    private void OnEndDrag()
    {
      
        if (!GameController.instance.crafter.view.holderSelected.isUnlocked || (GameController.instance.crafter.view.holderSelected == null && dragger.dragItem != null))
        {
            dragger.ResetDragItem();
            return;
        }


        if (GameController.instance.crafter.view.holderSelected != null && GameController.instance.crafter.view.recipeHolderSelected==null)
        {
            GameController.instance.crafter.controller.OnAddRecipe(this);
            dragger.ResetDragItem();
        }
        else if(GameController.instance.crafter.view.recipeHolderSelected!=null)
        {
            GameController.instance.crafter.controller.OnAddRecipe(recipe);
            dragger.ResetDragItem();
        }

        else dragger.ResetDragItem();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        if (Input.GetMouseButton(0)) 
           GameController.instance.crafter.controller.OnSelectHolder(this);

    }


    public  void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        SetDescription();
        
    }
    public void OnDelete()
    {
        GameController.instance.crafter.view.DisableAllParticles();
        confirm.SetPanel("Delete " + recipe.description.Name + " ?");
        confirm.Activate(true);
        confirm.ok.onClick.AddListener(delegate { Delete(); });
        confirm.cancel.onClick.AddListener(Abort);
        GameController.instance.buttons.cancel.gameObject.SetActive(false);
    }
    private void Abort()
    {
        confirm.Activate(false);
        confirm.Hide();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        GameController.instance.crafter.view.EmitLastParticles();
    }
    private void Delete()
    {
        var crafter = GameController.instance.crafter;
        crafter.isPrescripted = false;
        crafter.recipeSelected = null;
        crafter.inventory.Delete(recipe.description.Name);
        crafter.PopulateRecipeList(crafter.view.recipesListView, crafter.view.recipeInList);
        crafter.view.capacity.text = crafter.inventory.GetNumberOfElements().ToString() + " / " + crafter.inventory.capacity.ToString(); // refresh capacity text
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
