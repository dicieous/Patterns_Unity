using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private Transform pointCube;

    [SerializeField, Range(10, 200)] private int resolution = 10;

    [SerializeField]
    FunctionalLibrary.FunctionNames function;

    [SerializeField, Min(0f)]
    float functionDuration = 2f, transitionDuration = 1f;

    bool transitioning = false;
    FunctionalLibrary.FunctionNames transitionFunction;

    float duration = 0f;

    public enum TranstionMode
    {
        None,
        Cycle,
        Random,
    }

    [SerializeField]
    TranstionMode transitionMode;


    private Transform[] _point;
    private void Awake()
    {
        _point = new Transform[resolution * resolution];

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;

        for (int i = 0; i < _point.Length; i++)
        {
            Transform point = _point[i] = Instantiate(pointCube);

            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;

        if (duration >= transitionDuration)
        {
            if (transitioning)
            {
                duration -= functionDuration;
                transitioning = false;
            }
            else
            {
                duration -= functionDuration;
                transitioning = true;
                transitionFunction = function;
                PickNextFunction();
            }
        }

        if (transitioning)
        {
            UpdateTransitionFunction();
        }
        else
        {
            UpdateFunction();
        }
    }

    public void PickNextFunction()
    {
        function = transitionMode == TranstionMode.Cycle ? FunctionalLibrary.GetNextFunctionName(function) : FunctionalLibrary.GetRandomNameFunctionOtherThan(function);
    }

    private void UpdateFunction()
    {
        FunctionalLibrary.Function fun = FunctionalLibrary.GetFunction(function);

        float step = 2f / resolution;
        var time = Time.time;
        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < _point.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            _point[i].localPosition = fun(u, v, time);
        }
    }

    private void UpdateTransitionFunction()
    {
        FunctionalLibrary.Function from = FunctionalLibrary.GetFunction(transitionFunction), to = FunctionalLibrary.GetFunction(function);
        float progress = duration / transitionDuration;

        float step = 2f / resolution;
        var time = Time.time;
        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < _point.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            _point[i].localPosition = FunctionalLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}
