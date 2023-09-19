using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ItemStack : MonoBehaviourPun
{
    [SerializeField]
    private ItemMeta itemMeta;

    [SerializeField]
    [Range(1, 12)]
    private int amount;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody2D rb;


    [SerializeField]
    private PlayerInventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindObjectOfType<PlayerInventoryController>();

        spriteRenderer.sprite = itemMeta.GetSprite();
        rb = GetComponent<Rigidbody2D>();

        /*float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        Splash(x, y);
        rb.velocity = velocity * splashSpeed * Time.fixedDeltaTime;*/
    }

    private void Update()
    {
        SplashTimeCountdown();
    }

    float slowDownSpeed = 10f;
    private void FixedUpdate()
    {
        //rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, slowDownSpeed * Time.fixedDeltaTime);
    }

    float splashTime = 0.3f;
    public void SplashTimeCountdown()
    {
        splashTime -= Time.deltaTime;
        if(splashTime <= 0)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }

    float splashSpeed = 700f;
    Vector2 velocity;
    public void Splash(float x, float y)
    {
        velocity = new Vector2(x, y);
        velocity.Normalize();

        slowDownSpeed = Random.Range(8f, 10f);
    }

    public void SetItemMeta(ItemMeta itemMeta)
    {
        this.itemMeta = itemMeta;
        spriteRenderer.sprite = itemMeta.GetSprite();
    }
    [PunRPC]
    public void SetItemMeta(int itemMetaId)
    {
        ItemMeta itemMeta = ItemMetaManager.instance.GetItemMetaById(itemMetaId);
        SetItemMeta(itemMeta);
    }

    public ItemMeta GetItemMeta()
    {
        return itemMeta;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }

    public int GetAmount()
    {
        return amount;
    }

    //Event firing
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.velocity.x > 0.5 || rb.velocity.y > 0.5) return;

        bool collectionSuccess = false;

        if(collision.CompareTag("Player"))
        {
            if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
            {
                if (collision.GetComponent<PhotonView>().IsMine == true)
                {
                    collectionSuccess = inventoryController.OnItemCollected(this);
                }
            }
            else
            {
                collectionSuccess = inventoryController.OnItemCollected(this);
            }

            if(collectionSuccess)
                Destroy(this.gameObject);
        }
    }
}
