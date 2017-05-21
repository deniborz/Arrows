using UnityEngine;
using System.Collections;

public class TriggerScript : MonoBehaviour
{

    private bool _isTouching = false;
    public bool IsTouching
    {
        get
        {
            return _isTouching;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _isTouching = true;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        _isTouching = false;
    }
}
