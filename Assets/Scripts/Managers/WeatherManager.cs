using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeatherManager : MonoBehaviourPun
{
    public static WeatherManager instance;


    [SerializeField]
    private List<Weather> weatherList;

    [SerializeField]
    private Weather currentWeather;

    [SerializeField]
    private float weatherDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Vector2 initialWeatherData = new Vector2(0, 12);
        ChangeWeather(initialWeatherData);

        WorldTime.instance.onHourPass.AddListener(OnHourPassed);
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            if (weatherDuration <= 0)
            {
                //currentWeather.WeatherParticle.Stop();
                ChangeWeather(GetNextWeather());
            }
            else
            {
                if(currentWeather.WeatherParticle != null)
                {
                    currentWeather.WeatherParticle.Play();
                }
                //Debug.LogError(currentWeather.name);
            }
        }
        else if(PhotonNetwork.IsMasterClient)
        {
            if (weatherDuration <= 0)
            {
                Vector2 weatherData = GetNextWeather();
                photonView.RPC("ChangeWeather", RpcTarget.AllBuffered, weatherData);
            }
            if (currentWeather.WeatherParticle != null)
            {
                currentWeather.WeatherParticle.Play();
            }
        }
        else
        {
            if (currentWeather.WeatherParticle != null)
            {
                currentWeather.WeatherParticle.Play();
            }
        }
    }

    public void OnHourPassed()
    {
        weatherDuration--;
    }

    private Vector2 GetNextWeather()
    {
        Vector2 weatherData = new Vector2();

        float randomPossibily = Random.Range(0, 101);
        for(int i = 0; i < weatherList.Count; i++)
        {
            Weather weather = weatherList[i];
            if(randomPossibily <= weather.Possibility)
            {
                weatherData.x = i;                 //weather index

                float randomDuration = Random.Range(0, weather.MaxDuration + 1);
                weatherData.y = randomDuration;    //weather duration
            }
        }
        return weatherData;
    }

    [PunRPC]
    public void ChangeWeather(Vector2 weatherData)
    {
        if(currentWeather.WeatherParticle != null)
        {
            currentWeather.WeatherParticle.Stop();
        }

        currentWeather = weatherList[(int)weatherData.x];

        weatherDuration = weatherData.y;
    }
}
