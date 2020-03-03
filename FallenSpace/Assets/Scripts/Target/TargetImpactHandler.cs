using UnityEngine;

public class TargetImpactHandler : MonoBehaviour
{
    private TargetBasic target;

    public void HandleImpact(RaycastHit location, float damage)
    {
        target = location.transform.GetComponentInParent<TargetBasic>();

        if (target != null)
        {
            if (location.transform.name.Contains("HeadHitbox"))
            {
                target.TakeDamageHead(damage);
            }
            else if (location.transform.name.Contains("BodyHitbox"))
            {
                target.TakeDamageBody(damage);
            }
            else if (location.transform.name.StartsWith("_Minor"))
            {
                target.TakeDamageArms(damage);
            }
        }
    }
}
