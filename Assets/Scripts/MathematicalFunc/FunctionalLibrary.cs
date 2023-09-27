using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionalLibrary
{
    
    public delegate Vector3 Function(float u,float v, float t);

    public enum FunctionNames
    {
        Wave,MultiWave,Ripple
    }
    private static Function[] _functions = { Wave, MultiWave, Ripple };
    
    public static Function GetFunction(FunctionNames names)
    {
        return _functions[(int)names];
    }
    
   static Vector3 Wave(float u,float v,  float t)
   {
       Vector3 p;
       p.x = u;
       p.y = Sin(PI * (v + u + t));
       p.z = v;
       return p;
   }
    
    static Vector3 MultiWave(float u, float v,  float t)
    {
        Vector3 p;
        p.x = u;
       var y = Sin(PI * (u + 0.5f*t));
       y += 0.5f* Sin(2f*PI * (v + t));
       y += Sin(PI * (u + v + t*0.25f));
       p.y = y * 1f / 2.5f;
       p.z = v;
       return p;
    }
    
    static Vector3 Ripple(float u,float v,  float t)
    {
        Vector3 p;
        p.x = u;
        var d = Sqrt(u * u + v * v);
        var y = Sin(PI * (4f*d - t));
        p.y = y / (1f + 10f * d);
        p.z = v;
        return p;
    }
}
