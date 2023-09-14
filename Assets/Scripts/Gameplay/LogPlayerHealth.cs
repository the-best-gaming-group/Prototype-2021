using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player health is: " + GlobalData.playerHealth.currentHP.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
