using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutFunction : Function_
{
    public string varName = "Input";
    public override void Func(MainFunction mainFunction)
    {
        Debug.Log($"Printing {varName}");
        if (mainFunction.variables.ContainsKey(varName))
            mainFunction.outputDisplay.text = mainFunction.variables[varName].ToString();
    }

}
