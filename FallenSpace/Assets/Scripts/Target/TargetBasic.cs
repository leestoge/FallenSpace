using UnityEngine;

public class TargetBasic : MonoBehaviour
{
    public float globalHealth = 100f;
    private Animator _animator;
    private Collider[] hitboxes;
    private PlayerPoints points;

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

    void targetDowned() // points determined by hit location ^^^^^^^^
    {
        _animator.SetBool("Downed", true);

        FindObjectOfType<AudioManager>().RandomizePitchAndPlay("targetDown");

        foreach (Collider hitbox in hitboxes)
        {
            hitbox.enabled = false;
        }

        points.AwardElimination();
    }
}
