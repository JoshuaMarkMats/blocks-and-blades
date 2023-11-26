using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected FunctionBlock function;
    [SerializeField]
    protected Vector2 relativeDropPosition;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        function = gameObject.GetComponentInParent<FunctionBlock>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //get current object, return if it's a head type or if it isn't a FunctionBlock
        Transform currentObject = CodeBlockManager.Instance.lastSelected;
        if (currentObject.CompareTag(CodeBlockManager.HEAD_BLOCK))
            return;
        if (currentObject.GetComponent<FunctionBlock>() == null)
            return;

        //set object position and parent
        currentObject.SetParent(transform.parent, true);
        currentObject.localPosition= relativeDropPosition;
        
        //set next block to dropped object
        function.nextBlock = currentObject.GetComponent<FunctionBlock>();

        //set dropped object previous to this
        currentObject.GetComponent<FunctionBlock>().prevBlock = function;
        
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        //return if nothing is selected
        if (currentSelected == null)
            return;
        //check if selected is of type Function_
        if (currentSelected.GetComponent<FunctionBlock>() != null)
            canvasGroup.alpha = 0.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
    }

}
