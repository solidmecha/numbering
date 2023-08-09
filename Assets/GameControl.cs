using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class GameControl : MonoBehaviour {
    public static GameControl singleton;
    public List<NumControl> StartingNums=new List<NumControl> { };
    System.Random RNG;
    public NumControl A;
    public NumControl B;
    public OpBehaviourScript Op;
    public int Target;
    public Text TargetText;
    public int SolveCount;
    public float SolveTime;
    public float SolveSpeed;
    public Text CountText;
    public Text TimerText;
    public Text SpeedText;
    List<int> Vals=new List<int>();
    List<GameObject> UsedButtons=new List<GameObject>();
    private void Awake()
    {
        singleton= this;
        RNG = new System.Random();
    }

	// Use this for initialization
	void Start () {
	SetupNums();
        SetTarget();
	}

    public void SetupNums()
    {
        int max=10+RNG.Next(21);
        List<int> temp= new List<int>();
        for( int i=1; i<max; i++ )
        {
            temp.Add(i);
        }

        for(int i=0;i<StartingNums.Count;i++ )
        {
            int r=RNG.Next(temp.Count);
            StartingNums[i].Val = temp[r];
            temp.RemoveAt(r);
            StartingNums[i].NumText.text= StartingNums[i].Val.ToString();
        }
    }
    public void SetTarget()
    {
        List<int> startNums=new List<int>();
        for(int i=0; i<StartingNums.Count;i++ )
            startNums.Add(StartingNums[i].Val);
        int Steps = RNG.Next(3, 6);
        for(int i=0; i < 5-Steps;i++)
            startNums.RemoveAt(RNG.Next(startNums.Count));  
        for(int i=0;i<Steps;i++ )
        {
            int a = RNG.Next(startNums.Count);
            int b = a;
            while(b==a)
                b= RNG.Next(startNums.Count);
            int opIDmax = 3;
            if (a != 0 && b != 0 && a % b == 0)
                opIDmax++;
            startNums[a]=RandomOP(RNG.Next(opIDmax), startNums[a], startNums[b]);
            startNums.RemoveAt(b);
        }
        print(startNums.Count);
        Target = startNums[0];
        TargetText.text = Target.ToString();
    }

    public void PerformOp()
    {
        if (!((Op.ID == 5 && (Math.Pow(A.Val, B.Val) > int.MaxValue || Math.Pow(A.Val, B.Val) < int.MinValue)) ||
            (Op.ID==3 && A.Val%B.Val!=0) ||
            (Op.ID==7 && A.Val%Math.Pow(A.Val, .5)!=0)))
        {
            if (B == null)
                B = A;
            Vals.Add(A.Val);
            Vals.Add(B.Val);
            UsedButtons.Add(A.gameObject);
            UsedButtons.Add(B.gameObject);
            B.Val = RandomOP(Op.ID, A.Val, B.Val);
            B.NumText.text = B.Val.ToString();
            if (Op.ID != 7)
            {
                A.gameObject.SetActive(false);
            }

            if(B.Val==Target)
            {
                SetupNums();
                SetTarget();
                SolveCount++;
                CountText.text=SolveCount.ToString();
                SolveSpeed = ((SolveCount-1)*SolveSpeed+SolveTime)/SolveCount;
                SpeedText.text=SolveSpeed.ToString();
                SolveTime = 0;
                foreach (NumControl n in StartingNums)
                    n.gameObject.SetActive(true);
                Vals.Clear();
                UsedButtons.Clear();
            }
        }
        A.SetButtonColor(Color.white);
        if (B != null)
        {
            B.SetButtonColor(Color.white);
            B = null;
        }
        Op.SetButtonColor(Color.white);
        Op = null;
        A = null;
    }
    public void Undo()
    {
        if (UsedButtons.Count > 0)
        {
            UsedButtons[UsedButtons.Count - 2].GetComponent<NumControl>().Val = Vals[Vals.Count - 2];
            UsedButtons[UsedButtons.Count - 2].GetComponent<NumControl>().NumText.text = Vals[Vals.Count - 2].ToString();
            UsedButtons[UsedButtons.Count - 1].GetComponent<NumControl>().Val = Vals[Vals.Count - 1];
            UsedButtons[UsedButtons.Count - 1].GetComponent<NumControl>().NumText.text = Vals[Vals.Count - 1].ToString();
            UsedButtons[UsedButtons.Count - 2].SetActive(true);
            UsedButtons.RemoveAt(UsedButtons.Count - 1);
            UsedButtons.RemoveAt(UsedButtons.Count - 1);
            Vals.RemoveAt(Vals.Count - 1);
            Vals.RemoveAt(Vals.Count - 1);
            if (A != null)
            {
                A.SetButtonColor(Color.white);
                A = null;
            }
            if (B != null)
            {
                B.SetButtonColor(Color.white);
                B = null;
            }
            if (Op != null)
            {
                Op.SetButtonColor(Color.white);
                Op = null;
            }
        }
    }

    int RandomOP(int ID, int A, int B)
    {
        switch(ID) {
            case 1:
                return A - B;
            case 2:
                return A * B;
            case 3:
                return A / B;
            case 4:
                return A % B;
            case 5:
                return (int)Math.Pow(A, B);
            case 6:
                return  Int32.Parse(A.ToString()+B.ToString());
            case 7:
                return (int)Math.Pow(A, .5f);
            default:
                return A + B;
        }
    }
	
	// Update is called once per frame
	void Update () {
        SolveTime += Time.deltaTime;
        SolveTime= (int)(SolveTime*100)/100f;
        string s = SolveTime.ToString();
       if(!s.Contains("."))
            s += ".00";
       if (s.Length>1 && s[s.Length - 2].Equals('.'))
            s += "0";
        TimerText.text = s;
	}
}
