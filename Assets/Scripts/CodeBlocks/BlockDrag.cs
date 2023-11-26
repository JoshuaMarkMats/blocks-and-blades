using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IBeginDragHandler, IPointerUpHandler
{
    protected GameObject canvas_;

    private FunctionBlock function;
    private CanvasGroup canvasGroup;

    public bool isOutOfBounds = false;

    protected virtual void Awake()
    {
        function = GetComponent<FunctionBlock>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (function.prevBlock != null)
            function.prevBlock.nextBlock = null;

        function.prevBlock = null;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts=true;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);       
        transform.SetParent(canvas_.transform);
        transform.SetAsLastSibling(); //we want this on rendered on top so put it at the bottom of list

        //separate tracker for current object
        CodeBlockManager.Instance.lastSelected = transform;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Start()
    {
        canvas_ = GameObject.Find("Terminal");
    }

}
