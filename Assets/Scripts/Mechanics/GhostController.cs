using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.Mechanics
{
    using static Platformer.Mechanics.TurnDirection;
    using static Platformer.Mechanics.JumpState;
    public enum TurnDirection {
        LEFT,
        RIGHT,
        FRONT,
        BACK,
        NOT_TURNING
    }
    
    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// </summary>
    public class GhostController : MonoBehaviour
    {
        private readonly static Vector3 RIGHT_TURN = new Vector3(0,180,0);
        private readonly static Vector3 LEFT_TURN = new Vector3(0,-180,0);
        private GameObject _ghost_model;
        private const float floatSpeed = 0.125f;
        private const float moveSpeed = 4f;
        public TurnDirection turn_dir = NOT_TURNING;
        public TurnDirection last_turn_dir = NOT_TURNING;
        public JumpState jump = InFlight;
        private bool usedDoubleJump = false;
        private float keyHoriz = 0f;
        private bool jumpPending = false;
        /*internal new*/ public Collider _collider;

        public Rigidbody _rigidbody;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            _ghost_model = GameObject.Find("ghost basic");
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        void Update() {
            keyHoriz = Input.GetAxis("Horizontal");
            jumpPending = jumpPending || Input.GetButtonDown("Jump");
        }

        protected void FixedUpdate()
        {
            var sideMove = keyHoriz * moveSpeed;
            HandleTurn(sideMove);
            var upMove = HandleJump();

            // Move horizontally
            _rigidbody.MovePosition(_rigidbody.transform.position +
                    new Vector3(sideMove * Time.deltaTime, 0, 0));
            // Move vertically
            _rigidbody.AddForce(new Vector3(0, upMove, 0), ForceMode.Impulse);
            
            // Spoooky float!
            var curr_pos = _ghost_model.transform.position;
            _ghost_model.transform.position = new Vector3(
                curr_pos.x,
                curr_pos.y + floatSpeed * Mathf.Cos(Time.time) * Time.fixedDeltaTime,
                curr_pos.z
            );
        }
        
        private void HandleTurn(float sideMove) {
            // Clamp down the movement before we go futher. This would be easier if
            // Unity didn't roll 0 back to 359 and went negative instead
            last_turn_dir = turn_dir;
            if (turn_dir == RIGHT && transform.eulerAngles.y > 180) {
                _rigidbody.MoveRotation(Quaternion.Euler(0, 180f, 0));
            }
            else if (turn_dir == LEFT && transform.eulerAngles.y > 270) {
                _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
            }

            // Figure out which direction we need to try to turn toward
            if (sideMove > 0 || sideMove == 0 && last_turn_dir == RIGHT && _rigidbody.transform.eulerAngles.y < 180) {
                turn_dir = RIGHT;
            }
            else if (sideMove < 0 || last_turn_dir == LEFT && _rigidbody.transform.eulerAngles.y > 0) {
                turn_dir = LEFT;
            }
            else {
                turn_dir = NOT_TURNING;
            }
            
            // Set the turn
            Quaternion deltaRotation = Quaternion.identity;
            if (turn_dir == RIGHT ) {
                deltaRotation = Quaternion.Euler(RIGHT_TURN * Time.fixedDeltaTime);
            }
            else if (turn_dir == LEFT) {
                deltaRotation = Quaternion.Euler(LEFT_TURN * Time.fixedDeltaTime);
            }

            // Turn
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
        
        private float HandleJump() {
            // Jump!
            var upMove = 0f;
            switch (jump) {
                case InFlight:
                    if (_rigidbody.velocity.y == 0)
                    {
                        jump = Grounded;
                        usedDoubleJump = false;
                        jumpPending = false;
                    }
                    else if (!usedDoubleJump && jumpPending) {
                        usedDoubleJump = true;
                        upMove = moveSpeed;
                        jumpPending = false;
                        var curr_vel = _rigidbody.velocity;
                        _rigidbody.velocity = new Vector3(
                            curr_vel.x,
                            0,
                            curr_vel.z
                        );
                    }
                    break;
                case Grounded:
                    if (jumpPending) {
                        upMove = moveSpeed;
                        jump = InFlight;
                        jumpPending = false;
                    }
                    break;
                case Jumping:
                    if (_rigidbody.velocity.y < 0) {
                        jump = InFlight;
                    }
                    break;
                    
            }
            return upMove;
        }
    }
}