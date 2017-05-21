using UnityEngine;
using System.Collections;

public class CreditScript : PoolObject
{
    private LevelManager _level = null;
    private Rigidbody2D _rigid = null;

    private float _movementSpeed = 0.1f;
    public float MovementSpeed
    {
        // don't really need a getter here so specific define of the public variable
        set
        {
            _movementSpeed = value;
            _rigid.velocity = _movementSpeed * -Vector3.left;
        }
    }

    void Awake()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            _level.AddCredit();
        }
    }
}
