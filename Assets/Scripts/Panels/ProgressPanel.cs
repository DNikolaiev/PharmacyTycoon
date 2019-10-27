using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
public class ProgressPanel : Panel {
    public TextMeshProUGUI medicine, recipes, recipesTotal, timePlayed, factoryArea, killed, cured, money;
    [SerializeField] PlayerPanel pPanel;
    [SerializeField] BarFiller progress;
    [SerializeField] Tutorial tutorial;
    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPanel()
    {
        Player player = GameController.instance.player;
        medicine.text = GameController.instance.talentTree.talents.Where(x=>x.isUnlocked).ToList().Count + " / " + GameController.instance.talentTree.talents.Count;
        recipes.text = player.inventory.GetNumberOfElements().ToString() + " / " + player.inventory.capacity.ToString();
        recipesTotal.text = player.inventory.recipes.Count.ToString();
        factoryArea.text = (GameController.instance.roomOverseer.rooms.Count*100).ToString();
        int killedAmount = 0; int curedAmount = 0;
        GetAreasData(ref killedAmount, ref curedAmount);
        killed.text = killedAmount.ToString();
        cured.text = curedAmount.ToString();
        money.text = player.resources.money.ToString();
        timePlayed.text = (player.finances.dates.Count>1)?(player.finances.dates.LastOrDefault().year - player.finances.dates[0].year).ToString():" 0";
        pPanel.SetPanel();
        progress.SetValueToBarScalar(Mathf.Round(player.finances.GetProductivity() * 100), progress.healingBar, 100);
        progress.healingBar.GetComponent<Button>().onClick.RemoveAllListeners();
        progress.healingBar.GetComponent<Button>().onClick.AddListener(delegate { GameController.instance.buttons.GetHint("Productivity factor at " +  Mathf.Round(player.finances.GetProductivity()*100) + "%"); });
        if (!tutorial.isTutorialCompleted) tutorial.StartTutorial();
        GameController.instance.buttons.quest.GetComponentInChildren<ParticleSystem>().Stop();
    }
    private void GetAreasData(ref int killed, ref int cured)
    {
        foreach(Area area in World.instance.areas)
        {
            killed += area.dead;
            cured += area.cured;
        }
    }
    // Use this for initialization
    private void OnEnable()
    {
        SetPanel();
    }
    void Start () {
        
        EventManager.StartListening(TurnController.OnTurnEvent, SetPanel);
	}
	
	
}
