using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutFunction : FunctionBlock
{
    //public string varName = "Input";

    [SerializeField]
    private TMP_InputField varName;
    public override void Func(MainFunction mainFunction)
    {
        
        if (mainFunction.variables.ContainsKey(varName.text))
        {
            mainFunction.outputDisplay.text = mainFunction.variables[varName.text].ToString();
        }
        else
        {
            CodeBlockManager.Instance.codeBlockErrorEvent.Invoke($"{varName.text} does not exist.");
        }
            
    }

}
