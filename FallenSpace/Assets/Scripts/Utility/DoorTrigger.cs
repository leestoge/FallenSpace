using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private GameObject player;
    public Animator doorAnimator;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            doorAnimator.SetBool("character_nearby", true);
        }
    }
}
