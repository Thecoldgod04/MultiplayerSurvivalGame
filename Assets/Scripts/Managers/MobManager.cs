using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MobManager : MonoBehaviourPun
{
    public static MobManager instance;

    [field: SerializeField]
    public List<Mob> MobList { get; private set; }

    [field: SerializeField]
    public float DespawnDistance { get; private set; }

    [SerializeField]
    private int mobCap;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
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

    public bool ReachedMobCap()
    {
        return MobList.Count >= mobCap;
    }

    [PunRPC]
    public void AddMobToListRPC(int viewId)
    {
        for (int i = 0; i < MobList.Count; i++)
        {
            Mob mob = MobList[i];
            if (mob.photonView.ViewID == viewId)
            {
                MobList.Add(mob);
            }
        }
    }

    [PunRPC]
    public void RemoveFromMobListRPC(int viewId)
    {
        for(int i = 0; i < MobList.Count; i++)
        {
            Mob mob = MobList[i];
            if (mob.photonView.ViewID == viewId)
            {
                MobList.Remove(mob);
            }
        }
    }
}
