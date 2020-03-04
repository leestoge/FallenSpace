using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmo : MonoBehaviour
{
    public Text ammoOnScreen;

    // Update is called once per frame
    void Update()
    {
        ammoOnScreen.text = FindObjectOfType<Gun>().currentAmmo.ToString();
    }
}
