using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProgress : MonoBehaviour {

   [SerializeField] SceneSaver saver;
   [SerializeField] SceneLoader loader;

    public void Save()
    {
        SaveController sC = new SaveController();
        saver.SaveScene();
        sC.SaveData("scene.json", saver);
    }
    public void Load()
    {
        SaveController sC = new SaveController();
        saver = (SceneSaver)sC.LoadData("scene.json", saver);
        loader.LoadScene(saver);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }
}
