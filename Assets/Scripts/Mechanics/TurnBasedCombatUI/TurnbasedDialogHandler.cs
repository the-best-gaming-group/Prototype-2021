using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Rendering;

namespace Platformer.Mechanics
{
    public class TurnbasedDialogHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        public DialogStateMachine dsm = new DialogStateMachine();
        public S currentState = S.DSM;
        public readonly float buttonDelay = 0.3f;
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
            if (delayRemaining > 0f)
            {
                delayRemaining -= Time.deltaTime;
                return;
            }

            if (Input.GetButtonUp("Jump"))
            {
                DoButtonClick();
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                dsm.state = S.DSM;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                dsm.RunStateMachine(BUTTON.RIGHT);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                dsm.RunStateMachine(BUTTON.LEFT);
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                dsm.RunStateMachine(BUTTON.UP);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                dsm.RunStateMachine(BUTTON.DOWN);
            }
            else
            {
                return;
            }
            
            delayRemaining = buttonDelay;
            DoHighlightChange();
        }
        
        private void DoButtonClick() {
            Func<string> buttonFunc = null;
            if (dsm.state == S.DSM)
            {
                buttonFunc = dsm.dialogState switch {
                    DS.RUNES       => runePanelSelected,
                    DS.SPELL_ONE   => spellOneSelected,
                    DS.SPELL_TWO   => spellTwoSelected,
                    DS.SPELL_THREE => spellThreeSelected,
                    DS.SPELL_FOUR  => spellFourSelected,
                    DS.SUBMIT      => submitSelected,
                    _              => null
                };
            }
            else
            {
                buttonFunc = dsm.runeState switch {
                    RS.RUNE_ONE   => RuneOneSelected,
                    RS.RUNE_TWO   => RuneTwoSelected,
                    RS.RUNE_THREE => RuneThreeSelected,
                    RS.RUNE_FOUR  => RuneFourSelected,
                    RS.RUNE_FIVE  => RuneFiveSelected,
                    RS.RUNE_SIX   => RuneSixSelected,
                    RS.REROLL     => RuneRerollSelected,
                    _             => null
                };                    
            }
            if (buttonFunc != null)
            {
                Debug.Log(Time.time + " " + buttonFunc());
            }
        }
        
        private void DoHighlightChange() {
            if (dsm.state != currentState)
            {
                if (currentState == S.RSM)
                {
                    rpc.LoseFocus();
                }
                else
                {
                    rpc.GainFocus();
                }
            }

            currentState = dsm.state;
            if (currentState == S.RSM)
            {
                rpc.ChangeSelect((int)dsm.runeState);
            }
            else
            {
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

        /* All of these return an int because c# doesn't have function pointers */
        public string spellOneSelected()
        {
            return "Pressed spell ONE!";
        }

        public string spellTwoSelected()
        {
            return "Pressed spell TWO!";
        }

        public string spellThreeSelected()
        {
            return "Pressed spell THREE!";
        }

        public string spellFourSelected()
        {
            return "Pressed spell FOUR!";
        }

        public string submitSelected()
        {
            return "Pressed SUBMIT!";
        }

        public string runePanelSelected()
        {
            dsm.state = S.RSM;
            return "Pressed RUNE PANEL!";
        }

        public string RuneOneSelected()
        {
            return "Pressed RUNE ONE!";
        }

        public string RuneTwoSelected()
        {
            return "Pressed RUNE TWO!";
        }

        public string RuneThreeSelected()
        {
            return "Pressed RUNE THREE!";
        }

        public string RuneFourSelected()
        {
            return "Pressed RUNE FOUR!";
        }

        public string RuneFiveSelected()
        {
            return "Pressed RUNE FIVE!";
        }

        public string RuneSixSelected()
        {
            return "Pressed RUNE SIX!";
        }
        
        public string RuneRerollSelected()
        {
            return "Pressed REROLL";
        }
    }
}
