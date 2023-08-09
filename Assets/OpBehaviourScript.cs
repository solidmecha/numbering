using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpBehaviourScript : MonoBehaviour {

    public int ID;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { TryOP(); });
	}

    public void TryOP() {
        if (GameControl.singleton.Op != null)
        {
            GameControl.singleton.Op.SetButtonColor(Color.white);
        }
        GameControl.singleton.Op = this;
        SetButtonColor(Color.green);
        if (GameControl.singleton.A != null && GameControl.singleton.B != null)
        {
            GameControl.singleton.PerformOp();
        }
        else if(GameControl.singleton.A != null && ID==7)
        {
            GameControl.singleton.PerformOp();
        }
    }
    public void SetButtonColor(Color c)
    {
        GetComponent<Image>().color = c;
    }
}
