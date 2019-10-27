using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePanel : HintPanel {

    [SerializeField] RewardPanel rPanel;
    [SerializeField] ParticleSystem particles;
    public void SetPanel(string description, string Name, Sprite toShow=null, Reward reward = null)
    {
        
        base.SetPanel(description, Name, toShow);
        if(rPanel!=null && reward!=null)
        {
            rPanel.SetPanel(reward);
        }
        if (particles != null)
            particles.Play();
    }
    
}
