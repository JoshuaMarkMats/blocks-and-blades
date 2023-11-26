using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDeleteZone : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        //get current object, return if it's a head type
        Transform currentObject = CodeBlockManager.Instance.lastSelected;
        if (currentObject.CompareTag(CodeBlockManager.HEAD_BLOCK))
            return;

        //if it's a function block, delete the object
        if (currentObject.GetComponent<FunctionBlock>() != null)
            Destroy(currentObject.gameObject);

    }

}
