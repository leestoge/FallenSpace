using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : MonoBehaviour {

    public static FPSCounterUI counter;

    public int frameRange = 30;
    private Text textComponent;
    
    private int averageFPS = 0;
    private int lowestFPS = 0;
    private int highestFPS = 0;

    private int[] FPSBuffer;
    private int FPSBufferIndex;

    public float overallAverageFPS;
    public float overallHighestFPS = -1f;
    public float overallLowestFPS = Mathf.Infinity;

    private void Start () {

        if (counter == null)
            counter = this;
        else
            Destroy (GetComponentInParent<Canvas> ().transform.gameObject);

        textComponent = GetComponent<Text> ();

    }

    private void Update () {
        
        if (FPSBuffer == null || FPSBuffer.Length != frameRange)
            InitialiseBuffer ();
        
        UpdateBuffer ();
        CalculateFPS ();

    }

    private void InitialiseBuffer () {

        if (frameRange <= 0)
            frameRange = 1;

        FPSBuffer = new int [frameRange];
        FPSBufferIndex = 0;

        averageFPS = (int)(1f / Time.unscaledDeltaTime);
        highestFPS = averageFPS;
        lowestFPS = averageFPS;
        UpdateDisplay ();

    }

    private void UpdateBuffer () {

        FPSBuffer [FPSBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (FPSBufferIndex >= frameRange) {

            CalculateFPS ();
            UpdateDisplay ();
            FPSBufferIndex = 0;

        }

    }

    private void CalculateFPS () {

        int sum = 0;
        int highest = 0;
        int lowest = int.MaxValue;

        for (int i = 0; i < frameRange; i++) {

            int FPS = FPSBuffer [i];
            sum += FPS;

            if (FPS > highest)
                highest = FPS;

            if (FPS < lowest)
                lowest = FPS;

        }

        averageFPS = sum / frameRange;
        highestFPS = highest;
        lowestFPS = lowest;

        if (highestFPS > overallHighestFPS)
            overallHighestFPS = highestFPS;

        if (lowestFPS < overallLowestFPS && lowestFPS > 0f)
            overallLowestFPS = lowestFPS;

    }

    private void UpdateDisplay () {

        textComponent.text = averageFPS + " FPS";

        if (overallAverageFPS > 0f) {

            overallAverageFPS += averageFPS;
            overallAverageFPS /= 2f;

        } else
            overallAverageFPS += averageFPS;

    }

}
