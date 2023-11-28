using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Vector2 interactCenterOffset = Vector2.up;
    [SerializeField]
    private float interactRange = 3f;
    [SerializeField]
    private LayerMask interactMask;

    //interactable area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere((Vector2)transform.position + interactCenterOffset, interactRange);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnInteract()
    {
        Collider2D interactTarget = Physics2D.OverlapCircle((Vector2)transform.position + interactCenterOffset, interactRange, interactMask);

        //just making sure we hit an interactable object
        if (interactTarget.TryGetComponent<IInteractable>(out var interactable))
            interactable.OnInteract();
    }
}
