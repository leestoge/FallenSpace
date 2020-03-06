using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter settings")]
    public Transform teleportDestination;
    public float teleDelay;

    private GameObject _thePlayer;

    [Header("UI")]
    public Text noteText;

    [Header("Particles & Effects")]
    public ParticleSystem teleParticle;
    public Light[] teleLights;

    private float _origLightIntensity;
    private bool lerpLights = false;

    void Awake()
    {
        _origLightIntensity = teleLights[0].intensity;
        _thePlayer = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (lerpLights == true)
        {
            foreach (Light l in teleLights)
            {
                l.intensity = Mathf.Lerp(l.intensity, 4.65f, Time.deltaTime * teleDelay);
            }
        }
        else if (lerpLights == false)
        {
            foreach (Light l in teleLights)
            {
                l.intensity = Mathf.Lerp(l.intensity, _origLightIntensity, Time.deltaTime * teleDelay);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            noteText.text = "Press key <color=#88FF6AFF> [F] </color> to Use";
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RequireInput();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            noteText.text = "";
        }
    }

    private void RequireInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("teleStart");
            lerpLights = true;
            StartCoroutine(ExecuteAfterTime(teleDelay));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay     
        TeleportPlayer();
    }

    private void TeleportPlayer()
    {
        FindObjectOfType<AudioManager>().RandomizePitchAndPlay("telePort");
        teleParticle.Play();
        _thePlayer.transform.position = teleportDestination.transform.position;
        lerpLights = false;
    }
}
