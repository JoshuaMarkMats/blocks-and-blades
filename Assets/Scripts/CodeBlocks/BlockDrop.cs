using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDrop : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    private Function_ function;
    protected Vector2 relativeDropPosition;

    protected virtual void Awake()
    {
        function = GetComponent<Function_>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //get current object and check if it's not a head type
        Transform currentObject = EventSystem.current.currentSelectedGameObject.transform;
        if (currentObject.CompareTag("HeadBlock"))
            return;
        //set object position
        currentObject.SetParent(transform, false);
        currentObject.localPosition = relativeDropPosition;
        //set next block to dropped object
        function.nextBlock = currentObject.GetComponent<Function_>();
        //set dropped object previous to this
        currentObject.GetComponent<Function_>().prevBlock = function;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

}
