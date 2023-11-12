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
            Debug.Log("Anchor needs an anchor point");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor != null)
        {
            transform.position = anchor.transform.position;
        }
    }
}
