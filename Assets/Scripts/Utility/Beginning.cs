using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Beginning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(CheckForExistingSaves())
        {
            AcceptTerms();
        }

    }
    
    private bool CheckForExistingSaves()
    {
        SaveController saveController = new SaveController();
        return (Directory.Exists(Path.Combine(Application.persistentDataPath, saveController.directoryName))); 
    }
    public void AcceptTerms()
    {
        SceneManager.LoadScene("Start");
    }
}
