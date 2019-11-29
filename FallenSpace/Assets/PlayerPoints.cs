using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int Points; // set in the unity editor
    public int eliminationAward; // set in the unity editor
    public int headshotAward; // set in the unity editor
    public int bodyshotAward; // set in the unity editor
    public int limbshotAward; // set in the unity editor
    public TextMeshPro HUD_element; // variable to link to the ui element

    void Awake()
    {
        HUD_element.text = Points.ToString();
    }

    public void AwardLocationalDamage(string area) // headshot award
    {
        if (area.Contains("head"))
        {
            Points += headshotAward;
        }
        else if (area.Contains("body")) // body award
        {
            Points += bodyshotAward;
        }
        else if (area.Contains("limb")) // limb award
        {
            Points += limbshotAward;
        }

        HUD_element.text = Points.ToString(); // update ui element
    }

    public void AwardElimination()
    {
        Points += eliminationAward;

        HUD_element.text = Points.ToString(); // update ui element
    }
}
