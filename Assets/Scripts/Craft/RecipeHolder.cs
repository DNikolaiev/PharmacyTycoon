using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RecipeHolder : Holder, IPointerDownHandler, IDescription, IPointerEnterHandler
{
    public Recipe recipe;
    public Text quantitytxt;
    public Text priceText;
    public Text deathText;
    [SerializeField] Button action;
    [SerializeField] Button deleteButton;
    [SerializeField] ConfirmationWindow confirm;
    [SerializeField] DragHandler dragger;
    [SerializeField] AudioClip onEndDragSound;
    [SerializeField] AudioClip onSelectSound;
    [SerializeField] Animator anim;
    Canvas canvas;
    public void SetDescription()
    {
        if (!CompareTag("Basic"))
        {
            if( !GameController.instance.crafter.view.gameObject.activeInHierarchy)
            RecipeSelector.UnSelectRecipe();
            descriptionPanel.SetPanel(recipe, this);

        }
    }
    public void ClearDescription()
    {
        descriptionPanel.Clear();
       
      
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

        if (!GameController.instance.generalTutorial.isTutorialCompleted && deleteButton != null)
        {
           
                deleteButton.interactable = false;
        }
        else if (GameController.instance.generalTutorial.isTutorialCompleted && deleteButton!=null) deleteButton.interactable = true;
    }
    
    public override void SetPanel()
    {
        if (GameController.instance.player.inventory.CheckIfWarehouseContains(recipe.description.Name))
        {
            if (quantitytxt != null)
                quantitytxt.text = "x" + GameController.instance.player.inventory.GetQuantity(recipe.description.Name).ToString();
            if (priceText != null)
                priceText.text = recipe.price.Value.ToString();
        }
        else return;
        if (deathText != null)
            deathText.text = recipe.GetDeathRating() + " %";

        picture.sprite = recipe.description.sprite;
        if (Nametxt != null)
            Nametxt.text = recipe.description.Name;

    }
    private void InitializeDrag()
    {

        if (GameController.instance.crafter.view.transform.parent.gameObject.activeInHierarchy)
            canvas = GameController.instance.crafter.view.craftPanel.transform.parent.GetComponent<Canvas>();
        else
            canvas = FindObjectOfType<Seller>().view.transform.parent.GetComponent<Canvas>();
        dragger.SetCanvas(canvas);
        dragger.onBeginMethod += OnBeginDrag;
        dragger.onEndMethod += OnEndDrag;
    }
    private void OnBeginDrag()
    {
        if (dragger.dragItem != null) return;
        Debug.Log("BeginDrag");
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity, action.transform.parent.parent.parent.parent);
        copy.GetComponent<RecipeHolder>().picture.raycastTarget = false;
        copy.transform.SetParent(canvas.transform);
        SetDescription();
        dragger.SetDragItem(copy);
        GameController.instance.audio.MakeSound(onSelectSound);
        if (anim != null)
            anim.Play("CraftRecipe_Select");
    }
    private void OnEndDrag()
    {

        if (RecipeSelector.recipeHolderSelected != null)
        {
            RecipeSelector.SelectRecipe(recipe);
            Debug.Log("EndDrag1");
            dragger.ResetDragItem();
            GameController.instance.audio.MakeSound(onEndDragSound);
            return;
        }


        if (GameController.instance.crafter.view.holderSelected == null)
        {
            dragger.ResetDragItem();
            return;
        }
        if (!GameController.instance.crafter.view.holderSelected.isUnlocked || (GameController.instance.crafter.view.holderSelected == null && dragger.dragItem != null))
        {
            dragger.ResetDragItem();
            return;
        }


        if (GameController.instance.crafter.view.holderSelected != null && GameController.instance.crafter.view.recipeHolderSelected == null)
        {
            GameController.instance.crafter.controller.OnAddRecipe(this);
            GameController.instance.audio.MakeSound(onEndDragSound);
            dragger.ResetDragItem();
        }


        else { Debug.Log("EndDragEND"); dragger.ResetDragItem(); }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        if (Input.GetMouseButton(0) && !GameController.instance.crafter.view.gameObject.activeInHierarchy)
            RecipeSelector.recipeHolderSelected = this;

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        ClearDescription();
        SetDescription();
        if (!GameController.instance.crafter.view.gameObject.activeInHierarchy)
        {
            
            RecipeSelector.UnSelectRecipe();
        }

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
        GameController.instance.player.inventory.Delete(recipe.description.Name);
        crafter.PopulateRecipeList(crafter.view.recipesListView, crafter.view.recipeInList);
        crafter.view.capacity.text = GameController.instance.player.inventory.GetNumberOfElements().ToString() + " / " + GameController.instance.player.inventory.capacity.ToString(); // refresh capacity text
        Abort();
        Destroy(transform.parent.parent.gameObject);
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
