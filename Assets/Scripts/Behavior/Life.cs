using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Life : MonoBehaviourPun
{
    [field: SerializeField]
    public int health { get; private set; }

    [field: SerializeField]
    public int maxHealth { get; private set; }

    [field: SerializeField]
    public string triggerTag { get; private set; }

    [field: SerializeField]
    public ILife lifeBehavior { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
          photonView.IsMine == true)
        {
            lifeBehavior = GetComponent<ILife>();

            triggerTag = lifeBehavior.triggerDamageTag;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
           photonView.IsMine == true)
        {
            if (UIManager.instance.IsUsingUI()) return;
            Heal();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Die();
        }    
    }

    public void Heal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += lifeBehavior.Heal();
        }
    }

    [PunRPC]
    public void Die()
    {
        Debug.LogError(transform.name + ": died");
        PhotonNetwork.Destroy(this.gameObject);
    }

    // Event handling
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
           photonView.IsMine == true)
        {
            if (collision.CompareTag(triggerTag))
            {
                Debug.LogError("Ouch");
                int damageTaken = collision.GetComponent<Damage>().damage;
                TakeDamage(damageTaken);
            }
        }
    }
}
