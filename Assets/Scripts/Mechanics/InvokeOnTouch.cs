using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Animations;
using Platformer.Core;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider), typeof(Invokable))]
    public class InvokeOnTouch : MonoBehaviour
    {
        public Invokable invokableObject;
        bool isInteractable = false;
        // Start is called before the first frame update
        void Start()
        {
            invokableObject = GetComponent<Invokable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isInteractable && ( Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("Jump"))) {
                invokableObject.Invoke();
            }
        }
        void OnCollisionStay(Collision collision)
        {
            var player = collision.gameObject.GetComponent<GhostController>();
            if (player != null)
            {
                isInteractable = true;
            }
        }
        
        void OnCollisionExit(Collision collision) {
            var player = collision.gameObject.GetComponent<GhostController>();
            Debug.Log(player);
            if (player != null)
            {
                isInteractable = false;
            }
        }
    }
}