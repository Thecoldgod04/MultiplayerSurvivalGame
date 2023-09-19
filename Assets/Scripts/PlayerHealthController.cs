using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField]
    private Life playerLife;

    [SerializeField]
    private List<GameObject> hearts;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            playerLife = GameManager.instance.PlayerOwner.GetComponent<Life>();

            playerLife.onDamageTake.AddListener(UpdateHealthUI);
            playerLife.onHeal.AddListener(UpdateHealthUI);
        }
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI()
    {
        int currentHealth = playerLife.health;
        
        //Debug.LogError(currentHealth);

        for(int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }
}
