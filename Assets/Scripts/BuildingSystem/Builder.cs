using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Builder : MonoBehaviourPun
{
    [field: SerializeField]
    public BuildableMeta currentBuildable { get; private set; }

    [field: SerializeField]
    public static float buildRange { get; private set; }

    [field: SerializeField]
    private ConstructionLayer constructionLayer;

    [SerializeField]
    public Transform buildCursor { get; private set; }

    [field: SerializeField]
    public float buildTime { get; private set; }

    [SerializeField]
    private Timer buildTimer = null;


    private void Start()
    {
        buildRange = 3;

        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
           photonView.IsMine == true)
        {
            buildCursor = GameObject.FindGameObjectWithTag("BuildCursor").transform;
        }
        constructionLayer = FindObjectOfType<ConstructionLayer>();
    }

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
           photonView.IsMine == true)
        {
            UpdateBuildCursor();
            BuildInputCheck();
        }
    }

    private void UpdateBuildCursor()
    {
        // Deactivate/activate
        if (IsHoldingBuildable() == false)
        {
            buildCursor.gameObject.SetActive(false);
            return;
        }
        buildCursor.gameObject.SetActive(true);

        // Position update
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cursorCellPos = constructionLayer.tilemap.WorldToCell(mousePos);
        Vector3 cursorWorldPos = constructionLayer.tilemap.CellToWorld(cursorCellPos);
        buildCursor.position = new Vector3(cursorWorldPos.x + 0.5f, cursorWorldPos.y + 0.5f, cursorWorldPos.z);

        // Color update
        if (constructionLayer.IsEmpty(mousePos) && IsInRange(mousePos))
            buildCursor.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.34f);
        else
            buildCursor.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.34f);
    }

    bool cancel = false;
    private void BuildInputCheck()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (IsHoldingBuildable() == false || IsInRange(mousePos) == false)
        {
            CancelAction();
            return;
        }

        if(Input.GetMouseButtonDown(1)) //Right click
        {
            buildTimer = StartCooldown(buildTime);
            cancel = false;
        }
        if ((buildTimer != null && buildTimer.signal == false) || buildTimer == null || cancel == true) return;

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            RequestBuild(buildCursor.position, currentBuildable.GetId());
        else
            photonView.RPC("RequestBuild", RpcTarget.All, buildCursor.position, currentBuildable.GetId());
        buildTimer = null;
    }

    [PunRPC]
    private void RequestBuild(Vector3 pos, int buildableMetaId)
    {
        if(ItemMetaManager.instance.GetItemMetaById(buildableMetaId) == null)
        {
            Debug.LogError(transform.name + ": no BuildableMeta found with id:" + buildableMetaId);
        }
        BuildableMeta buildableMeta = (BuildableMeta)ItemMetaManager.instance.GetItemMetaById(buildableMetaId);
        //Debug.LogError(buildableMeta.GetId());
        constructionLayer.Build(pos, buildableMeta);

        //constructionLayer.Build(buildCursor.position, currentBuildable);
    }

    public void CancelAction()
    {
        cancel = true;
    }

    private bool IsHoldingBuildable()
    {
        if (!(PlayerInventoryController.itemInHand is BuildableMeta))
        {
            currentBuildable = null;
            return false;
        }
        currentBuildable = (BuildableMeta) PlayerInventoryController.itemInHand;
        return true;
    }

    public bool IsInRange(Vector3 mousePos)
    {
        //Debug.LogError(Vector3.Distance(transform.position, mousePos));
        return Vector2.Distance(transform.position, mousePos) <= buildRange;
    }

    private bool finishedCoolDown = false;
    private Timer StartCooldown(float coolDown)
    {
        Timer timer = new Timer(coolDown);
        return timer;
    }
}
