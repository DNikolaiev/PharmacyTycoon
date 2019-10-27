using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TipGiver : MonoBehaviour {

    public List<string> tips;
    public TextMeshProUGUI tipText;
    private string GenerateTip()
    {
        return tips[Random.Range(0, tips.Count)];
    }
    public void ShowTip()
    {
        tipText.text = GenerateTip();
    }
}
