using UnityEngine;

public class ImpactParticleHandler : MonoBehaviour
{
    [Header("Particles")]
    [Space]
    public GameObject Wood;
    public GameObject Concrete;
    public GameObject Metal;
    public GameObject Enemy;

    private float rScale;

    // Caching what would otherwise be local variables
    #region Caching
    private GameObject cachePar;
    #endregion
    // ***********************************************

    void Awake()
    {
        rScale = Random.Range(0.5f, 1.0f);
    }

    public void CheckImpactLocation(RaycastHit location)
    {
        if (location.collider.CompareTag("Concrete"))
        {
            // concrete things
            cachePar = Instantiate(Concrete, location.point, Quaternion.LookRotation(location.normal));
            cachePar.transform.localPosition += .02f * location.normal;
            cachePar.transform.localScale = new Vector3(rScale, rScale, rScale);
            cachePar.transform.parent = location.transform;
        }
        else if (location.collider.CompareTag("Metal"))
        {
            // metal things
            cachePar = Instantiate(Metal, location.point, Quaternion.LookRotation(location.normal));
            cachePar.transform.localPosition += .02f * location.normal;
            cachePar.transform.localScale = new Vector3(rScale, rScale, rScale);
            cachePar.transform.parent = location.transform;
        }
        else if (location.collider.CompareTag("Enemy"))
        {
            // enemy things
            cachePar = Instantiate(Enemy, location.point, Quaternion.LookRotation(location.normal));
            cachePar.transform.localPosition += .02f * location.normal;
            cachePar.transform.localScale = new Vector3(rScale, rScale, rScale);
            cachePar.transform.parent = location.transform;
        }
    }
}
