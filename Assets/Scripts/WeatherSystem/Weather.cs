using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class Weather
{
    public string name;

    [field: SerializeField]
    public float MaxDuration { get; private set; }

    [field: SerializeField]
    public float Possibility { get; private set; }

    [field: SerializeField]
    public ParticleSystem WeatherParticle { get; private set; }
}