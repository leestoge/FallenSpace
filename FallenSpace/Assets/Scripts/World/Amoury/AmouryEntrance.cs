using UnityEngine;
using UnityEngine.UI;

public class AmouryEntrance : MonoBehaviour
{
    [Header("Map")]
    public GameObject Armoury;

    [Header("Door")]
    public GameObject Door;
    private Animator _doorAnimator;

    [Header("UI")]
    public Text noteText;

    void Awake()
    {
        _doorAnimator = Door.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if player enters trigger area
        {
            noteText.text = "Press key <color=#88FF6AFF> [F] </color> to Use"; // display hint text
        }

        if (Armoury.activeSelf == true)
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
        if (Input.GetKeyDown(KeyCode.F) && Armoury.activeSelf == false) // if Armoury not active, ie: door unopened
        {
            _doorAnimator.SetBool("character_nearby", true); // open door
            LoadMap(); // load armoury
        }
    }

    private void LoadMap()
    {
        Armoury.SetActive(true);
    }
}
