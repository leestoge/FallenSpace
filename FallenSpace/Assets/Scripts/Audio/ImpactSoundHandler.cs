using System.Collections;
using TMPro;
using UnityEngine;

public class ImpactSoundHandler : MonoBehaviour
{
    private AudioSource _source;
    private Random rnd;

    public float playAfter;

    [Header("Metal")]
    public bool isMetal;
    public AudioClip[] metalImpactSounds;  
    [Header("Concrete")]
    public bool isConcrete;
    public AudioClip[] concreteImpactSounds;
    [Header("Wood")]
    public bool isWood;
    public AudioClip[] woodImpactSounds;
    [Header("Enemy")]
    public bool isEnemy;
    public AudioClip enemyImpact;


    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (isMetal)
        {
            _source.clip = metalImpactSounds[Random.Range(0, metalImpactSounds.Length)];
            StartCoroutine(ExecuteAfterTime(playAfter));
           
        }
        else if (isConcrete)
        {
            _source.clip = concreteImpactSounds[Random.Range(0, concreteImpactSounds.Length)];
            StartCoroutine(ExecuteAfterTime(playAfter));
        }
        else if (isWood)
        {
            _source.clip = woodImpactSounds[Random.Range(0, woodImpactSounds.Length)];
            StartCoroutine(ExecuteAfterTime(playAfter));
        }
        else if (isEnemy)
        {
            _source.pitch = (Random.Range(0.95f, 1.05f));
            _source.clip = enemyImpact;
            StartCoroutine(ExecuteAfterTime(playAfter));
        }
        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        _source.PlayOneShot(_source.clip);
    }
}
