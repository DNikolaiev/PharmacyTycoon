using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class IconSetup : MonoBehaviour {
    public Sprite[] icons;
    public GameObject prefab;
    public Transform spawnPlace;
    public void SetupRecipeIcons(bool isLiquid)
    {
        var children = new List<GameObject>();
        foreach (Transform child in spawnPlace) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        foreach (Sprite icon in icons)
        {
            if (isLiquid)
            {
                if (icon.name.Contains("liquid"))
                {
                    GameObject toInstantiate = Instantiate(prefab, spawnPlace);
                    toInstantiate.GetComponent<Image>().sprite = icon;
                }
            }
            else
            {
                if (icon.name.Contains("pills"))
                {
                    GameObject toInstantiate = Instantiate(prefab, spawnPlace);
                    toInstantiate.GetComponent<Image>().sprite = icon;
                }
            }
        }
    }

    public Image[] GetAllIcons()
    {
        Image[] Children = new Image[spawnPlace.childCount];
        for (int ID = 0; ID < spawnPlace.childCount; ID++)
        {
            Children[ID] = spawnPlace.GetChild(ID).GetComponent<Image>();
        }
        return Children;
    }


    public Sprite GetRandomIcon(bool isLiquid)
    {
        Sprite[] liquidIcons;
        Sprite[] pillsIcons;
        liquidIcons = icons.Where(x => x.name.Contains("liquid")).ToArray();
        pillsIcons = icons.Where(x => x.name.Contains("pills")).ToArray();
        if (isLiquid)
        {
            int rand = Random.Range(0, liquidIcons.Length);
            return liquidIcons[rand];
        }
        else
        {
            int rand = Random.Range(0, pillsIcons.Length);
            return pillsIcons[rand];
        }
    }
}
