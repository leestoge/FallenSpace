using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter settings")]
    public Transform teleportDestination;
    public float teleDelay;

    private GameObject _thePlayer;
    private PracticeRangeEnter rangeEnterScriptInstance;

    [Header("UI")]
    public Text noteText;
    public GameObject canvasScore;

    void Awake()
    {
        _thePlayer = GameObject.FindGameObjectWithTag("Player");
        rangeEnterScriptInstance = GetComponent<PracticeRangeEnter>();
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

        if (canvasScore.activeSelf == true)
        {
            canvasScore.SetActive(false);
        }
    }

    private void RequireInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("teleStart");
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
        _thePlayer.transform.position = teleportDestination.transform.position;

        //if (canvasScore.activeSelf == true)
        //{
        //    canvasScore.SetActive(false);
        //}
    }
}
