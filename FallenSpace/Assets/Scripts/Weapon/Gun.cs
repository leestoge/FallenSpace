using DG.Tweening;
using UnityEngine;
using System.Collections;

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
    public GameObject Steamer;
    public Transform smokePos;
    public GameObject Shell;
    public Transform shellPos;
    public GameObject impact;

    // Caching what would otherwise be local variables
    private ParticleSystem Steam;
    private GameObject shellIteration;
    private GameObject smokeIteration;
    private GameObject steamIteration;
    private ParticleSystem[] smokers;
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
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolSwitch");
        }

        if (isRifle)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("rifleSwitch");
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("sniperSwitch");
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

        if (currentAmmo <= 0) // require reload
        {
            //StartCoroutine(Reload()); // automatic reload
            ADSHandler();

            if (isPistol)
            {
                gunAnimator.SetBool("isEmpty", true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }
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

        ADSHandler();
    }

    void ADSHandler()
    {
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
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolFire");

            if (Muzzleflash != null && Smoke != null && Shell != null)
            {
                Muzzleflash.Play();
                smokeIteration = Instantiate(Smoke, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                shellIteration = Instantiate(Shell, shellPos.position, shellPos.rotation);
                Destroy(smokeIteration, 1.8f);
                Destroy(shellIteration, 5f);
            }
        }

        if (isRifle)
        {
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
            if (Muzzleflash != null && Smoke != null)
            {
                Muzzleflash.Play();
                smokeIteration = Instantiate(Smoke, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                Destroy(smokeIteration, 1.8f);
            }
            if (Steamer != null)
            {
                steamIteration = Instantiate(Steamer, smokePos.position, smokePos.rotation);
                Destroy(steamIteration, 1.8f);
            }
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("sniperFire");

            smokers = smokePos.GetComponentsInChildren<ParticleSystem>();
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

            GameObject impactParticle = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal)); // handle impact
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
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolReload");
            gunAnimator.SetBool("isEmpty", false);
        }

        if (isRifle)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("rifleReload");

            Steam.Play();

            mat.SetColor("_EmissionColor", changeColor);
        }

        if (isSniper)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("sniperReload");
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

    public void DestroyShellCasing()
    {
        Destroy(shellIteration, 1f);
    }
}
