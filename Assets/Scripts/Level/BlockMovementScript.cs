using UnityEngine;
using System.Collections;

public class BlockMovementScript : MonoBehaviour
{
    private float _movementSpeed = 0.1f;
    public float MovementSpeed
    {
        // don't really need a getter here so specific define of the public variable
        set
        {
            _movementSpeed = value;
        }
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
}
