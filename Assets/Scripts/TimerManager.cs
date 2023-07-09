using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [field: SerializeField]
    public List<Timer> timers { get; private set; }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            bool time = timers[i].DoTimer();
            if (time == true)
            {
                timers[i].signal = true;
                timers.RemoveAt(i);
            }
        }
    }

    public void RegisterTimer(Timer timer)
    {
        timers.Add(timer);
    }
}

[System.Serializable]
public class Timer
{
    public float coolDown;
    public bool signal;

    public Timer(float coolDown)
    {
        this.coolDown = coolDown;
        TimerManager.instance.RegisterTimer(this);
    }

    public bool DoTimer()
    {
        coolDown -= Time.deltaTime;
        if (coolDown <= 0)
            return true;
        return false;
    }
}
