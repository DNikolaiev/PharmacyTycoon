using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IUpgradable {
    
    void Upgrade(int level, bool chargePlayer=true);
    void IncreaseStats(int lvl, bool expand=true);
}
