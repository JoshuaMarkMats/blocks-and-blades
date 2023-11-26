
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
    List<FunctionBlock> sequence; //list of functions (type Functions_). The code sequence is read from here
    private int isPlaying;

    public FunctionBlock headBlock;

    MainFunction loop1;

    private void Awake()
    {
        //function = GetComponent<Function_>();
    }

    public void Paly()
    {
        sequence.Clear();
        sequence = TranslateCodeFromBlocks(headBlock, sequence);
        
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
        sequence = new List<FunctionBlock>();
    }
    
    void Update()
    {
        if (isPlaying == 2) //play
        {
            //no infinite loops... for now
            //loop1.infiniteLoop = transform.GetChild(1).GetComponent<Toggle>().isOn;
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
    private List<FunctionBlock> TranslateCodeFromBlocks(FunctionBlock block, List<FunctionBlock> sequence_)
    {
        /*foreach (Transform child in parent)
        {
            var functionName = child.name.Split('_'); //looks like a little face ^^

            if (functionName[0] == "Function")
            {
                string function = functionName[1];
                switch (function)
                {
                    case "Set":
                        sequence_.Add(child.GetComponent<SetFunction>());
                        break;
                    case "Out":
                        sequence_.Add(child.GetComponent<OutFunction>());
                        break;

                }
            }
        }*/
        //Debug.Log("adding item");
        sequence_.Add(block);
        //Debug.Log($"new sequence length is {sequence_.Count}");
        if (block.nextBlock != null)
            return TranslateCodeFromBlocks(block.nextBlock, sequence_);
        
        
        return sequence_;
    }
    
}

public class MainFunction
{
    public TextMeshProUGUI inputDisplay, outputDisplay;
    List<FunctionBlock> sequence_;
    public bool infiniteLoop;
    public bool end;
    public int inputValue = 10;
    private float waitTime;
    public Dictionary<string, object> variables = new();

    public Transform nextBlock;

    public MainFunction(TextMeshProUGUI inputDisplay, TextMeshProUGUI outputDisplay, List<FunctionBlock> sequence_, int inputValue)
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
        WaitForSeconds wait = new(waitTime);
        this.end = false;
        foreach (FunctionBlock fun in this.sequence_)
        {
            fun.Func(this);
            yield return wait;
        }
        this.end = true;
    }
    
}





