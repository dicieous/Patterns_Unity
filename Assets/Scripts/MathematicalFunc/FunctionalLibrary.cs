using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionalLibrary
{
    public static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }
    
    public static float MultiWave(float x, float t)
    {
       var y = Sin(PI * (x + t));
       y += Sin(2f*PI * (x + t))*(1f/ 2f);
       return y*3f/2f;
    }
    
    public static float Ripple(float x, float t)
    {
        var d = Abs(x);
        var y = Sin(PI * (4f*d - t));
        return y / (1f + 10f * d);
    }
}
