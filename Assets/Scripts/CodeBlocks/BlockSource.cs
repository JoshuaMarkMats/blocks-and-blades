using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSource : BlockDrag
{
    [SerializeField]
    private GameObject blockPrefab;
    private GameObject currentBlock;

    protected override void Awake()
    {
        //old awake stuff not relevant here
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        currentBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        currentBlock.transform.position = Input.mousePosition;// - diffPosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        currentBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        currentBlock = null;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentBlock = Instantiate(blockPrefab, canvas_.transform, true);

        EventSystem.current.SetSelectedGameObject(currentBlock);       
        //currentBlock.transform.SetParent(canvas_.transform);
        currentBlock.transform.SetAsLastSibling();

        CodeBlockManager.Instance.lastSelected = currentBlock.transform;
    }
}
