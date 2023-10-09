using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Platformer.Mechanics 
{
using static Platformer.Core.Simulation;
    [RequireComponent(typeof(Collider), typeof(Invokable))]
    
    public class DoorController : MonoBehaviour
    {
        internal Collider _collider;
        public Bounds Bounds => _collider.bounds;
        public Invokable invokable;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            invokable = GetComponent<Invokable>();
        }

        void Update()
        {
            if (isInteractable() && Input.GetAxis("Vertical") > 0) {
                invokable.Invoke();
               
            }
        }

        
        public bool isInteractable() {

            return false;
        }
    }
}