using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    private LevelManager _level = null;

    public float ComboInterval = 3.0f;
    private float _comboInteval = 0.0f;
    private int _comboCount = 0;

	void Awake ()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

    }
	
	void LateUpdate ()
    {
        _comboInteval -= Time.deltaTime;
        if(_comboInteval <= 0)
        {
            _comboCount = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnter");
        if(other.tag == "Bullet")
        {
            if (!_level.IsPlaying)
                return; // you done goofed

            _comboInteval = ComboInterval;
            ++_comboCount;

            VisualType type = VisualType.NONE;
            switch(_comboCount)
            {
                case 3:
                    type = VisualType.SWEET;
                    break;
                case 4:
                    type = VisualType.COOL;
                    break;
                case 5:
                    type = VisualType.AWESOME;
                    break;
                case 6:
                    type = VisualType.MAJESTIC;
                    break;
                case 7:
                    type = VisualType.ONFIRE;
                    break;
                case 8:
                    type = VisualType.GODLIKE;
                    break;
                default:
                    if (_comboCount > 8)
                        type = VisualType.CHEAT;
                    break;
            }

            BulletScript bullet = other.GetComponent<BulletScript>();
            int multiplier = 1;
            if (bullet.IsHit)
            {
                multiplier = 5;
                Debug.Log("Wtf...");
                type = VisualType.WTF;
            }
            else if (bullet.IsNearMiss)
            {
                multiplier = (int)Mathf.Pow(2, bullet.NearMisses);
                if(bullet.NearMisses > 1)
                    type = VisualType.SOCLOSE;
                else
                    type = VisualType.SUPERCLOSE;
            }

            int score = multiplier * _level.CurrentLevel * 10;
            score += 5 * (_comboCount-1);

            _level.AddHitScore(score, type);
            bullet.Deactivate();
        }
    }
}
