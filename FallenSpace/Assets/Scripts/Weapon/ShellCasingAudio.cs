using UnityEngine;

public class ShellCasingAudio : MonoBehaviour
{

    private bool audioPlayed;


    void Awake()
    {
        audioPlayed = false;
    }

    void OnParticleCollision(GameObject other)
    {
        if (audioPlayed == false)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("shellCasing");
            FindObjectOfType<AudioManager>().Play("shellCasing");
            audioPlayed = true;
        }      
    } 
}
