using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


static class RandomUtils
{
    public static Vector3 RandomOffset(float maxOffset)
    {
        return Random.onUnitSphere * maxOffset * 2 * (RandomGaussian(-1.0f, 1.0f) - 0.5f);
    }

    public static float RandomDiv(float divBase, float divStd)
    {
        float offset = divStd * RandomGaussian(-1.0f, 1.0f);
        return divBase + offset;
    }

    public static float RandomGaussian(float min, float max)
    {
        // See http://www.design.caltech.edu/erik/Misc/Gaussian.html
        float x1 = Random.Range(0, 1.0f);
        float x2 = Random.Range(0, 1.0f);

        float val = Mathf.Sqrt(-2 * Mathf.Log(x1)) * Mathf.Cos(2 * Mathf.PI * x2);

        return val * (max - min) + min;
    }
}
