using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValueDropZone : BlockDropZone
{
    private CanvasGroup canvasGroup;
    public string heldValue;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        //get current object, return if it's not a ValueBlock
        Transform currentObject = CodeBlockManager.Instance.lastSelected;
        if (currentObject.GetComponent<ValueBlock>() == null)
            return;

        //set object position and parent
        currentObject.position = transform.position;
        currentObject.SetParent(transform.parent, true);

        heldValue = currentObject.GetComponent<ValueBlock>().value;
        //update function values
        function.UpdateValues();

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        //return if nothing is selected
        if (currentSelected == null)
            return;
        //check if selected is of type ValueBlock
        if (currentSelected.GetComponent<ValueBlock>() != null)
            canvasGroup.alpha = 0.5f;
    }

}
