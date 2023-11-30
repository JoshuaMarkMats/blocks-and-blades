using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBlock : MonoBehaviour 
{
    public FunctionBlock nextBlock;
    public FunctionBlock prevBlock;
    
    //meat of the function called by controller
    public virtual void Func(MainFunction mainFunction)
    {

    }

    //used to update value blocks of a function
    public virtual void UpdateValues()
    {

    }

}