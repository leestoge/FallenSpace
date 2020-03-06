using UnityEngine;

public class PracticeRangeMapReloader : MonoBehaviour
{
    [Header("Range UI")]
    public GameObject canvasScore;

    [Header("Maps to reload")]
    public GameObject[] Maps; // other map prefabs


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canvasScore.activeSelf == true)
            {
                canvasScore.SetActive(false);
            };

            if (Maps != null)
            {
                foreach (GameObject map in Maps)
                {
                    if (map.activeSelf == false)
                    {
                        map.SetActive(true); // WE WOULD DISABLE HUB AND TELE ROOM IF THEY WERE EVEN ACTIVE
                    }
                }
            }
        }
    }
}
