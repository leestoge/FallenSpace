using UnityEngine;

public class PracticeRangeEnter : MonoBehaviour
{
    public GameObject canvasScore;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasScore.SetActive(true);
        }
    }
}
