using UnityEngine;

namespace Platformer.Mechanics
{
    using static Platformer.Mechanics.Turn_Direction;
    public enum Turn_Direction {
        LEFT,
        RIGHT,
        FRONT,
        BACK,
        NOT_TURNING
    }
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// </summary>
    public class GhostController : MonoBehaviour
    {
        private readonly static Vector3 RIGHT_TURN = new Vector3(0,180,0);
        private readonly static Vector3 LEFT_TURN = new Vector3(0,-180,0);
        private GameObject _ghost_model;
        private const float speed = 0.125f;
        public Turn_Direction turn_dir = NOT_TURNING;
        public Turn_Direction last_turn_dir = NOT_TURNING;
        /*internal new*/ public Collider _collider;

        public Rigidbody _rigidbody;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            _ghost_model = GameObject.Find("ghost basic");
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void FixedUpdate()
        {
            last_turn_dir = turn_dir;
            if (turn_dir == RIGHT && transform.eulerAngles.y > 180) {
                _rigidbody.MoveRotation(Quaternion.Euler(0, 180f, 0));
            }
            else if (turn_dir == LEFT && transform.eulerAngles.y > 270) {
                _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
            }

            var sideMove = Input.GetAxis("Horizontal") * 4;
            if (sideMove > 0|| last_turn_dir == RIGHT && _rigidbody.transform.eulerAngles.y < 180) {
                turn_dir = RIGHT;
            }
            else if (sideMove < 0 || last_turn_dir == LEFT && _rigidbody.transform.eulerAngles.y > 0) {
                turn_dir = LEFT;
            }
            else {
                turn_dir = NOT_TURNING;
            }
            
            Quaternion deltaRotation = Quaternion.identity;
            if (turn_dir == RIGHT ) {
                deltaRotation = Quaternion.Euler(RIGHT_TURN * Time.fixedDeltaTime);
            }
            else if (turn_dir == LEFT) {
                deltaRotation = Quaternion.Euler(LEFT_TURN * Time.fixedDeltaTime);
            }

            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
            _rigidbody.MovePosition(_rigidbody.transform.position +
                    new Vector3(sideMove * Time.deltaTime,
                                0,
                                0));
            
            // Spoooky float!
            var curr_pos = _ghost_model.transform.position;
            _ghost_model.transform.position = new Vector3(
                curr_pos.x,
                curr_pos.y + speed * Mathf.Cos(Time.time) * Time.fixedDeltaTime,
                curr_pos.z
            );
        }
    }
}