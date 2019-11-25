using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    void Awake()
    {
        selectWeapon();
    }


    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1)) // pistol
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) // rifle
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) // sniper
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4) // heavy??
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            selectWeapon();
        }
    }

    void selectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
