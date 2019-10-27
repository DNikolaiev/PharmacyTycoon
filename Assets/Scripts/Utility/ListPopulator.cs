using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class ListPopulator : MonoBehaviour {

    public static void DeleteAllTalents(Transform panel)
    {
        var children = new List<GameObject>();
        foreach (Transform child in panel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
      
    }
    public static void DeleteAllRecipes(Transform panel)
    {
        var children = new List<GameObject>();
        foreach (Transform child in panel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
      
    }
    public static void DeleteAllQuests(Transform panel)
    {
        var children = new List<GameObject>();
        foreach (Transform child in panel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

    }
    public static void PopulateTalentList(Transform panel, GameObject talentPrefab, List<Talent> talents, bool primaryTalents = true)
    {
        List<Talent> openTalents = new List<Talent>();
        if (!primaryTalents)
            openTalents = talents.Where(p => p.isUnlocked && !p.isSelected && !p.isPrimary).ToList(); //populate list
        else openTalents = talents.Where(p => p.isUnlocked && !p.isSelected && p.isPrimary).ToList();
        foreach (Talent talent in openTalents)
        {
            GameObject toInstantiate = Instantiate(talentPrefab, panel);
            toInstantiate.GetComponentInChildren<CraftHolder>().Talent = talent;
        }
    }
    public static void PopulateRecipeList(Transform panel, GameObject recipePrefab, List<Recipe> recipes) 
    {
        DeleteAllRecipes(panel);
        
        foreach (Recipe recipe in recipes)
        {

            GameObject toInstantiate = Instantiate(recipePrefab, panel);
            toInstantiate.GetComponentInChildren<RecipeHolder>().recipe = recipe;
            toInstantiate.GetComponentInChildren<RecipeHolder>().SetPanel();

        }
    }
    public static void PopulateQuestList(Transform panel, GameObject questHolderPrefab, List<Quest> quests, Canvas canvas, QuestGiver giver, GameObject questPrefab)
    {
        DeleteAllQuests(panel);

        foreach (Quest quest in quests)
        {

            GameObject toInstantiate = Instantiate(questHolderPrefab, panel);
            toInstantiate.GetComponentInChildren<QuestHolder>().SetEnvironment(canvas, questPrefab, giver);
            toInstantiate.GetComponentInChildren<QuestHolder>().SetPanel(quest);
            

        }
    }

}
