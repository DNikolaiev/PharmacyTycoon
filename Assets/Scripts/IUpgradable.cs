using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IUpgradable {
    
    void Upgrade(int level);
    void IncreaseStats(int lvl);
}
