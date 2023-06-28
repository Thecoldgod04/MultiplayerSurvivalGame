using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpener : MonoBehaviour
{
    [SerializeField]
    private GameObject uiObject;

    [SerializeField]
    private KeyCode activationKey;

    [SerializeField]
    private bool beingUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (UIManager.instance.IsUsingUI() && beingUsed == false) return;

        if(Input.GetKeyDown(activationKey))
        {
            if(UIManager.instance.IsUsingUI() == true && beingUsed == true)  //The player is using this UI
            {
                CloseUI();
            }
            else if(UIManager.instance.IsUsingUI() == false && beingUsed == false)
            {
                OpenUI();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.instance.IsUsingUI() == true && beingUsed == true)  //The player is using this UI
            {
                CloseUI();
            }
        }
    }

    public void OpenUI()
    {
        uiObject.SetActive(true);
        UIManager.instance.IsUsingUI(true);
        beingUsed = true;
    }

    public void CloseUI()
    {
        uiObject.SetActive(false);
        UIManager.instance.IsUsingUI(false);
        beingUsed = false;
    }
}
