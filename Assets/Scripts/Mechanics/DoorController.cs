using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Platformer.Mechanics 
{
using static Platformer.Core.Simulation;
    [RequireComponent(typeof(Collider2D), typeof(Invokable))]
    public class DoorController : MonoBehaviour
    {
        internal Collider2D _collider;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;
        public Invokable invokable;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            invokable = GetComponent<Invokable>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<GhostController>();
            if (player != null)
            {
                var ev = Schedule<PlayerDoorCollision>();
                ev.player = player;
                ev.door = this;
            }
        }

        void Update()
        {
        }

    }
}