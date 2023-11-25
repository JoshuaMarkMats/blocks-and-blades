using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFunction : Function_
{
    public string varName = "Input";
    public int value = 0;

    override public void Func(MainFunction mainFunction)
    {
        Debug.Log($"Setting variable {varName} to {value}");
        if (mainFunction.variables.ContainsKey(varName))
        {
            mainFunction.variables[varName] = value;
        }
        else
        {
            mainFunction.variables.Add(varName, value);
        }

    }
}