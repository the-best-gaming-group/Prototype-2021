using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Rendering;

namespace Platformer.Mechanics
{
    public class PrototypeDSMRunner : MonoBehaviour
    {
        // Start is called before the first frame update
        public DialogStateMachine dsm = new DialogStateMachine();
        public S currentState = S.DSM;
        public readonly float buttonDelay = 0.25f;
        public float delayRemaining = 0f;
        public RunePanelController rpc;
        public SpellController[] scs = new SpellController[4];
        public SubmitController sc;
        private Selectable previousSelect;
        void Start()
        {
            previousSelect = rpc;
            rpc.Show();
        }
        
        void Awake() {
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
            
            if (dsm.state != currentState) {
                if (currentState == S.RSM) {
                    rpc.LoseFocus();
                    // todo: add dpc
                }
                else {
                    // todo: add dpc
                    rpc.GainFocus();
                }
            }
            currentState = dsm.state;
            if (currentState == S.RSM) {
                rpc.ChangeSelect((int)dsm.runeState);
            }
            else {
                Selectable sel = dsm.dialogState switch {
                    DS.RUNES => rpc,
                    DS.SPELL_ONE => scs[0],
                    DS.SPELL_TWO => scs[1],
                    DS.SPELL_THREE => scs[2],
                    DS.SPELL_FOUR => scs[3],
                    DS.SUBMIT => sc,
                    _ => rpc
                };
                previousSelect.Hide();
                sel.Show();
                previousSelect = sel;
            }
            
        }
    }
}
