using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowRandomizer : MonoBehaviour
{

    ParticleSystem m_particleSystem;
    public float m_drift = 0.01f;
    public float m_jitter = 0.5f;
    public Vector3 m_snowDirection = Vector3.zero;
    public float m_maxWindChangeTimer = 5f;
    float m_currentMaxWindChangeTimer = 0f;
    float m_currentWindChangeTimer = 0f;

    // Use this for initialization
    void Start ()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        UpdateCurrentMaxWindTimer();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_currentWindChangeTimer += Time.deltaTime;
        if (m_currentWindChangeTimer >= m_currentMaxWindChangeTimer)
        {
            m_currentWindChangeTimer = 0f;
            UpdateSnowDirection();
            UpdateCurrentMaxWindTimer();
            PushSnow(m_drift);
        }
	}

    void PushSnow(float drift)
    {
        if (m_snowDirection.magnitude != 0f)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[m_particleSystem.main.maxParticles];
            int numParticlesAlive = m_particleSystem.GetParticles(particles);
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].velocity += m_snowDirection * drift;
;
            }

            m_particleSystem.SetParticles(particles, numParticlesAlive);
        }
    }

    void UpdateCurrentMaxWindTimer()
    {
        m_currentMaxWindChangeTimer = Random.Range(0.1f, m_maxWindChangeTimer);
    }

    float RandomClamped()
    {
        return Random.Range(-1f, 1f);
    }

    void UpdateSnowDirection()
    {
        m_snowDirection += new Vector3(RandomClamped() * m_jitter, 0f, RandomClamped() * m_jitter);
        m_snowDirection.Normalize();
    }
}
