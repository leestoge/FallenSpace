using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PracticeRangeHandler : MonoBehaviour
{  
    [Header("UI")]
    public Text noteText;
    public TextMeshPro timerText; // variable to link to the ui element

    [Header("Range Settings")]
    public GameObject RangeTargets;
    public float startDelay;
    public float duration = 30.0f;
    public GameObject trigger;

    [Header("Sound")]
    public GameObject startSoundHolder;
    public AudioClip startSound;
    public GameObject endSound;

    private GameObject _thePlayer;
    private AudioSource _aSource;
    private TargetBasic target;
    private float timer = 0.0f;
    private PlayerPoints points;

    void Awake()
    {
        _aSource = startSoundHolder.GetComponent<AudioSource>();
        _thePlayer = GameObject.FindGameObjectWithTag("Player");
        points = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPoints>(); // find points component in player
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = FormatSeconds(timer);

        if (timer <= 0.0f)
        {
            TrainingEnds();
        }
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
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

    private void RequireInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            _aSource.clip = startSound;
            _aSource.PlayOneShot(_aSource.clip); // START SOUND

            StartCoroutine(ExecuteAfterTime(startDelay));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            noteText.text = "";
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay     
        endSound.SetActive(false);
        BeginPracticeRound();
    }

    void BeginPracticeRound()
    {
        trigger.GetComponent<BoxCollider>().enabled = false;
        noteText.text = "";

        timer = duration;

        foreach (TargetBasic target in RangeTargets.GetComponentsInChildren<TargetBasic>())
        {
            target.TargetActivate();
        }
    }

    void TrainingEnds()
    {
        foreach (TargetBasic target in RangeTargets.GetComponentsInChildren<TargetBasic>())
        {
            target.TargetInactive();         
        }

        timerText.text = "0:00";
        trigger.GetComponent<BoxCollider>().enabled = true;

        endSound.SetActive(true);
        points.TrainingModeUIReset();
    }
}
