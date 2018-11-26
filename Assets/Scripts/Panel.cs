using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Panel : MonoBehaviour {

    public Text Nametxt;
    public abstract void SetPanel();
    public abstract void Hide();
}
