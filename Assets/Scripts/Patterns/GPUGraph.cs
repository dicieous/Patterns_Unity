using System;
using UnityEngine;


public class GPUGraph : MonoBehaviour
{
    public enum TransitionMode
    {
        None,
        Cycle,
        Random,
    }

    private static readonly int
        positionID = Shader.PropertyToID("_Positions"),
        timeID = Shader.PropertyToID("_Time"),
        resolutionID = Shader.PropertyToID("_Resolution"),
        stepID = Shader.PropertyToID("_Step"),
        transitionProgressID = Shader.PropertyToID("_TransitionProgress");

    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private Mesh patternObjMesh;
    [SerializeField] private Material patternObjMaterial;

    private const int MAX_RESOLUTION = 1000;
    [SerializeField, Range(10, MAX_RESOLUTION)] private int resolution = 10;

    [SerializeField] private FunctionalLibrary.FunctionNames function;
    [SerializeField] private TransitionMode transitionMode;

    [SerializeField, Min(0f)] float functionDuration = 2f, transitionDuration = 1f;

    bool transitioning = false;
    FunctionalLibrary.FunctionNames transitionFunction;

    float duration = 0f;

    private ComputeBuffer positionBuffer;


    private void OnEnable()
    {
        positionBuffer = new ComputeBuffer(MAX_RESOLUTION * MAX_RESOLUTION, 3 * 4);
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

        UpdateFunctionOnGPU();
    }

    private void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionID, resolution);

        computeShader.SetFloat(timeID, Time.time);
        computeShader.SetFloat(stepID, step);
        
        if (transitioning) {
            computeShader.SetFloat(
                transitionProgressID,
                Mathf.SmoothStep(0f, 1f, duration / transitionDuration)
            );
        }

        var kernelIndex =
            (int)function +
            (int)(transitioning ? transitionFunction : function) *
            FunctionalLibrary.FunctionsCount;
        
        computeShader.SetBuffer(kernelIndex, positionID, positionBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        patternObjMaterial.SetBuffer(positionID, positionBuffer);
        patternObjMaterial.SetFloat(stepID, step);
        
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(patternObjMesh, 0, patternObjMaterial, bounds, resolution * resolution);
    }

    private void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle
            ? FunctionalLibrary.GetNextFunctionName(function)
            : FunctionalLibrary.GetRandomNameFunctionOtherThan(function);
    }

    private void OnDisable()
    {
        positionBuffer?.Release();
        positionBuffer = null;
    }
}