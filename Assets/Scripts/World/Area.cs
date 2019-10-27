using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName ="Area")]
public class Area : ScriptableObject {
    private int currentHealthPerTurnDecreased;
    public string Name;
    public int health;
    public int maxHealth;
    public int startHealthPerTurnDecreased;
    
    public int maxEpidemicCount;
    public List<Talent> activeEpidemies;
    public int dead;
    public int cured;
    public int deathRatingAllowed;
    public int experienceMultiplier = 1;
    public float sellMultiplier = 1;
    public int maxQuotum = Int32.MaxValue;
    public int soldItems;
    public string[] passiveSkills;
    public Sprite backgroundSprite;
    public void ReduceHealth()
    {
        CalculateHealthDecrease();
        health -= (int)(currentHealthPerTurnDecreased);

        if((float)((float)health/(float)maxHealth) <= 0.4f)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show(Name + " is suffering from diseases! Buy medkit!", Resources.Load<Sprite>("Icons/world_icon"));
        }
        if((float)((float)health/(float)maxHealth)<=0.2)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show(Name + " is in critical state! Buy second chance!", Resources.Load<Sprite>("Icons/world_icon"));
            GameController.instance.buttons.market.SetPanel();
        }
        
    }
    public void RefreshQuotum()
    {
        soldItems = 0;
    }
    public void ResetAll()
    {
        health = (int)(maxHealth*0.8f);
        soldItems = 0;
        dead = 0;
        cured = 0;
    }
    public int CalculateHealthDecrease()
    {
        int timePassed = GameController.instance.player.finances.dates.Count;
        if (timePassed <= 2) currentHealthPerTurnDecreased = startHealthPerTurnDecreased;
        bool escalate = (timePassed % 2 == 0 && timePassed>0) ? true : false;
        if (escalate)
        {
            currentHealthPerTurnDecreased = Mathf.Max((startHealthPerTurnDecreased/2 ) + timePassed * 75,startHealthPerTurnDecreased);
            currentHealthPerTurnDecreased = Mathf.Clamp(currentHealthPerTurnDecreased, 100, (int)maxHealth / 8);
        }
        
        return currentHealthPerTurnDecreased;


    }
}
