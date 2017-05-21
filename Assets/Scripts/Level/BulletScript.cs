using UnityEngine;
using System.Collections;

public class BulletScript : PoolObject
{
    public bool _isNearMiss = false;
    public bool IsNearMiss
    {
        get
        {
            return _isNearMiss;
        }
    }
    private int _nearMisses = 0;
    public int NearMisses
    {
        get
        {
            return _nearMisses;
        }
    }

    private bool _isHit = true;
    public bool IsHit
    {
        get
        {
            return _isHit;
        }
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public override void Reset()
    {
        base.Reset();
        _isNearMiss = false;
        _nearMisses = 0;
        _isHit = false;
    }

    public void SetHit()
    {
        _isHit = true;
    }

    public void SetNearMiss()
    {
        _isNearMiss = true;
        Debug.Log("Near miss!");
        ++_nearMisses;
    }
}
