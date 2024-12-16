using UnityEngine;

public class RandomColorLerpController : MonoBehaviour
{
    public Material material;        // The material using the shader
    public float lerpDuration = 2f;  // Duration of the color transition
    public string colorAProperty = "_ColorA"; // Name of the property for Color A
    public string colorBProperty = "_ColorB"; // Name of the property for Color B

    private Color startColorA;
    private Color startColorB;
    private Color targetColorA;
    private Color targetColorB;

    private float lerpTime;

    void Start()
    {
        // Initialize with random colors
        startColorA = GenerateRandomColor();
        targetColorA = GenerateRandomColor();
        startColorB = GenerateRandomColor();
        targetColorB = GenerateRandomColor();

        UpdateMaterialColors(startColorA, startColorB); // Set initial colors
    }

    void Update()
    {
        if (material == null)
        {
            Debug.LogWarning("Material is not assigned.");
            return;
        }

        // Increment lerp time
        lerpTime += Time.deltaTime / lerpDuration;

        if (lerpTime > 1f) // Transition complete
        {
            lerpTime = 0f;

            // Set new start and target colors
            startColorA = targetColorA;
            startColorB = targetColorB;
            targetColorA = GenerateRandomColor();
            targetColorB = GenerateRandomColor();
        }

        // Interpolate between colors
        Color currentColorA = Color.Lerp(startColorA, targetColorA, lerpTime);
        Color currentColorB = Color.Lerp(startColorB, targetColorB, lerpTime);

        // Update material colors
        UpdateMaterialColors(currentColorA, currentColorB);
    }

    private void UpdateMaterialColors(Color colorA, Color colorB)
    {
        material.SetColor(colorAProperty, colorA);
        material.SetColor(colorBProperty, colorB);
    }

    private Color GenerateRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f); // RGB with full alpha
    }
}