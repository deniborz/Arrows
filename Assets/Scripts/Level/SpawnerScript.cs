using UnityEngine;
using System.Collections; // iterator include
using System.Collections.Generic; // list include

public class SpawnerScript : MonoBehaviour
{
    // PLACE IMPORTANT OBJECTS AND DATA HERE
    private LevelManager _level = null;
    private PoolManager _pool = null;

    public List<SpawnPointScript> SpawnPoints = new List<SpawnPointScript>();
    private List<PoolObject> _activeLevelBlocks = new List<PoolObject>();

    public float BlockBorder = 0.0f;
    public bool IsGameVertical = true; // currently just for fun here

    private bool _spawnCredit = false;

	void Awake ()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); // fetch the level manager once
        _pool = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolManager>(); // fetch the pool manager once
        if (_pool == null)
            Debug.Log("ERROR! Pool manager not found"); // just keep this one in

        StartCoroutine(SpawnBlocks());
        StartCoroutine(RemoveBlocks());
    }

    void Start()
    {
    }
	
	void Update ()
    {


    }

    public void SpawnCredit()
    {
        _spawnCredit = true;
    }

    IEnumerator SpawnBlocks()
    {
        yield return new WaitForSeconds(1); // hold the spawner for the countdown
        UnlockLine();
        while (true) // sounds so bad
        {
            List<SpawnPointScript> _possibleLocations = SpawnPoints.FindAll(e => e.IsUnlocked); // Lambda again, a little bit heavier but its on separate thread

            float delay = _level.CurrentSpawnDelay;
            if (_possibleLocations.Count > 0)
            {
                int line = Random.Range(0, _possibleLocations.Count); // Range is exclusive the max value
                                                                      // TTT Fix this for the amount we have
                                                                      // get a random size

                if (!_spawnCredit)
                {
                    int size = Random.Range(2, 5);
                    delay *= size;
                    PoolObject block = _pool.ActivateObject("L1Block" + size);
                    block.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, 90);
                    block.GetComponent<BlockScript>().MovementSpeed = 250.0f;

                    block.transform.position = _possibleLocations[line].transform.position;

                    _activeLevelBlocks.Add(block);
                }
                else
                {
                    int size = Random.Range(2, 5);
                    delay *= size;

                    PoolObject credit = _pool.ActivateObject("Credit");
                    credit.GetComponent<CreditScript>().MovementSpeed = 250.0f;

                    credit.transform.position = _possibleLocations[line].transform.position;

                    _activeLevelBlocks.Add(credit);

                    _spawnCredit = false;
                }
            }
            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator RemoveBlocks()
    {
        while (_level.IsPlaying || _activeLevelBlocks.Count > 0) // if the player died we still can clean up nicely right?
        {
            if (IsGameVertical)
            {
                for(int count = 0; count < _activeLevelBlocks.Count; ++count)
                {
                    if(_activeLevelBlocks[count].transform.position.x >= BlockBorder)
                    {
                        _activeLevelBlocks[count].Deactivate(); // reset the pool object by deactivating it, so it becomes available again
                    }
                }

                // Lambda expression, a little heavier but results in cleaner code :)
                //"e" is set as value of the type of the list, in this case PoolObject, so you can call methods/data like it is that object
                _activeLevelBlocks.RemoveAll(e => !e.IsActive);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void UnlockLine()
    {
        foreach(SpawnPointScript sps in SpawnPoints)
        {
            if(!sps.IsUnlocked)
            {
                Debug.Log("Unlocked a new line");
                sps.Unlock();
                return; // just get out of this function
            }
        }
    }
}
