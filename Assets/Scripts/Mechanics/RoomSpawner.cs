using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Platformer.Mechanics
{
    public class RoomSpawner : MonoBehaviour
    {
        public bool WillSpawn = true;
        
        // This will basically function as a unique hash ID for our object,
        // more efficient solutions exist but we wont have many enemies so it's fine
        public string uID = null;

        // Start is called before the first frame update
        public virtual void Start()
        {
            uID = string.Format("x: {0}, y: {1}, z: {2}", transform.position.x, transform.position.y, transform.position.z);
            Debug.Log(uID);
            GameManager.Instance.RegisterRoomSpawner(this);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
            
            