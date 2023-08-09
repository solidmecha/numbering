using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumControl : MonoBehaviour {
    public int Val;
    public Text NumText;
    private void Start ()
    {
        GetComponent<Button>().onClick.AddListener(delegate { SelectNum(); });
    }
    public void SelectNum()
    {
        if (GameControl.singleton.A == null && GameControl.singleton.B == null)
        {
            GameControl.singleton.A = this;
            SetButtonColor(Color.cyan);
            if (GameControl.singleton.Op != null && GameControl.singleton.Op.ID==7)
            {
                GameControl.singleton.PerformOp();
            }
        }
        else if(GameControl.singleton.A != null && GameControl.singleton.B != null)
        {
            GameControl.singleton.A.SetButtonColor(Color.white);
            GameControl.singleton.B.SetButtonColor(Color.white);
            GameControl.singleton.B= null;
            GameControl.singleton.A = this;
            SetButtonColor(Color.cyan);
        }
        else if(GameControl.singleton.A != null && GameControl.singleton.B == null)
        {
            GameControl.singleton.B= this;
            SetButtonColor(Color.magenta);
            if(GameControl.singleton.Op!=null)
            {
                GameControl.singleton.PerformOp();
            }
        }
    }
    public void SetButtonColor(Color c)
    {
        GetComponent<Image>().color= c;
    }
}
