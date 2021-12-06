using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagickFX : MonoBehaviour
{
    public float SparklesThreshold = 0.5f;
    public SongController songController;
    public RunAroundScreen runAroundScreen;
    public RunAroundScreen runAroundScreen2;
    public RunAroundScreen runAroundScreen3;
    public RunAroundScreen runAroundScreen4;
    public RunAroundScreen runAroundScreen5;
    public RunAroundScreen runAroundScreen6;
    public ParticleSystem[] Beaters;
    public ParticleSystem[] Beefers;
    public ParticleSystem[] Sparklers;
    public ParticleSystem[] Shiners;
    private int _lastBeater = 0;
    private bool _stillOnBeat = false;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        runAroundScreen.distanceFromCamera = 10 - (songController.Loudness * 7);
        runAroundScreen2.distanceFromCamera = 10 - (songController.Loudness * 7);
        runAroundScreen3.distanceFromCamera = 7 - Mathf.Clamp(songController.Beefiness, 0, 3);
        runAroundScreen4.distanceFromCamera = 7 - Mathf.Clamp(songController.Beefiness, 0, 3);
        runAroundScreen5.distanceFromCamera = 7 - Mathf.Clamp(songController.Beefiness, 0, 3);
        runAroundScreen6.distanceFromCamera = 7 - Mathf.Clamp(songController.Beefiness, 0, 3);
        if (songController.Beatiness > 0 && !_stillOnBeat)
        {
            //runAroundScreen.ReverseDirection();
            runAroundScreen3.BumpPosition();
            runAroundScreen4.BumpPosition();
            runAroundScreen5.BumpPosition();
            runAroundScreen6.BumpPosition();
            _lastBeater = (_lastBeater + 1) % Beaters.Length;
            if (songController.Loudness > 0.5f)
            {
                foreach (var beater in Beaters)
                {
                    beater.Play();
                }
            } else
            {
                Beaters[_lastBeater].Play();
            }
            _stillOnBeat = true;
        } else if (songController.Beatiness < 1)
        {
            Beaters[_lastBeater].Stop();
            _stillOnBeat = false;
        }
        foreach (var ps in Beefers)
        {
            ps.enableEmission = songController.BeefMode > 0;
        }
        foreach (var ps in Sparklers)
        {
            ps.enableEmission = songController.BeefMode < 1 && songController.MidsSumWeighted > SparklesThreshold;
        }
        foreach (var ps in Sparklers)
        {
            ps.enableEmission = songController.Shininess > 0.8f;
        }
        if (songController.Peak > 0)
        {
            runAroundScreen.BumpPosition();
            runAroundScreen2.BumpPosition();
        }
    }
}
