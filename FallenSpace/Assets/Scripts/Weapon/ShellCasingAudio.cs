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
        if (other.CompareTag("Player"))
        {
            return;
        }

        if (audioPlayed == false && !other.gameObject.CompareTag("Weapon") && !other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().RandomizePitchAndPlay("shellCasing");
            audioPlayed = true;
            FindObjectOfType<Gun>().DestroyShellCasing();
        }
    } 
}
