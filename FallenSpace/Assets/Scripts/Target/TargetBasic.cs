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

    void Awake()
    {
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

        FindObjectOfType<AudioManager>().RandomizePitchAndPlay("targetDown");

        foreach (Collider hitbox in hitboxes)
        {
            hitbox.enabled = false;
        }

        points.AwardElimination();

        if (isTrainingMode)
        {
            StartCoroutine(ResetTarget());
        }
    }

    void targetUp()
    {
        _animator.SetBool("Downed", false); // get target up

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
