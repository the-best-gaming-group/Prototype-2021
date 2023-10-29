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
        public BattleSystem bs;
        public DialogStateMachine dsm = new();
        public S currentState = S.DSM;
        public RunePanelController rpc;
        public SpellController[] scs = new SpellController[4];
        public SubmitController sc;
        public SubmitController rc;
        private Hoverable previousSelect;
        public ResourceHandler resourceHandler;
        public readonly float buttonDelay = 0.35f;
        public float delayRemaining = 0f;
        public bool isEnabled = true;
        private LASTSELECTED lastSelected = BUTTONUP;
        private List<Func<string>> spellEffects = new();
        
        private Spell[] spells;
        void Start()
        {
            previousSelect = rpc;
            rpc.Show();
            Disable();
            resourceHandler = bs.resourceHandler;
            bs.RegisterPlayerTurnBeginListener(() => {
                spells = bs.spells;
                for (int i = 0; i < 4; i++)
                {
                    scs[i].SetCost(spells[i]);
                    scs[i].text.text = spells[i].name;
                }
                SetupNewRound();
                return "Setting up new round";
            });
            bs.RegisterPlayerTurnBeginListener(() => {Enable(); return "Enabled player turn";});
            bs.RegisterPlayerTurnEndListener(() => {Disable(); return "Disabled player turn";});
            /* All of these return a string because c# doesn't have function pointers */
        }

        void Awake()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!isEnabled)
            {
                return;
            }
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

            if (Input.GetKeyUp(KeyCode.Escape)
                || (Input.GetAxis("Horizontal") > 0
                    && dsm.state == S.RSM
                    && dsm.runeState == RS.REROLL)
                || (Input.GetAxis("Vertical") < 0)
                    && dsm.state == S.RSM
                    && (dsm.runeState == RS.RUNE_FOUR
                        || dsm.runeState == RS.RUNE_FIVE
                        || dsm.runeState == RS.RUNE_SIX
                        || dsm.runeState == RS.REROLL
                ))
            {
                dsm.state = S.DSM;
            }
            else if ((Input.GetAxis("Horizontal") < 0
                    || Input.GetAxis("Vertical") > 0)
                        && dsm.state == S.DSM
                        && dsm.dialogState == DS.RUNES
                    )
            {
                dsm.state = S.RSM;
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
                DoHighlightChange();
                return;
            }

            delayRemaining = buttonDelay;
            DoHighlightChange();
        }
        
        public void Enable()
        {
            isEnabled = true;
            transform.gameObject.SetActive(true);            
        }

        public void Disable()
        {
            isEnabled = false;
            transform.gameObject.SetActive(false);            
        }
        
        public void SetupNewRound()
        {
            resourceHandler.Initialize(null);
            rpc.DoReset(resourceHandler);
            ColorButtons();
        }

        private void DoButtonClick()
        {
            string result;
            if (dsm.state == S.DSM)
            {
                result = dsm.dialogState switch
                {
                    DS.RUNES       => RunePanelSelected(),
                    DS.SPELL_ONE   => OnSpellButton(0),
                    DS.SPELL_TWO   => OnSpellButton(1),
                    DS.SPELL_THREE => OnSpellButton(2),
                    DS.SPELL_FOUR  => OnSpellButton(3),
                    DS.SUBMIT      => SubmitSelected(),
                    DS.RESET       => ResetSelected(),
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
                    rpc.Show();
                }
                else
                {
                    rpc.GainFocus();
                    rpc.Hide();
                }
            }

            currentState = dsm.state;
            if (currentState == S.RSM)
            {
                rpc.ChangeSelect((int)dsm.runeState);
            }
            else
            {
                Hoverable sel = dsm.dialogState switch
                {
                    DS.RUNES       => rpc,
                    DS.SPELL_ONE   => scs[0],
                    DS.SPELL_TWO   => scs[1],
                    DS.SPELL_THREE => scs[2],
                    DS.SPELL_FOUR  => scs[3],
                    DS.SUBMIT      => sc,
                    DS.RESET       => rc,
                    _              => rpc
                };
                previousSelect.Hide();
                sel.Show();
                previousSelect = sel;
            }
        }

        /* All of these return a string because c# doesn't have function pointers */
        public string SubmitSelected()
        {
            if (sc.enabled)
            {
                foreach (var func in spellEffects)
                {
                    func();
                }
                spellEffects.Clear();
                bs.OnEndTurnButton();
            }
            return "Pressed SUBMIT!";
        }
        
        public string ResetSelected()
        {
            OnResetButton();
            return "Pressed RESET!";
        }

        public string RunePanelSelected()
        {
            dsm.state = S.RSM;
            return "Pressed RUNE PANEL!";
        }

        public string RuneSelected(int i)
        {
            if (resourceHandler.GetRuneTypes()[i] != USED)
            {
                rpc.ToggleRune(i);
            }
            return string.Format("Pressed RUNE {0}!", i+1);
        }

        public string RuneRerollSelected()
        {
            resourceHandler.Reroll(rpc.rerolls);
            rpc.DoReroll(resourceHandler);
            ColorButtons();
            return "Pressed REROLL";
        }
        
        public void OnResetButton()
        {
            resourceHandler.UncommitRunes();
            rpc.ColorRunes(resourceHandler);
            spellEffects.Clear();
            ColorButtons();
        }
        
        public string OnSpellButton(int i)
        {
            if (resourceHandler.CanCastSpell(spells[i]))
            {
                resourceHandler.CommitRunesForSpell(spells[i]);
                spellEffects.Add(spells[i].effect);
                rpc.ColorRunes(resourceHandler);
                ColorButtons();
            }
            return string.Format("Pressed spell {0}!", i+1);
        }
        
        public void ColorButtons()
        {
            for (int i = 0; i < 4; i++)
            {
                if (resourceHandler.CanCastSpell(spells[i]))
                {
                    scs[i].DoEnable();
                }
                else
                {
                    scs[i].DoDisable();
                }
            }
            if (rpc.HasRollsLeft && spellEffects.Count == 0)
            {
                sc.DoDisable();
            }
            else
            {
                sc.DoEnable();
            }

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
