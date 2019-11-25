using UnityEngine;

public class TargetMaster : MonoBehaviour
{
    public float globalHealth = 100f;
    private Animator _animator;
    private Collider[] hitboxes;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        hitboxes = GetComponentsInChildren<BoxCollider>();
    }

    public void TakeDamageHead(float amount)
    {
        globalHealth -= amount * 2f;

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageBody(float amount)
    {
        globalHealth -= amount * 1.5f;

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageArms(float amount)
    {
        globalHealth -= amount;

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    public void TakeDamageLegs(float amount)
    {
        globalHealth -= amount;

        if (globalHealth <= 0)
        {
            targetDowned();
        }
    }

    void targetDowned() // points determined by hit location ^^^^^^^
    {
        _animator.SetBool("Downed", true);

        foreach (Collider hitbox in hitboxes)
        {
            hitbox.enabled = false;
        }

        // add points
    }
}
