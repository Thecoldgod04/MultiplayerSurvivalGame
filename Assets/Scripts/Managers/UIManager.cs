using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private bool isUsingUI;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUsingUI()
    {
        return isUsingUI;
    }

    public void IsUsingUI(bool isUsingUI)
    {
        this.isUsingUI = isUsingUI;
    }
}
