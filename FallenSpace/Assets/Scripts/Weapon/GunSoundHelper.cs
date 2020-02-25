using UnityEngine;

public class GunSoundHelper : MonoBehaviour
{
    private float totalTimeBeforeDestroy;

    // Use this for initialization
    void Awake()
    {
        var sound = GetComponent<AudioSource>();
        totalTimeBeforeDestroy = sound.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeBeforeDestroy -= Time.deltaTime;

        if (totalTimeBeforeDestroy <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
