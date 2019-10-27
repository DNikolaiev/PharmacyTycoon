using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class MainMenu : MonoBehaviour {
    public ConfirmationWindow confirmation;
    public AsyncLoader loader;
	public void NewGame()
    {
        confirmation.Activate(true);
        confirmation.SetPanel("Are you sure you want to start a new game? All current data will be overwritten.");
       
        confirmation.ok.onClick.AddListener(OverwriteData);
        confirmation.cancel.onClick.AddListener(confirmation.Hide);
    }
    private void OverwriteData()
    {
        SaveController saveController = new SaveController();
        string path = Path.Combine(Application.persistentDataPath, saveController.directoryName);
        if (Directory.Exists(path)) {
            Directory.Delete(Path.Combine(Application.persistentDataPath, saveController.directoryName), true);
        }
        loader.LoadLevel("SampleScene");
    }
    public void Continue()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {

    }
}
