using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTarget;
    private GameObject _thePlayer;

    void Awake()
    {
        _thePlayer = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        _thePlayer.transform.position = teleportTarget.transform.position;
    }
}
