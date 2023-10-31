using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] HashSet<InputType> inputs;
    void Start()
    {
        inputs = new HashSet<InputType>();
    }

    // Update is called once per frame
    void Update()
    {
        var newInputs = new HashSet<InputType>();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newInputs.Add(InputType.ONE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newInputs.Add(InputType.TWO);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newInputs.Add(InputType.THREE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            newInputs.Add(InputType.FOUR);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            newInputs.Add(InputType.JUMP);
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            newInputs.Add(InputType.LEFT_MOVE);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newInputs.Add(InputType.RIGHT_MOVE);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            newInputs.Add(InputType.SELECT);
        }

        inputs = newInputs;
    }
    
    [Serializable]
    public enum InputType {
        ONE,
        TWO,
        THREE,
        FOUR,
        SELECT,
        JUMP,
        LEFT_MOVE,
        RIGHT_MOVE,
        UP_MOVE,
        DOWN_MOVE
    }
        
    public HashSet<InputType> GetInputs()
    {
        return inputs;
    }
}
