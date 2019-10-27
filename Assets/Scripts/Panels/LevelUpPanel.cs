using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LevelUpPanel : Panel
{
    Player player;
    [SerializeField] PlayerPanel pPanel;
    [SerializeField] Image objectImg;
    [SerializeField] Image talentsImg;
    [SerializeField] Text talents;
    [SerializeField] Text objects;
    [SerializeField] Animation anim;
    [SerializeField] ParticleSystem onLevelUp;
    [SerializeField] AudioClip clip;
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void SetPanel()
    {
        gameObject.SetActive(true);
        anim.Play("LevelUp_Appear");
        pPanel.SetPanel();
        onLevelUp.Play();
        objectImg.gameObject.SetActive(false);
        talentsImg.gameObject.SetActive(false);
        talents.text = string.Empty;
        objects.text = string.Empty;
        if(NewTalentsUnlocked())
        {
            talentsImg.gameObject.SetActive(true);
            talents.text = "New Medicine available!";
        }
        if(NewRoomsUnlocked()!=string.Empty)
        {
            objectImg.gameObject.SetActive(true);
            objects.text = NewRoomsUnlocked() + " available!";
        }
        

    }
    public void MakeSound()
    {
        GameController.instance.audio.MakeSound(clip); 
    }
    private void Start()
    {
        player = GameController.instance.player;
    }
    private bool NewTalentsUnlocked()
    {
        return (GameController.instance.player.level % 2 == 0);
    }
    private string NewRoomsUnlocked()
    {
        List<Constructible> objects = new List<Constructible>();
        objects.AddRange(GameController.instance.constructor.objectsToConstruct); objects.RemoveAt(0);
        var sceneObjects = objects.ConvertAll(x => (SceneObject)x);
        foreach(SceneObject s in sceneObjects)
        {
            if (GameController.instance.player.level == s.requiredLvl)
                return s.description.Name;
        }
        return string.Empty; 
    }
}
