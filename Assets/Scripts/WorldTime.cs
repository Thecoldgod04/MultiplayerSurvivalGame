using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class WorldTime : MonoBehaviourPun
{
    public static WorldTime instance;

    [SerializeField]
    [Range(10, 300)]
    private float cycle;

    [SerializeField]
    private float currentTime;

    [SerializeField]
    private int dayPassed;

    private double startTime;
    ExitGames.Client.Photon.Hashtable customValue;

    public UnityEvent onHourPass;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined) return;

        if (PhotonNetwork.IsMasterClient)
        {
            customValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            customValue.Add("StartTime", startTime);
            //PhotonNetwork.oom.SetCustomProperties(CustomeValue);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        }

    }

    int hourBefore, hourAfter = 0;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            DoTimeCycleOffline();
        else
            DoTimeCycle();

        hourAfter = GetCurrentHour();
        if(hourBefore != hourAfter)
        {
            hourBefore = hourAfter;
            onHourPass.Invoke();
        }
    }

    private void DoTimeCycle()
    {
        if (currentTime >= cycle)
        {
            //Reset time
            photonView.RPC("UpdateCycleTime", RpcTarget.All);
        }
        else
        {
            currentTime = (float)(PhotonNetwork.Time - startTime);
        }
    }

    private void DoTimeCycleOffline()
    {
        if (currentTime >= cycle)
        {
            //Reset time
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public int GetCurrentHour()
    {
        int currentHour = (int) (currentTime / (cycle / 24));
        return currentHour;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetCycle()
    {
        return cycle;
    }

    [PunRPC]
    private void UpdateCycleTime()
    {
        startTime = PhotonNetwork.Time;
        currentTime = 0;
        dayPassed++;
    }
}
