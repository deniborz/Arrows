﻿using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour
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

	void Awake ()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _rigid = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            collision.collider.GetComponent<BulletScript>().SetHit();
            _level.LoseLife(collision.transform.position);
        }
    }
}
