using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public static MouseCursorManager instance;

    [SerializeField]
    private Texture2D defaultCursor, interactionCursor, breakingCursor, attackingCursor;

    private Vector2 cursorHotspot;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;

            SetCursor(defaultCursor);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCursor(Texture2D cursor)
    {
        cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);

        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetToInteractionCursor()
    {
        SetCursor(interactionCursor);
    }

    public void SetToBreakingCursor()
    {
        SetCursor(breakingCursor);
    }

    public void SetToAttackingCursor()
    {
        SetCursor(attackingCursor);
    }

    public void SetToDefaultCursor()
    {
        SetCursor(defaultCursor);
    }
}
