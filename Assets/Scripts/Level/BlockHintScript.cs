using UnityEngine;
using System.Collections;

public class BlockHintScript : MonoBehaviour
{
    public TriggerScript BeginHint = null;
    public TriggerScript EndHint = null;

    private Animator _anim = null;

    private bool _showHint = false;

	void Awake ()
    {
        _anim = GetComponent<Animator>();
    }
	
	void LateUpdate ()
    {
        if (EndHint.IsTouching)
            _showHint = false;
        if (BeginHint.IsTouching)
            _showHint = true;

        _anim.SetBool("ShowHint", _showHint);
	}
}
