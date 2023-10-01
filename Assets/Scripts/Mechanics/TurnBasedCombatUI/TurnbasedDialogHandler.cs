using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    using static Rune;
    public class TurnbasedDialogHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        public DialogStateMachine dsm = new();
        public S currentState = S.DSM;
        public readonly float buttonDelay = 0.3f;
        public float delayRemaining = 0f;
        public RunePanelController rpc;
        public SpellController[] scs = new SpellController[4];
        public SubmitController sc;
        private Selectable previousSelect;
        public ResourceHandler resourceHandler = new();
        private readonly bool[] rerolls = new bool[6];
        public Dictionary<Rune, Color> runeColorMap = new()
        {
            {WATER, Color.blue},
            {FIRE,  Color.red},
            {EARTH, Color.green},
            {AIR,   Color.yellow}
        };

        void Start()
        {
            previousSelect = rpc;
            rpc.Show();
            resourceHandler.Initialize(null);
            ColorRunes();
        }

        void Awake()
        {
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
            string result = null;
            if (dsm.state == S.DSM)
            {
                result = dsm.dialogState switch
                {
                    DS.RUNES       => RunePanelSelected(),
                    DS.SPELL_ONE   => SpellSelected(0),
                    DS.SPELL_TWO   => SpellSelected(1),
                    DS.SPELL_THREE => SpellSelected(2),
                    DS.SPELL_FOUR  => SpellSelected(3),
                    DS.SUBMIT      => SubmitSelected(),
                    _              => null
                };
            }
            else
            {
                result = dsm.runeState switch
                {
                    RS.RUNE_ONE   => RuneSelected(0),
                    RS.RUNE_TWO   => RuneSelected(1),
                    RS.RUNE_THREE => RuneSelected(2),
                    RS.RUNE_FOUR  => RuneSelected(3),
                    RS.RUNE_FIVE  => RuneSelected(4),
                    RS.RUNE_SIX   => RuneSelected(5),
                    RS.REROLL     => RuneRerollSelected(),
                    _             => null
                };                    
            }
            if (result != null)
            {
                Debug.Log(Time.time + " " + result);
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
                Selectable sel = dsm.dialogState switch
                {
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
        public string SpellSelected(int i)
        {
            return string.Format("Pressed spell {0}!", i+1);
        }
        public string SubmitSelected()
        {
            return "Pressed SUBMIT!";
        }

        public string RunePanelSelected()
        {
            dsm.state = S.RSM;
            return "Pressed RUNE PANEL!";
        }

        public string RuneSelected(int i)
        {
            rerolls[i] ^= true;
            return string.Format("Pressed RUNE {0}!", i+1);
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
