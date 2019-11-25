using System.Collections;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private Animator animator;
    public GameObject scopeOverlay;
    public GameObject weaponCam;
    public Camera playerView;
    private bool isScoped = false;

    public float scopedFOV = 15f;
    private float originalFOV;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        originalFOV = playerView.fieldOfView;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            animator.SetBool("IsScoped", isScoped);

            if (isScoped)
            {
                StartCoroutine(OnScoped());
            }
            else
            {
                OnUnscoped();
            }
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);

        playerView.fieldOfView = scopedFOV;
        scopeOverlay.SetActive(true);
        weaponCam.SetActive(false);
    }

    void OnUnscoped()
    {
        scopeOverlay.SetActive(false);
        weaponCam.SetActive(true);
        playerView.fieldOfView = originalFOV;
    }
}
