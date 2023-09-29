using UnityEngine;

using static Rune;
public enum Rune
{
    WATER = 0,
    FIRE  = 1,
    EARTH = 2,
    AIR   = 3
}

public class Spell
{
    public string name;    
    public int[] cost = {0, 0, 0, 0};
}

public class ResourceHandler
{
    public Rune[] runes = new Rune[6];
    /* The below code is for example purposes */
    public Spell slam     = new Spell { name = "Slam" };
    public Spell fireBall = new Spell { name = "Fire Ball" };
    public Spell dodge    = new Spell { name = "Dodge" };
    public Spell heal     = new Spell { name = "Heal" };
    private int[] cumSum;

    public void Initialize(int[] chances)
    {
        if (chances == null)
        {
            chances = new int[]{25, 25, 25, 25};            
        }
    /* The below code is for example purposes */
        fireBall.cost[(int)FIRE] = 2;
        fireBall.cost[(int)AIR] = 1;
        
        slam.cost[(int)FIRE] = 1;
        slam.cost[(int)EARTH] = 1;
        
        dodge.cost[(int)WATER] = 2;
        dodge.cost[(int)AIR] = 1;
        
        heal.cost[(int)WATER] = 3;
        
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
}
