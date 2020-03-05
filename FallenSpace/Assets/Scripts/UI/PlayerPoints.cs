using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoints : MonoBehaviour
{
    public int Points; // set in the unity editor
    public int eliminationAward; // set in the unity editor
    public int headshotAward; // set in the unity editor
    public int bodyshotAward; // set in the unity editor
    public int limbshotAward; // set in the unity editor

    public TextMeshPro physicalScoreUI; // variable to link to the ui element
    public Text canvasScoreUI;

    void Awake()
    {
        physicalScoreUI.text = Points.ToString();
    }

    public void AwardLocationalDamage(string area, bool isTraining) // headshot award
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

        if (isTraining)
        {
            canvasScoreUI.text = Points.ToString();
        }
        else
        {
            physicalScoreUI.text = Points.ToString(); // update ui element
        }      
    }

    public void AwardElimination(bool isTraining)
    {
        Points += eliminationAward;

        if (isTraining)
        {
            canvasScoreUI.text = Points.ToString();
        }
        else
        {
            physicalScoreUI.text = Points.ToString(); // update ui element
        }
    }

    public void TrainingModeUIReset()
    {
        canvasScoreUI.text = "0";
    }
}
