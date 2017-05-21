using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum VisualType
{
    NONE,
    
    SWEET,
    COOL,
    AWESOME,
    MAJESTIC,
    ONFIRE,
    GODLIKE,
    CHEAT,

    SOCLOSE,
    SUPERCLOSE,

    WTF
}

public class Visual
{
    public float RemainingTime = 0;
    public PoolObject VisualObj = null;

    public Visual(float remainingTime, PoolObject obj)
    {
        RemainingTime = remainingTime;
        VisualObj = obj;
    }
}

public class ScoreboardScript : MonoBehaviour
{
    private PoolManager _pool = null;
    private List<Visual> _visualList = new List<Visual>();

    public Transform VisualParent = null;

	// Use this for initialization
	void Awake ()
    {
        _pool = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // because meh
        foreach(Visual v in _visualList)
        {
            v.RemainingTime -= Time.deltaTime;
            if(v.RemainingTime <= 0)
            {
                v.VisualObj.Reset();
                v.VisualObj.Deactivate();
            }
        }
        _visualList.RemoveAll(e => e.RemainingTime <= 0);
	}

    public void PushVisual(int score, VisualType type)
    {
        PoolObject po = _pool.ActivateObject("ScoreVisual");
		po.transform.SetParent(VisualParent);
        po.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        po.transform.localPosition = new Vector3(0, 0, 0);
        string text = "";
        switch(type)
        {
            case VisualType.SWEET:
                text = "Sweet";
                break;
            case VisualType.COOL:
                text = "Cool";
                break;
            case VisualType.AWESOME:
                text = "Awesome!";
                break;
            case VisualType.MAJESTIC:
                text = "Majestic";
                break;
            case VisualType.ONFIRE:
                text = "On fire";
                break;
            case VisualType.GODLIKE:
                text = "Godlike";
                break;
            case VisualType.CHEAT:
                text = "CHEAT!";
                break;

            case VisualType.SOCLOSE:
                text = "So close";
                break;
            case VisualType.SUPERCLOSE:
                text = "Superclose!";
                break;

            case VisualType.WTF:
                text = "What the...";
                break;
        }
        po.GetComponent<VisualScript>().VisualText.text = text + " +" + score;
        po.GetComponent<Animator>().Play("Score_Addition");

        _visualList.Add(new Visual(0.5f, po));
    }
}
