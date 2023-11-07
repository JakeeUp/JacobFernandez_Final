using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer 
{
    public float bufferTime = 0.5f; 
    private float lastInputTime = float.NegativeInfinity; 
    private string lastInput = "";

    public void BufferInput(string input)
    {
        lastInput = input;
        lastInputTime = Time.time;
    }

    public bool HasInput(string input)
    {
        if (lastInput == input && (Time.time - lastInputTime) <= bufferTime)
        {
            ClearInput(); 
            return true;
        }
        return false;
    }

   
    public void ClearInput()
    {
        lastInput = "";
        lastInputTime = float.NegativeInfinity;
    }
}
