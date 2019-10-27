using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameOverConditions  {
    private Player player;
    private List<Area> areas;

    public GameOverConditions(Player player, List<Area> areas)
    {
        this.player = player;
        this.areas = areas;
    }
	private bool FirstCondition()
    {
        foreach(Area a in areas)
        {
            if (a.health <= 0)
                return true;
        }
        return false;
    }
    private bool SecondCondition()
    {
        return (player.resources.money <= player.finances.bankruptState);
    }
    public bool CheckCondition()
    {
        return (FirstCondition() || SecondCondition());
    }
}
