using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 6;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public Animator gunAnimator;
    public Transform gunModel;

    [Space]
    public float punchStrenght = .2f;
    public int punchVibrato = 5;
    public float punchDuration = .3f;
    [Range(0, 1)]
    public float punchElasticity = .5f;

    // particles ***************
    public ParticleSystem Muzzleflash;
    public GameObject laserShot;
    public GameObject Smoke;
    private ParticleSystem Steam;
    public Transform smokePos;
    public GameObject Shell;
    public Transform shellPos;
    public GameObject impact;
    // *************************

    private float nextTimeToFire = 0f;

    //pistol ADS
    private bool isAiming = false;
    public GameObject crosshairElement;

    public float adsFOV = 80f;
    private float originalFOV = 90f;

    // *************************
    //mg specific
    //
    private int framesBeforeNextShot; // to delay the sound to fix lag increment this
    private int currentShotFrame;
    public GameObject rifleSound;

    private Material mat;

    private Color origColor;
    private Color changeColor;
    //
    // *************************

    public bool isPistol;
    public bool isRifle;
    public bool isSniper;
    public bool canADS;

    void Awake()
    {
        currentAmmo = maxAmmo;

        if (isRifle)
        {
            Steam = smokePos.GetComponentInChildren<ParticleSystem>();
            mat = GetComponentInChildren<Renderer>().material;
            origColor = mat.color;
            changeColor = Color.black;
        }
    }

    void OnEnable()
    {
        isReloading = false;
        isAiming = false;
        fpsCam.fieldOfView = originalFOV;
        gunAnimator.SetBool("Reloading", false);

        // sounds
        if (isPistol)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("pistolSwitch");
            FindObjectOfType<AudioManager>().Play("pistolSwitch");
        }

        if (isRifle)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("rifleSwitch");
            FindObjectOfType<AudioManager>().Play("rifleSwitch");
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("sniperSwitch");
            FindObjectOfType<AudioManager>().Play("sniperSwitch");
        }

        // currentAmmo = maxAmmo; ammo reset when weapon switch
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload()); // automatic reload
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (canADS)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                isAiming = !isAiming;
                gunAnimator.SetBool("isAim", isAiming);

                if (isAiming)
                {
                    StartCoroutine(OnADS());
                }
                else
                {
                    OnUnADS();
                }
            }
        }
    }

    void Shoot()
    {
        gunModel.DOComplete();
        gunModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity);

        gunAnimator.SetTrigger("Fire");

        if (isPistol)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("pistolFire");
            FindObjectOfType<AudioManager>().Play("pistolFire");

            if (Muzzleflash != null)
            {
                Muzzleflash.Play();
            }

            if (Shell != null)
            {
                GameObject shellIteration = Instantiate(Shell, shellPos.position, shellPos.rotation);
                Destroy(shellIteration, 1f);
                isPistol = true;
            }
        }

        if (Smoke != null)
        {
            GameObject smokeIteration = Instantiate(Smoke, smokePos.position, smokePos.rotation);
            Destroy(smokeIteration, 1.8f);
        }

        if (isRifle)
        {
            if (Muzzleflash != null)
            {
                Muzzleflash.Play();
            }
            if (rifleSound != null) // mg specific
            {
                if (currentShotFrame == 0)
                {
                    GameObject gunShot = Instantiate(rifleSound, transform.position, transform.rotation);
                    gunShot.transform.parent = transform;
                    currentShotFrame = framesBeforeNextShot;
                }
                else
                {
                    currentShotFrame--;
                }
            }
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("sniperFire");
            FindObjectOfType<AudioManager>().Play("sniperFire");
            var smokers = smokePos.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem p in smokers)
            {
                p.Play();
            }
        }

        currentAmmo--;
            
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            TargetBasic targetBasic = hit.transform.GetComponentInParent<TargetBasic>();

            if (targetBasic != null)
            {
                if (hit.transform.name.Contains("HeadHitbox"))
                {
                    targetBasic.TakeDamageHead(damage);
                }
                else if (hit.transform.name.Contains("BodyHitbox"))
                {
                    targetBasic.TakeDamageBody(damage);
                }
                else if (hit.transform.name.StartsWith("_Minor"))
                {
                    targetBasic.TakeDamageArms(damage);
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactParticle = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactParticle, 0.40f);
        }

        if (laserShot != null) // laser specific
        {
            GameObject laser = Instantiate(laserShot, transform.position, transform.rotation);
            laser.GetComponent<LaserBehaviour>().setTarget(hit.point);
            Destroy(laser, 2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        gunAnimator.SetBool("Reloading", true);

        if (isPistol)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("pistolReload");
            FindObjectOfType<AudioManager>().Play("pistolReload");
        }

        if (isRifle)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("rifleReload");
            FindObjectOfType<AudioManager>().Play("rifleReload");

            Steam.Play();

            mat.SetColor("_EmissionColor", changeColor);
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("sniperReload");
            FindObjectOfType<AudioManager>().Play("sniperReload");
        }

        yield return new WaitForSeconds(reloadTime - .25f);

        gunAnimator.SetBool("Reloading", false);

        if (isRifle)
        {         
            mat.SetColor("_EmissionColor", origColor);
        }

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator OnADS()
    {
        yield return new WaitForSeconds(.15f);

        fpsCam.fieldOfView = adsFOV;
        // crosshairElement.SetActive(false);
    }

    void OnUnADS()
    {
        fpsCam.fieldOfView = originalFOV;
        // crosshairElement.SetActive(true);
    }
}
