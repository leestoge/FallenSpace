using UnityEngine;

public class WepRecoil : MonoBehaviour
{
    [Header("Reference points")]
    public Transform recoilPosition;
    public Transform rotationPoint;
    [Space(10)]

    [Header("Speed Settings")]
    public float positionalRecoilSpeed;
    public float rotationalRecoilSpeed;
   
    [Space(10)]

    public float positionalReturnSpeed;
    public float rotationalReturnSpeed;

    [Header("Hipfire")]
    public Vector3 recoilRotationHipfire;
    public Vector3 recoilKickbackHipfire;
    [Space]

    [Header("Aiming")]
    public Vector3 recoilRotationAiming;
    public Vector3 recoilKickbackAiming;

    private Vector3 rotationalRecoil;
    private Vector3 positionalRecoil;
    private Vector3 Rot;

    [Header("State")]
    public bool isAiming;

    void Awake()
    {
        //hipfire
        //recoilRotationHipfire = new Vector3(10, 5, 7);
        //recoilRotationHipfire = new Vector3(0.015f, 0f, -0.2f);

        //aim
        //recoilRotationAiming = new Vector3(10, 4, 6);
        //recoilRotationAiming = new Vector3(0.015f, 0f, -0.2f);
    }

    void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);
    }

    public void Fire()
    {
        if (isAiming)
        {
            rotationalRecoil += new Vector3(-recoilRotationAiming.x, Random.Range(-recoilRotationAiming.y, recoilRotationAiming.y), Random.Range(-recoilRotationAiming.z, recoilRotationAiming.z));
            positionalRecoil += new Vector3(Random.Range(-recoilKickbackAiming.x, recoilKickbackAiming.x), Random.Range(-recoilKickbackAiming.y, recoilKickbackAiming.y), recoilKickbackAiming.z);
        }
        else
        {
            rotationalRecoil += new Vector3(-recoilRotationHipfire.x, Random.Range(-recoilRotationHipfire.y, recoilRotationHipfire.y), Random.Range(-recoilRotationHipfire.z, recoilRotationHipfire.z));
            positionalRecoil += new Vector3(Random.Range(-recoilKickbackHipfire.x, recoilKickbackHipfire.x), Random.Range(-recoilKickbackHipfire.y, recoilKickbackHipfire.y), recoilKickbackHipfire.z);
        }
    }
}
