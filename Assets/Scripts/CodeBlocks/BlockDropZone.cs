using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Function_ function;
    [SerializeField]
    protected Vector2 relativeDropPosition;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //get current object and check if it's not a head type
        Transform currentObject = DragDropManager.Instance.lastSelected;
        if (currentObject.CompareTag("HeadBlock"))
            return;
        //set object position and parent
        currentObject.SetParent(transform.parent, false);
        currentObject.localPosition = relativeDropPosition;      
        //set next block to dropped object
        function.nextBlock = currentObject.GetComponent<Function_>();
        //set dropped object previous to this
        currentObject.GetComponent<Function_>().prevBlock = function;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            canvasGroup.alpha = 0.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
    }

}
