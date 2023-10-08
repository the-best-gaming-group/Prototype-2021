using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Animations;
using Platformer.Core;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider), typeof(Invokable))]
    public class InvokeOnCover : MonoBehaviour
    {
        public Invokable invokableObject;
        GameObject playerObj;
        Collider _collider;
        public Bounds Bounds => _collider.bounds;
        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<Collider>();
            invokableObject = GetComponent<Invokable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerObj == null) {
                playerObj = GameObject.Find("GhostPC");
            }
            else if (IsInteractable() && ( Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("Jump"))) {
                invokableObject.Invoke();
                
            }
        }
        
        bool IsInteractable() {
            var playerBounds = playerObj.GetComponent<GhostController>().Bounds;
            if (playerBounds.Intersects(Bounds)) {
                //doorSound.Play();
                return true;
               
            }
            else {
                return false;            
            }
        }
    }
}
