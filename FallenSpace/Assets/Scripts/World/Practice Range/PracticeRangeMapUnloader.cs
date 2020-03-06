using UnityEngine;

public class PracticeRangeMapUnloader : MonoBehaviour
{
    [Header("Range UI")]
    public GameObject canvasScore;

    [Header("Maps to unload")]
    public GameObject[] Maps; // other map prefabs

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasScore.SetActive(true);

            if (Maps != null)
            {
                foreach (GameObject map in Maps)
                {
                    if (map.activeSelf == true)
                    {
                        map.SetActive(false); // WE WOULD DISABLE HUB, tele, armoury
                    }
                }
            }
        }
    }
}
