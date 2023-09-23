namespace Platformer.Mechanics {
    using System;
    using static Platformer.Mechanics.DS;
    using static Platformer.Mechanics.RS;
    using static Platformer.Mechanics.BUTTON;
    using UnityEngine;

    [Serializable]
    public class DialogStateMachine
    {
        public S state = S.DSM;
        public DS dialogState = SPELL_ONE;
        public RS runeState = RUNE_ONE;
        private readonly DS[,] dsm = new DS [(int)DS.SIZE,4] {
            /* RUNES       = 0 */
            {RUNES, RUNES, SUBMIT, SPELL_ONE},
            /* SPELL_ONE   = 1 */
            {RUNES, SPELL_ONE, SPELL_TWO, SPELL_THREE},
            /* SPELL_TWO   = 2 */
            {SUBMIT, SPELL_ONE, SPELL_TWO, SPELL_FOUR},
            /* SPELL_THREE = 3 */
            {SPELL_ONE, SPELL_THREE, SPELL_FOUR, SPELL_THREE},
            /* SPELL_FOUR  = 4 */
            {SPELL_TWO, SPELL_THREE, SPELL_FOUR, SPELL_FOUR},
            /* SUBMIT      = 5 */
            {SUBMIT, RUNES, SUBMIT, SPELL_TWO}
        };
        private readonly RS[,] rsm = new RS [(int)RS.SIZE,2] {
            /* RUNE_ONE   = 0 */ 
            {RUNE_ONE,   RUNE_TWO},
            /* RUNE_TWO   = 1 */ 
            {RUNE_ONE,   RUNE_THREE},
            /* RUNE_THREE = 2 */ 
            {RUNE_TWO,   RUNE_FOUR},
            /* RUNE_FOUR  = 3 */ 
            {RUNE_THREE, RUNE_FIVE},
            /* RUNE_FIVE  = 4 */ 
            {RUNE_FOUR,  RUNE_SIX},
            /* RUNE_SIX   = 5 */ 
            {RUNE_FIVE,  REROLL},
            /* REROLL     = 6 */ 
            {RUNE_SIX,   REROLL}
        };
        
        public void RunStateMachine(BUTTON movement) {
            if (state == S.RSM) {
                RunRSM(movement);
            }
            else {
                RunDSM(movement);
            }
        }
        
        private void RunDSM(BUTTON movement) {
            if (movement == BACK) {
                return;
            }
            else if (movement == SELECT) {
                if (dialogState == RUNES) {
                    state = S.RSM;
                }
                else if (dialogState == SPELL_ONE) {
                   Debug.Log("Pressed spell ONE!");
                }
                else if (dialogState == SPELL_TWO) {
                   Debug.Log("Pressed spell TWO!");
                }
                else if (dialogState == SPELL_THREE) {
                   Debug.Log("Pressed spell THREE!");
                }
                else if (dialogState == SPELL_FOUR) {
                   Debug.Log("Pressed spell FOUR!");
                }
                else if (dialogState == SUBMIT) {
                    Debug.Log("Pressed SUBMIT!");
                }
                else {
                    Debug.LogError("DialogStateMachine.RunDSM(): Incorrect state pressed!");
                }
            }
            if (movement == SELECT) {
                return;
            }
            dialogState = dsm[(int)dialogState,(int)movement];
        }
        
        private void RunRSM(BUTTON movement) {
            if (movement == BACK) {
                state = S.DSM;
                return;
            }
            else if (movement == UP || movement == DOWN) {
                return;
            }
            else if (movement == SELECT) {
                if (runeState == RUNE_ONE) {
                    Debug.Log("Selected Rune ONE!");
                }
                else if (runeState == RUNE_TWO) {
                    Debug.Log("Selected Rune TWO!");
                }
                else if (runeState == RUNE_THREE) {
                    Debug.Log("Selected Rune THREE!");
                }
                else if (runeState == RUNE_FOUR) {
                    Debug.Log("Selected Rune FOUR!");
                }
                else if (runeState == RUNE_FIVE) {
                    Debug.Log("Selected Rune FIVE!");
                }
                else if (runeState == RUNE_SIX) {
                    Debug.Log("Selected Rune SIX!");
                }
                else if (runeState == REROLL) {
                    Debug.Log("Rerolling!");
                }
                else {
                    Debug.LogError("DialogStateMachine.RunRSM(): Incorrect state pressed!");
                }
            }
            if (movement == SELECT) {
                return;
            }
            runeState = rsm[(int)runeState,(int)movement-1];
        }
    }
    /** Dialog State */
    [Serializable]
    public enum DS {
        RUNES       = 0,
        SPELL_ONE   = 1,
        SPELL_TWO   = 2,
        SPELL_THREE = 3,
        SPELL_FOUR  = 4,
        SUBMIT      = 5,
        SIZE        = 6
    }
    /** Rune State */
    [Serializable]
    public enum RS {
        RUNE_ONE   = 0,
        RUNE_TWO   = 1,
        RUNE_THREE = 2,
        RUNE_FOUR  = 3,
        RUNE_FIVE  = 4,
        RUNE_SIX   = 5,
        REROLL     = 6,
        SIZE       = 7
    }
    /** State */
    [Serializable]
    public enum S {
        DSM = 0,
        RSM = 1
    }
    
    [Serializable]
    public enum BUTTON {
        UP     = 0,
        LEFT   = 1,
        RIGHT  = 2,
        DOWN   = 3,
        BACK   = 4,
        SELECT = 5
    }
}
