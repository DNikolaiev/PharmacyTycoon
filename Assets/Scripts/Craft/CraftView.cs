using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftView : MonoBehaviour {
    public ParticleSystem badMatch;
    public ParticleSystem okMatch;
    public ParticleSystem goodMatch;

    public void ReflectMatch(int matchCounter)
    {
        switch (matchCounter)
        {
           
            case '1':
                {
                    DisableAllParticles();
                    okMatch.gameObject.SetActive(true);
                    okMatch.Play();
                }
                break;
            case '2':
                {
                    DisableAllParticles();
                    okMatch.gameObject.SetActive(true);
                    okMatch.Play();
                }
                break;
            case '3':
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                }
                break;
            case '4':
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                }
                break;
            case '5':
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                }
                break;
            default:
                {
                    Debug.Log("def");
                    DisableAllParticles();
                    badMatch.gameObject.SetActive(true);
                    badMatch.Play();
                }
                break;
        }
    }
	public void DisableAllParticles()
    {
        ParticleSystem[] particles = { badMatch, okMatch, goodMatch };
        foreach(ParticleSystem p in particles)
        {
            p.Stop();
            p.gameObject.SetActive(false);
        }
    }
}
