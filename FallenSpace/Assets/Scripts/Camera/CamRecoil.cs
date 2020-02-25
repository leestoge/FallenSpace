using UnityEngine;

public class CamRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float rotationSpeed;
    public float returnSpeed;
    [Space]

    [Header("Hipfire")]
    public Vector3 recoilRotationHipfire;
    [Space]

    [Header("Aiming")]
    public Vector3 recoilRotationAiming;
    [Space]

    [Header("State")]
    public bool isAiming;

    private Vector3 currentRot;
    private Vector3 Rot;


    void awake()
    {
    }
    void FixedUpdate()
    {
        currentRot = Vector3.Lerp(currentRot, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, currentRot, rotationSpeed * Time.fixedDeltaTime);
        gameObject.transform.localRotation = Quaternion.Euler(Rot);
    }

    public void Fire()
    {
        if (isAiming)
        {
            currentRot += new Vector3(-recoilRotationAiming.x, Random.Range(-recoilRotationAiming.y, recoilRotationAiming.y), Random.Range(-recoilRotationAiming.z, recoilRotationAiming.z));
        }
        else
        {
            currentRot += new Vector3(-recoilRotationHipfire.x, Random.Range(-recoilRotationHipfire.y, recoilRotationHipfire.y), Random.Range(-recoilRotationHipfire.z, recoilRotationHipfire.z));
        }
    }
}
