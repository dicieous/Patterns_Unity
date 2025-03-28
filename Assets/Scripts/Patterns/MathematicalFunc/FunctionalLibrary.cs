using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionalLibrary
{

    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionNames
    {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    }
    private static Function[] _functions = { Wave, MultiWave, Ripple, Sphere, Torus };

    public static int FunctionsCount => _functions.Length;

    public static Function GetFunction(FunctionNames names) => _functions[(int)names];

    public static FunctionNames GetNextFunctionName(FunctionNames name)
    {
        if ((int)name < _functions.Length - 1)
        {
            return name + 1;
        }

        return 0;
    }

    public static FunctionNames GetRandomNameFunctionOtherThan(FunctionNames name)
    {
        var choice = (FunctionNames)Random.Range(1, _functions.Length);
        return choice == name ? 0 : choice;
    }

    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (v + u + t));
        p.z = v;
        return p;
    }

    static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        var y = Sin(PI * (u + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (v + t));
        y += Sin(PI * (u + v + t * 0.25f));
        p.y = y * 1f / 2.5f;
        p.z = v;
        return p;
    }

    static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        var d = Sqrt(u * u + v * v);
        p.y = Sin(PI * (4f * d - t));
        p.y /= (1f + 10f * d);
        p.z = v;
        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        var r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        var s = r * Cos(0.5f * PI * v);

        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        var r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        var r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));

        float s = r1 + r2 * Cos(PI * v);

        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}
