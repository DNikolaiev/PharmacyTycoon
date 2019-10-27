using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSelector : MonoBehaviour
{
    public static RecipeHolder recipeHolderSelected;

    public static bool IsSelected
    {
        get
        {
            return (recipeHolderSelected != null && recipeHolderSelected.recipe != null) ? true : false;
        }

    }
    public static IVisualize visualizer;
    public static void SelectRecipe(Recipe recipe)
    {
        recipeHolderSelected.recipe = recipe;

        recipeHolderSelected.SetPanel();

        recipeHolderSelected.SetDescription();

        visualizer.DrawBar();
        


    }
    public static void UnSelectRecipe()
    {
        if (recipeHolderSelected != null && recipeHolderSelected.recipe != null)
        {
            recipeHolderSelected.recipe = null;
            recipeHolderSelected.picture.sprite = recipeHolderSelected.defaultSprite;
            recipeHolderSelected.ClearDescription();
            recipeHolderSelected = null;
            EventManager.TriggerEvent("OnRecipeRemove");
        }
    }
    public void SetVisualizer(IVisualize visualize)
    {
        visualizer = visualize;
    }
}
