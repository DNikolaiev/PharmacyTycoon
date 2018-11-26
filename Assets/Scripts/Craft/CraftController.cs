using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour {
 
    public void OnSelectHolder(CraftHolder holder) // on click in main talent holders
    {
        Crafter.instance.holderSelected = holder;
    }
   
    public void OnAddTalent(TalentHolder talentHolder) // on click in talent's list
    {
        Crafter.instance.craftDescriptionPanel.SetPanel(talentHolder.Talent, (CraftHolder)talentHolder);
      
        Crafter.instance.AssignTalent(talentHolder.Talent);
    }
}
