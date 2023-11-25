using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler
{
    Vector3 startPosition;
    Vector3 diffPosition;
    GameObject canvas_;

    private Function_ function;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        function = GetComponent<Function_>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (function.prevBlock != null)
            function.prevBlock.nextBlock = null;

        function.prevBlock = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition - diffPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts=true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = transform.position;
        diffPosition = Input.mousePosition - startPosition;
        EventSystem.current.SetSelectedGameObject(gameObject);
        EventSystem.current.currentSelectedGameObject.transform.SetParent(canvas_.transform);
        Debug.Log("start drag " + gameObject.name);
    }

    void Start()
    {
        canvas_ = GameObject.Find("Canvas");
    }

    void Update()
    {
    
    }

}
