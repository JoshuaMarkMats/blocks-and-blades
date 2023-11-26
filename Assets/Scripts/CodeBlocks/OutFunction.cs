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
            Debug.Log($"Printing {varName.text}");
            mainFunction.outputDisplay.text = mainFunction.variables[varName.text].ToString();
        }
        else
        {
            Debug.Log($"Variable {varName.text} not found");
            CodeBlockManager.Instance.codeBlockErrorEvent.Invoke(CodeBlockManager.CodeBlockErrorType.VARIABLE_NOT_FOUND);
        }
            
    }

}
