using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class OneShotParticles : MonoBehaviour
{
    private ParticleSystem particles;

    public void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (particles.IsAlive() == false)
        {
            Destroy(gameObject);
        }
    }
}
