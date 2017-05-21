using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{

    public Animator AdButton = null;
    public Animator GiftButton = null;
    public Animator PrizeButton = null;
    public Animator RestartButton = null;

    public float DelayInterval = 1.0f;

    void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void ShowMenu()
    {
        // go through all the items and see if they need to be launched
        float delay = 0.0f;
        StartCoroutine(PopupButton(AdButton, delay));
        StartCoroutine(PopupButton(GiftButton, delay += DelayInterval));
        StartCoroutine(PopupButton(PrizeButton, delay += DelayInterval));
        StartCoroutine(PopupButton(RestartButton, delay += DelayInterval));
    }

    IEnumerator PopupButton(Animator button, float delay)
    {
        yield return new WaitForSeconds(delay);
        button.Play("PopUp");
    }
}
