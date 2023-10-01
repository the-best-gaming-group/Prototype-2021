using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    using static Rune;
    using static LASTSELECTED;
    public class TurnbasedDialogHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        public DialogStateMachine dsm = new();
        public S currentState = S.DSM;
        public RunePanelController rpc;
        public SpellController[] scs = new SpellController[4];
        public SubmitController sc;
        private Selectable previousSelect;
        public ResourceHandler resourceHandler = new();
        public readonly float buttonDelay = 0.3f;
        public float delayRemaining = 0f;
        private readonly bool[] rerolls = new bool[6];
        private LASTSELECTED lastSelected = BUTTONUP;
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
            if (Input.GetButtonUp("Jump"))
            {
                DoButtonClick();
            }

            if (delayRemaining > 0f)
            {
                delayRemaining -= Time.deltaTime;
                return;
            }
            else
            {
                lastSelected = BUTTONUP;
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                dsm.state = S.DSM;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                if (lastSelected == RIGHT)
                {
                    return;
                }
                dsm.RunStateMachine(BUTTON.RIGHT);
                lastSelected = RIGHT;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (lastSelected == LEFT)
                {
                    return;
                }
                dsm.RunStateMachine(BUTTON.LEFT);
                lastSelected = LEFT;
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                if (lastSelected == UP)
                {
                    return;
                }
                dsm.RunStateMachine(BUTTON.UP);
                lastSelected = UP;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (lastSelected == DOWN)
                {
                    return;
                }
                dsm.RunStateMachine(BUTTON.DOWN);
                lastSelected = DOWN;
            }
            else
            {
                lastSelected = BUTTONUP;
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
    
    enum LASTSELECTED {
        UP = 0,
        LEFT = 1,
        RIGHT = 2,
        DOWN = 3,
        BUTTONUP = 4
    }
}
