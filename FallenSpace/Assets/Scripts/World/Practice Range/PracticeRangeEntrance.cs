using UnityEngine;
using UnityEngine.UI;

public class PracticeRangeEntrance : MonoBehaviour
{
    [Header("Map to load")]
    public GameObject PracticeRange; //what to load

    [Header("Maps to unload")]
    public GameObject[] Maps; // other map prefabs

    [Header("Door")]
    public GameObject DoorEntrance; // entrance door
    public GameObject DoorExit; // exit door
    private Animator _doorEntranceAnimator; // animator for entrance door
    private Animator _doorExitAnimator; // animator for exit door

    [Header("UI")]
    public Text noteText;

    void Awake()
    {
        _doorEntranceAnimator = DoorEntrance.GetComponent<Animator>();
        _doorExitAnimator = DoorExit.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if player enters trigger area
        {
            noteText.text = "Press key <color=#88FF6AFF> [F] </color> to Use"; // display hint text
        }

        if (PracticeRange.activeSelf == true)
        {
            noteText.text = ""; // set hint text back to nothing
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // if player stays in trigger area
        {
            RequireInput(); // require the input displayed in hint text
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // if player leaves trigger area
        {
            noteText.text = ""; // set hint text back to nothing
        }
    }

    private void RequireInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && PracticeRange.activeSelf == false) // if PracticeRange not active, ie: door unopened
        {
            _doorEntranceAnimator.SetBool("character_nearby", true); // open door
            LoadMap(); // load PracticeRange
        }
    }

    private void LoadMap()
    {
        PracticeRange.SetActive(true);

        if (Maps != null) 
        {
            foreach (GameObject map in Maps)
            {
                if (map.activeSelf == true)
                {
                    map.SetActive(false); // WE WOULD DISABLE HUB AND TELE ROOM IF THEY WERE EVEN ACTIVE
                }
            }
        }
    }
}
