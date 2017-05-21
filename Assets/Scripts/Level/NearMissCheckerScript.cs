using UnityEngine;
using System.Collections;

public class NearMissCheckerScript : MonoBehaviour
{

    private BulletScript _parent = null;

    // Use this for initialization
    void Awake()
    {
        _parent = transform.parent.GetComponent<BulletScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Block")
        {
            _parent.SetNearMiss();
        }
    }
}
