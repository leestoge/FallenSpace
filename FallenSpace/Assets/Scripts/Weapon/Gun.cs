using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Global variables")]
    [Space]
    public float damage;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public float reloadTime = 1f;
    public int maxAmmo;

    [HideInInspector] public int currentAmmo; 
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    private bool isAiming = false;

    public Camera fpsCam;
    public Animator gunAnimator;
    public Transform gunModel;

    [Space]
    [Header("DOTWEEN variables")]
    public float punchStrenght = .2f;
    public int punchVibrato = 5;
    public float punchDuration = .3f;
    [Range(0, 1)]
    public float punchElasticity = .5f;


    [Space]
    [Header("Particle related variables")]

    // particles ***************
    public ParticleSystem Muzzleflash;
    public GameObject Smoke;
    public GameObject Steamer;
    public GameObject Shockwave;
    public Transform smokePos;
    public GameObject Shell;
    public Transform shellPos;
    public GameObject impact;

    // Caching what would otherwise be local variables
    #region Caching
    private ParticleSystem Steam;
    private GameObject cachedGameObject;
    private ParticleSystem[] smokers;
    // *************************
    #endregion

    [Space]
    [Header("MG related variables")]

    // *************************
    //mg specific
    //
    public GameObject mgSound;
    public GameObject mgEmptySound;
    public CamRecoil camRecoil;
    public WepRecoil wepRecoil;
    private int framesBeforeNextShot; // to delay the sound to fix lag increment this
    private int currentShotFrame;

    // Energy Cycler colour
    private Material mat;
    private Color origColor;
    private Color changeColor;
    //
    // *************************

    [Space]
    [Header("Weapon type differentiation")]

    public bool isPistol;
    public bool isRifle;
    public bool isSniper;
    public bool canADS;

    // Weapon out of ammo audio bool
    private bool isOutAudioPlayed;

    void Awake()
    {
        currentAmmo = maxAmmo;
        isOutAudioPlayed = false;

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

        // ammo reset when weapon switch
        // currentAmmo = maxAmmo; 
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0) // need to reload
        {
            // automatic reload
            //StartCoroutine(Reload()); 
            //

            ADSHandler();
            AmmoOut();

            if (isPistol || isRifle)
            {
                gunAnimator.SetBool("isEmpty", true);                
            }

            if (isRifle)
            {
                mat.SetColor("_EmissionColor", changeColor);             
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }

            // if fire input play empty sound
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ShootEmpty();
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
                camRecoil.isAiming = !camRecoil.isAiming;

                if (isRifle)
                {
                    wepRecoil.isAiming = !wepRecoil.isAiming;
                }

                isAiming = !isAiming;
                gunAnimator.SetBool("isAim", isAiming);

            }
        }
    }

    void Shoot()
    {
        gunModel.DOComplete();
        gunModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity);

        gunAnimator.SetTrigger("Fire");
        camRecoil.Fire();

        if (isPistol)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolFire");

            if (Muzzleflash != null && Smoke != null && Shell != null)
            {
                Muzzleflash.Play();
                cachedGameObject = Instantiate(Smoke, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                cachedGameObject = Instantiate(Shell, shellPos.position, shellPos.rotation);
                Destroy(cachedGameObject, 1.8f);
                Destroy(cachedGameObject, 5f);
            }
        }

        if (isRifle)
        {
            if (mgSound != null) // mg specific
            {
                if (currentShotFrame == 0)
                {
                    
                    wepRecoil.Fire();
                    cachedGameObject = Instantiate(mgSound, transform.position, transform.rotation);
                    cachedGameObject.transform.parent = transform;
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
                cachedGameObject = Instantiate(Smoke, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                Destroy(cachedGameObject, 1.8f);
            }
            if (Steamer != null)
            {
                cachedGameObject = Instantiate(Steamer, smokePos.position, smokePos.rotation);
                Destroy(cachedGameObject, 1.8f);
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

            if (Muzzleflash != null && Smoke != null && Shockwave !=null)
            {
                Muzzleflash.Play();
                cachedGameObject = Instantiate(Shockwave, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                cachedGameObject = Instantiate(Smoke, Muzzleflash.transform.position, Muzzleflash.transform.rotation);
                Destroy(cachedGameObject, 1.8f);
                Destroy(cachedGameObject, 1.8f);
            }
        }

        currentAmmo--;
            
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("'" + transform.name + "' has hit '" + hit.transform.name+ "'");

            // Handle target impact
            FindObjectOfType<TargetImpactHandler>().HandleImpact(hit, damage);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            // handle particle impact
            FindObjectOfType<ImpactParticleHandler>().HandleImpact(hit);
        }
    }

    void ShootEmpty()
    {
        if (isPistol)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolDryFire");          
        }

        if (isRifle)
        {
            if (mgSound != null) // mg specific
            {
                if (currentShotFrame == 0)
                {
                   cachedGameObject = Instantiate(mgEmptySound, transform.position, transform.rotation);
                   cachedGameObject.transform.parent = transform;
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
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("sniperDryFire");
        }      
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("'" + transform.name + "' is reloading...");

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
            gunAnimator.SetBool("isEmpty", false);
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
        isOutAudioPlayed = false;

        if (isRifle)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("rifleReady");
        }
    }

    public void DestroyShellCasing()
    {
        Destroy(cachedGameObject, 1f);
    }

    void AmmoOut()
    {

        if (isPistol && isOutAudioPlayed == false)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("pistolOut");
            isOutAudioPlayed = true;
        }

        if (isRifle && isOutAudioPlayed == false)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("rifleOut");
            isOutAudioPlayed = true;
        }

        if (isSniper && isOutAudioPlayed == false)
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("sniperOut");
            isOutAudioPlayed = true;
        }      
    }
}
