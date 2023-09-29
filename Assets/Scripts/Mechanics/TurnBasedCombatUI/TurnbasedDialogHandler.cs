using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    using static Rune;
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
        public ResourceHandler resourceHandler = new ResourceHandler();
        private bool[] rerolls = new bool[6];
        public Dictionary<Rune, Color> runeColorMap = new Dictionary<Rune, Color> {
            {WATER, Color.blue},
            {FIRE, Color.red},
            {EARTH, Color.green},
            {AIR, Color.yellow}
        };

        void Start()
        {
            previousSelect = rpc;
            rpc.Show();
            resourceHandler.Initialize(null);
            ColorRunes();
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
                    DS.RUNES       => rpc,
                    DS.SPELL_ONE   => scs[0],
                    DS.SPELL_TWO   => scs[1],
                    DS.SPELL_THREE => scs[2],
                    DS.SPELL_FOUR  => scs[3],
                    DS.SUBMIT      => sc,
                    _              => rpc
                };
                previousSelect.Hide();
                sel.Show();
                previousSelect = sel;
            }
        }
        
        private void WipeRerolls()
        {
            for (int i = 0; i < 6; i++)
            {
                rerolls[i] = false;
            }
        }
        
        private void ColorRunes()
        {
            for (int i = 0; i < 6; i++)
            {
                rpc.runes[i].ChangeColor(runeColorMap[resourceHandler.runes[i]]);
            }
        }

        /* All of these return a string because c# doesn't have function pointers */
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
            rerolls[0] ^= true;
            return "Pressed RUNE ONE!";
        }

        public string RuneTwoSelected()
        {
            rerolls[1] ^= true;
            return "Pressed RUNE TWO!";
        }

        public string RuneThreeSelected()
        {
            rerolls[2] ^= true;
            return "Pressed RUNE THREE!";
        }

        public string RuneFourSelected()
        {
            rerolls[3] ^= true;
            return "Pressed RUNE FOUR!";
        }

        public string RuneFiveSelected()
        {
            rerolls[4] ^= true;
            return "Pressed RUNE FIVE!";
        }

        public string RuneSixSelected()
        {
            rerolls[5] ^= true;
            return "Pressed RUNE SIX!";
        }
        
        public string RuneRerollSelected()
        {
            resourceHandler.Reroll(rerolls);
            ColorRunes();
            WipeRerolls();
            return "Pressed REROLL";
        }
    }
}
