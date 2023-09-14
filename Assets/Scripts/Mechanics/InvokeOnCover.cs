using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Animations;
using Platformer.Core;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Invokable))]
    public class InvokeOnCover : MonoBehaviour
    {
        public Invokable invokableObject;
        Transform _transform;
        SpriteRenderer spriteRenderer; // We will need to find a "3d" way of getting this info
        // Start is called before the first frame update
        GameObject playerObj;
        void Start()
        {
            invokableObject = GetComponent<Invokable>();
            _transform = GetComponent<Transform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerObj == null) {
                playerObj = GameObject.Find("Player");
            }
            else if (IsInteractable() && ( Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("Jump"))) {
                invokableObject.Invoke();
            }
        }
        
        bool IsInteractable() {
            var playerSpriteRenderer = playerObj.GetComponent<SpriteRenderer>();
            if (spriteRenderer.bounds.Intersects(playerSpriteRenderer.bounds)) {
                return true;
            }
            else {
                return false;            
            }
        }
    }
}