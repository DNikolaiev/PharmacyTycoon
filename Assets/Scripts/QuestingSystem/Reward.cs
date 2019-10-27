using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Reward {

   [Range(1,100)] public float experiencePercentage;
    public int money;
    public int herbs;
    public int chems;
    public int plastic;
    public int researchPoints;
    public int coins;
    public void RewardPlayer(float rewardMultiplier=1)
    {
        Player player = GameController.instance.player;
        player.GainExperience((int)((float)player.experienceNeeded * ((experiencePercentage*rewardMultiplier) / 100)));
        player.resources.ChangeBalance((int)(money*rewardMultiplier));
        player.resources.AddChemistry((int)(chems*rewardMultiplier));
        player.resources.AddHealingPlants((int)(herbs*rewardMultiplier));
        player.resources.AddPlastic((int)(plastic*rewardMultiplier));
        player.resources.AddResearchPoints((int)(researchPoints));
        player.resources.AddCoins((int)(coins*rewardMultiplier));
    }
}
