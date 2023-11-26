using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetFunction : FunctionBlock
{
    //public string varName = "Input";
    //public int value = 0;

    [SerializeField]
    private TMP_InputField varName;
    [SerializeField]
    private TMP_InputField value;

    override public void Func(MainFunction mainFunction)
    {
        //Debug.Log($"Setting variable {varName} to {value}");
        if (mainFunction.variables.ContainsKey(varName.text))
        {
            Debug.Log($"Setting {varName.text} to {value.text}");
            mainFunction.variables[varName.text] = value.text;
        }
        else
        {
            Debug.Log($"Setting new variable {varName.text} to {value.text}");
            mainFunction.variables.Add(varName.text, value.text);
        }

    }

    public override void UpdateValues()
    {
        
    }
}