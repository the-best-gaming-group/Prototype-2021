using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    public GameObject anchor;
    // Start is called before the first frame update
    void Start()
    {
        if (anchor == null) {
            Debug.Log("Error: Anchor needs an anchor point");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = anchor.transform.position;
    }
}
