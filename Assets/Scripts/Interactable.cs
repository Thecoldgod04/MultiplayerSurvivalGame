using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Interactable : MonoBehaviour
{
    public UnityEvent onRightClick, onLeftClick;

    private bool isHovering = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHovering) return;
        if (UIManager.instance.IsUsingUI()) return;

        if(Input.GetMouseButtonDown(1))
        {
            onRightClick.Invoke();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            onLeftClick.Invoke();
        }
    }

    public void SetIsHovering(bool isHovering)
    {
        this.isHovering = isHovering;

        if(isHovering == true)
        {
            MouseCursorManager.instance.SetToInteractionCursor();
        }
        else
        {
            MouseCursorManager.instance.SetToDefaultCursor();
        }
    }

    private void OnMouseEnter()
    {
        if (UIManager.instance.IsUsingUI()) return;

        /*isHovering = true;
        MouseCursorManager.instance.SetToInteractionCursor();*/
        SetIsHovering(true);
    }
    private void OnMouseExit()
    {
        if (UIManager.instance.IsUsingUI()) return;

        /*isHovering = false;
        MouseCursorManager.instance.SetToDefaultCursor();*/
        SetIsHovering(false);
    }
}
