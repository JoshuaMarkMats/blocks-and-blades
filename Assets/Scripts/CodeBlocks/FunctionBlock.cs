using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBlock : MonoBehaviour 
{
    public FunctionBlock nextBlock;
    public FunctionBlock prevBlock;

    /*public Function_ NextBlock
    {
        get { return nextBlock; }
        set
        {
            if (nextBlock != null)
                nextBlock.GetComponent<Function_>().PrevBlock = null;
            nextBlock = value;
        }
    }

    public Function_ PrevBlock
    {
        get { return prevBlock; }
        set
        {
            if (prevBlock != null)
                prevBlock.GetComponent<Function_>().NextBlock = null;
            prevBlock = value;
        }
    }*/
    
    //meat of the function called by controller
    public virtual void Func(MainFunction mainFunction)
    {

    }

    //used to update value blocks of a function
    public virtual void UpdateValues()
    {

    }

}