using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExpencesFiller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI expences;
    private void OnEnable()
    {
        expences.text = "- "+ GameController.instance.player.finances.GetTotalMonthExpences().ToString() + " $";
    }
}
