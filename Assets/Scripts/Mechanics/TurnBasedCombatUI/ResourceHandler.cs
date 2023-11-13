using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameManager;
using static Rune;
public enum Rune
{
    WATER = 0,
    FIRE  = 1,
    EARTH = 2,
    AIR   = 3,
    USED = 4
}

public class ResourceHandler
{
    private Rune[] runes = new Rune[6];
    private readonly bool[] committed = new bool[6];
    /* The below code is for example purposes */
    private int[] cumSum;

    public void Initialize(int[] chances)
    {
        if (chances == null)
        {
            chances = new int[]{25, 25, 25, 25};            
        }
    /* The below code is for example purposes */
        
        int runningSum = 0;
        cumSum = new int[4];
        for (int i = 0; i < 4; i++)
        {
            cumSum[i] = runningSum += chances[i];
        }
        if (cumSum[3] != 100)
        {
            Debug.LogError("ERROR: Rune chances do not add to 1");
        }
        
        for (int i = 0; i < 6; i++)
        {
            RerollRune(i);
        }
        for (int i = 0; i < 6; i++)
        {
            committed[i] = false;
        }
    }
    public void Reroll(bool[] roll)
    {
        for (int i = 0; i < 6; i++)
        {
            if (roll[i])
            {
                RerollRune(i);
            }
        }
    }
    
    private void RerollRune(int idx)
    {
        var chance = Random.Range(0, 100);
        if (chance < cumSum[0])
        {
            runes[idx] = WATER;
        }
        else if (chance < cumSum[1])
        {
            runes[idx] = FIRE;
        }
        else if (chance < cumSum[2])
        {
            runes[idx] = EARTH;
        }
        else
        {
            runes[idx] = AIR;
        }
    }
    
    public bool CanCastSpell(Spell spell)
    {
        var totalResources = new int[4];
        for (int i = 0; i < 6; i++)
        {
            if (!committed[i])
            {
                totalResources[(int)runes[i]]++;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (totalResources[i] < spell.cost[i])
            {
                return false;
            }
        }
        return true;
    }
    
    public void CommitRunesForSpell(Spell s)
    {
        int[] costLeft = new int[4];
        Array.Copy(s.cost, costLeft, 4);
        for (int i = 0; i < 6; i++)
        {
            if (!committed[i] && costLeft[(int)runes[i]]-- > 0)
            {
                committed[i] = true;
            }
        }
    }

    public void UncommitRunes()
    {
        for (int i = 0; i < 6; i++)
        {
            committed[i] = false;
        }
    }
    
    public Rune[] GetRuneTypes()
    {
        var runeTypes = new Rune[6];
        for (int i = 0; i < 6; i++)
        {
            if (committed[i])
            {
                runeTypes[i] = USED;
            }
            else
            {
                runeTypes[i] = runes[i];
            }
        }
        return runeTypes;
    }
}
