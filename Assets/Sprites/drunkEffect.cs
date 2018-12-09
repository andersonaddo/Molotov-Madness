using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class drunkEffect : MonoBehaviour
{

    public float intensityChangePerSecond, XYchangePerSecond;
    PostProcessProfile drunkProfile;
    LensDistortion lensDistortion;

    public int maxMinIntensity;
    int XTarget, YTarget, intensityTarget;

    // Start is called before the first frame update
    void Start()
    {
        drunkProfile = GetComponent<PostProcessVolume>().profile;
        drunkProfile.TryGetSettings(out lensDistortion);

        XTarget = Random.Range(0, 2);
        YTarget = Random.Range(0, 2);
        lensDistortion.intensityX.value = Random.Range(0, 1f);
        lensDistortion.intensityY.value = Random.Range(0, 1f);

        lensDistortion.intensity.value = Random.Range(-maxMinIntensity, maxMinIntensity);
        intensityTarget = maxMinIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        //The x and Y multipliers will be penduluming between 0 and 1
        if (lensDistortion.intensityX.value == XTarget)
        {
            if (XTarget == 0) XTarget = 1;
            else XTarget = 0;
        }

        if (lensDistortion.intensityY.value == YTarget)
        {
            if (YTarget == 0) YTarget = 1;
            else YTarget = 0;
        }

        if (lensDistortion.intensity.value == intensityTarget)
        {
            intensityTarget *= -1;
        }

        lensDistortion.intensityY.value = Mathf.MoveTowards(lensDistortion.intensityY.value, YTarget, XYchangePerSecond * Time.deltaTime);
        lensDistortion.intensityX.value = Mathf.MoveTowards(lensDistortion.intensityX.value, XTarget, XYchangePerSecond * Time.deltaTime);
        lensDistortion.intensity.value = Mathf.MoveTowards(lensDistortion.intensity.value, intensityTarget, intensityChangePerSecond * Time.deltaTime);

    }
}
