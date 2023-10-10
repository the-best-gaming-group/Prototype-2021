using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomEnemySpawner : MonoBehaviour
{
    public bool WillSpawn = true;
    
    // This will basically function as a unique hash ID for our object,
    // more efficient solutions exist but we wont have many enemies so it's fine
    public string uID = null;

    // Start is called before the first frame update
    void Start()
    {
        uID ??= string.Format("x: {0}, y: {1}, z: {2}", transform.position.x, transform.position.y, transform.position.z);

        GetComponentInChildren<Enemy>().uID = uID;
        GameManager.Instance.RegisterRoomEnemySpawner(this);
        if (!GameManager.Instance.CanSpawn(uID))
        {
            transform.GameObject().SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
