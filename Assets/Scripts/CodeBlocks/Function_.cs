using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function_ : MonoBehaviour 
{
    public Function_ nextBlock;
    public Function_ prevBlock;

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

    public virtual void Func(MainFunction mainFunction)
    {

    }

}