using System.Collections;
using UnityEngine;

public class TargetBasic : MonoBehaviour
{
    public float globalHealth = 100f;
   
    private Animator _animator;
    private Collider[] hitboxes;
    private PlayerPoints points;

    [Header("Shooting range")]
    // Shooting range training mode
    public bool isTrainingMode;
    public float resetTime;

    private float baseHealth = 100f;

    [Header("Sounds")]
    public AudioClip upSound;
    public AudioClip downSound;
    public AudioClip awardSound;

    private AudioSource _aSource;

    void Awake()
    {
        _aSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        hitboxes = GetComponentsInChildren<BoxCollider>();
        points = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPoints>(); // find player
    }

    public void TakeDamageHead(float amount)
    {
        globalHealth -= amount * 2f;

        points.AwardLocationalDamage("head");

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageBody(float amount)
    {
        globalHealth -= amount * 1.5f;

        points.AwardLocationalDamage("body");

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageArms(float amount)
    {
        globalHealth -= amount;

        points.AwardLocationalDamage("limb");

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageLegs(float amount)
    {
        globalHealth -= amount;

        points.AwardLocationalDamage("limb");

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    void targetDowned()
    {
        _animator.SetBool("Downed", true);
        _aSource.clip = downSound;
        _aSource.PlayOneShot(_aSource.clip);

        foreach (Collider hitbox in hitboxes)
        {
            hitbox.enabled = false;
        }

        _aSource.clip = awardSound;
        _aSource.PlayOneShot(_aSource.clip);
        points.AwardElimination();

        if (isTrainingMode)
        {
            StartCoroutine(ResetTarget());
        }
    }

    void targetUp()
    {
        _animator.SetBool("Downed", false); // get target up
        _aSource.clip = upSound;
        _aSource.PlayOneShot(_aSource.clip);

        // some sort of audio queue

        foreach (Collider hitbox in hitboxes)
        {
            hitbox.enabled = true; // re-enable hitboxes
        }

        globalHealth = baseHealth;
    }

    IEnumerator ResetTarget()
    {
        yield return new WaitForSeconds(resetTime);

        targetUp();
    }
}
