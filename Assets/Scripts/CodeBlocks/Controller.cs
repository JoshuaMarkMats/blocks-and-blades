
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//The code implements a controller for a sequence of functions using the theory of machine language and
// its way to arrange functions to be run in a organized sequence. It run thought a list and base on the function list.
// The objective is to induce the player to use a Drag and Drop Visual programming interface to write its own code to
// interact with the game or parts of the game.
// This is a sample of a bigger project being developed with loops/conditions, functions with variables and a simple
// textual programming language enable the translation between Visual (blocks) and Textual coding.
// 
//#Visual Block Coding:
// drag and drop from (https://github.com/danielcmcg/Unity-UI-Nested-Drag-and-Drop) functinos to the desired positions
// on the main loop of the object

public class Controller : MonoBehaviour
{
    public delegate void FuntionsList();
    public TextMeshProUGUI inputDisplay, outputDisplay;
    List<Function_> sequence; //list of functions (type Functions_). The code sequence is read from here
    private int isPlaying;

    MainFunction loop1;
    
    public void Paly()
    {
        sequence.Clear();
        sequence = TranslateCodeFromBlocks(transform.parent, sequence);
        
        loop1 = new MainFunction(inputDisplay, outputDisplay, sequence, 5);
        StartCoroutine(loop1.Play());

        isPlaying = 2; 
    }

    public void Stop()
    {
        isPlaying = 1; 
    }

    void Start()
    {
        isPlaying = 0; 
        sequence = new List<Function_>();
    }
    
    void Update()
    {
        if (isPlaying == 2) //play
        {
            loop1.infiniteLoop = transform.GetChild(1).GetComponent<Toggle>().isOn;
            if (loop1.infiniteLoop && loop1.end)
            {
                StartCoroutine(loop1.Play());
            }
        }
        if (isPlaying == 1) //stop
        {
            StopCoroutine(loop1.Play());
        }
    }
    
    //recursive parser function
    private List<Function_> TranslateCodeFromBlocks(Transform parent, List<Function_> sequence_)
    {
        foreach (Transform child in parent)
        {
            var functionName = child.name.Split('_'); //looks like a little face ^^

            if (functionName[0] == "Function")
            {
                string function = functionName[1];
                switch (function)
                {
                    case "Set":
                        var setData = child.GetComponent<SetBlockData>();
                        sequence_.Add(new SetFunction("SetFunction", setData.varName, setData.value));
                        break;
                    case "Out":
                        var outData = child.GetComponent<OutBlockData>();
                        sequence_.Add(new OutFunction("OutFunction", outData.varName));
                        break;

                }
            }
        }
        
        return sequence_;
    }
    
}

public class MainFunction
{
    public TextMeshProUGUI inputDisplay, outputDisplay;
    List<Function_> sequence_;
    public bool infiniteLoop;
    public bool end;
    public int inputValue = 10;
    private float waitTime;
    public Dictionary<string, object> variables = new();

    public int OutputDisplay
    {
        set
        {
            int valueToSet = value;
        }
    }

    public MainFunction(TextMeshProUGUI inputDisplay, TextMeshProUGUI outputDisplay, List<Function_> sequence_, int inputValue)
    {
        this.end = false;
        this.inputDisplay = inputDisplay;
        this.outputDisplay = outputDisplay;
        this.sequence_ = sequence_;
        this.waitTime = 0.5f; //wait time between functions in sequence (list)
        variables.Add("Input", inputValue);
    }
    public IEnumerator Play()
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        this.end = false;
        foreach (Function_ fun in this.sequence_)
        {
            fun.Func(this);
            yield return wait;
        }
        this.end = true;
    }
    
}

public class SetFunction : Function_
{
    private string variable;
    private int value;

    public SetFunction(string ID, string variable, int value) : base(ID)
    {
        this.variable = variable;
        this.value = value;
        this.ID = ID;
    }

    override public void Func(MainFunction mainFunction)
    {
        if (mainFunction.variables.ContainsKey(variable))
        {
            mainFunction.variables[variable] = value;
        }
        else
        {
            mainFunction.variables.Add(variable, value);
        }

    }
}

public class OutFunction : Function_
{
    private string varName;

    public OutFunction(string ID, string varName) : base(ID)
    {
        this.ID = ID;
        this.varName = varName;
    }

    public override void Func(MainFunction mainFunction)
    {
        if (mainFunction.variables.ContainsKey(varName))
            mainFunction.outputDisplay.text = mainFunction.inputValue.ToString();
    }

}

public class Function_
{
    public string ID;

    //contructor for sinple functions
    public Function_(string ID)
    {
        this.ID = ID;
    }

    public virtual void Func(MainFunction mainFunction)
    {
       
    }

}