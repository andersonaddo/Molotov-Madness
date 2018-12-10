using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class drunkennessScript : MonoBehaviour
{
    public PostProcessVolume drunkVolume;
    public Slider drunkSlider;

    float drunkenness, nextReductionTime;

    public float reduction, reductionGapTime, molotovIncrease;

    // Start is called before the first frame update
    void Start()
    {
        drunkSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextReductionTime) increaseDrunkenness(-reduction);
    }


    public void increaseDrunkenness(float increment)
    {
        drunkenness = Mathf.Clamp01(drunkenness + increment);
        drunkSlider.value = drunkenness;
        drunkVolume.weight = drunkenness;
        nextReductionTime = Time.time + reductionGapTime;
    }

    //This is here so that the amount that the molotov increases can be changed in the default inspector, 
    //rather than the animator inspector (since this method is called from an event in the drink animation)
    public void increaseDrunkennessMolotov()
    {
        increaseDrunkenness(molotovIncrease);
    }
}
