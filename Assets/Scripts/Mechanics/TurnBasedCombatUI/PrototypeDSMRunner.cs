using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Platformer.Mechanics
{
    public class PrototypeDSMRunner : MonoBehaviour
    {
        // Start is called before the first frame update
        public DialogStateMachine dsm;
        public readonly float buttonDelay = 0.25f;
        public float delayRemaining = 0f;
        public RunePanelController rpc;
        void Start()
        {
            dsm = new DialogStateMachine();
        }

        // Update is called once per frame
        void Update()
        {
            if (delayRemaining > 0f) {
                delayRemaining -= Time.deltaTime;
                return;
            }
            if (Input.GetButtonUp("Jump")) {
                dsm.RunStateMachine(BUTTON.SELECT);
            }
            else if (Input.GetKeyUp(KeyCode.Escape)) {
                dsm.RunStateMachine(BUTTON.BACK);
            }
            else if (Input.GetAxis("Horizontal") > 0) {
                dsm.RunStateMachine(BUTTON.RIGHT);
            }
            else if (Input.GetAxis("Horizontal") < 0) {
                dsm.RunStateMachine(BUTTON.LEFT);
            }
            else if (Input.GetAxis("Vertical") > 0) {
                dsm.RunStateMachine(BUTTON.UP);
            }
            else if (Input.GetAxis("Vertical") < 0) {
                dsm.RunStateMachine(BUTTON.DOWN);
            }
            else {
                return;
            }
            delayRemaining = buttonDelay;
            
            if (dsm.state == S.RSM) {
                rpc.ChangeSelect((int)dsm.runeState);
            }
        }
    }
}
