
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
    [SerializeField]
    private NotificationBox errorBox;
    [SerializeField]
    private NotificationBox successBox;

    [SerializeField]
    private int tests = 3;
    [SerializeField] 
    private int testLowerBound = -10;
    [SerializeField]
    private int testUpperBound = 10;
    [SerializeField]
    private Solution solution;

    public TerminalAccess terminalAccess;
    public delegate void FuntionsList();
    public TextMeshProUGUI inputDisplay, outputDisplay;
    List<FunctionBlock> sequence; //list of functions (type Functions_). The code sequence is read from here
    private int isPlaying;
    public FunctionBlock headBlock;

    MainFunction loop1;
    [SerializeField]
    private int correctTests;
    private int completedTests = 0;
    private int testValue;

    /*ONLY FOR DEMO*/
    [SerializeField]
    private GameObject unlockableBlock;

    void Start()
    {
        CodeBlockManager.Instance.codeBlockErrorEvent.AddListener(OpenErrorBox);
        solution = GetComponent<Solution>();
        isPlaying = 0;
        sequence = new List<FunctionBlock>();

        /*ONLY FOR DEMO*/
        if (CodeBlockManager.Instance.blockUnlocked)
            unlockableBlock.SetActive(true);
    }

    public void Paly()
    {
        successBox.gameObject.SetActive(false);
        errorBox.gameObject.SetActive(false);
        correctTests = 0;
        completedTests = 0;
        sequence.Clear();
        sequence = TranslateCodeFromBlocks(headBlock, sequence);

        RunNewFunction();           

        isPlaying = 2; 
    }

    private void RunNewFunction()
    {
        testValue = Random.Range(testLowerBound, testUpperBound);
        inputDisplay.text = testValue.ToString();
        loop1 = new MainFunction(inputDisplay, outputDisplay, sequence, testValue);
        StartCoroutine(loop1.Play());        
    }

    private void Update()
    {     

        if (completedTests < tests && isPlaying == 2 && loop1.end)
        {
            completedTests++;

            if (outputDisplay.text == solution.Solve(testValue))
                correctTests++;

            if (correctTests == tests)
            {
                terminalAccess.puzzleSolved.Invoke();
                successBox.gameObject.SetActive(true);
                successBox.Text = "Access Granted!";
                isPlaying = 1;
                return;
            }

            if (completedTests == tests)
                isPlaying = 1;
            else
                RunNewFunction();
                
            
        }
    }

    /*public void Stop()
    {
        isPlaying = 1; 
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
    }*/

    //recursive parser function
    private List<FunctionBlock> TranslateCodeFromBlocks(FunctionBlock block, List<FunctionBlock> sequence_)
    {
        sequence_.Add(block);
        if (block.nextBlock != null)
            return TranslateCodeFromBlocks(block.nextBlock, sequence_);
        
        
        return sequence_;
    }

    public void OpenErrorBox(string message)
    {
        errorBox.gameObject.SetActive(true);
        errorBox.Text = message;
    }

    public void CloseTerminal()
    {
        terminalAccess.CloseTerminal();
    }
}

public class MainFunction
{
    public TextMeshProUGUI inputDisplay, outputDisplay;
    List<FunctionBlock> sequence_;
    public bool infiniteLoop;
    public bool end;
    public int inputValue;
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





