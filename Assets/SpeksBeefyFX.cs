using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeksBeefyFX : MonoBehaviour
{
    public float SparklesThreshold = 0.5f;

    public SongController songController;
    public ParticleSystem[] particleSystems;
    public ParticleSystem[] booshers;
    public ParticleSystem[] sparklers;

    private int lastBoosher = -1;
    private float booshCooldown = 0f;

    void FixedUpdate()
    {
        booshCooldown -= Time.deltaTime;
        if (songController.Beatiness > 0.5f /*&& songController.Peak > 1.5f*/ && booshCooldown < 0)
        {
            booshCooldown = 0.15f;
            int b;
            do
            {
                b = Random.Range(0, booshers.Length);
            } while (b == lastBoosher);
            booshers[b].Play();
            lastBoosher = b;
        }
        foreach (var ps in particleSystems)
        {
            ps.enableEmission = songController.BeefMode > 0;
        }

        foreach (var ps in sparklers)
        {
            ps.enableEmission = songController.BeefMode < 1 && songController.Shininess > SparklesThreshold;
        }
    }
}
