using UnityEngine;
using UnityEngine.UI;

public class SemiCircleProportional : MonoBehaviour
{
    public int sliceCount = 6;
    public float innerRadius = 50f;
    public float outerRadius = 120f;

    public float[] weights;
    private RadialSlice[] slices;

    public float growAmount = 0.5f;

    void Start()
    {
        if (weights == null || weights.Length != sliceCount)
        {
            weights = new float[sliceCount];
            for (int i = 0; i < sliceCount; i++)
                weights[i] = 1f;
        }

        GenerateSlices();
        UpdateSlices();
    }

    void GenerateSlices()
    {
        slices = new RadialSlice[sliceCount];

        for (int i = 0; i < sliceCount; i++)
        {
            GameObject go = new GameObject("Slice" + i);
            go.transform.SetParent(transform, false);

            // RadialSlice est le renderer
            RadialSlice slice = go.AddComponent<RadialSlice>();
            slice.innerRadius = innerRadius;
            slice.outerRadius = outerRadius;

            // bouton
            Button button = go.AddComponent<Button>();

            // couleur (RadialSlice hérite de Graphic => color est disponible)
            slice.color = Random.ColorHSV();

            // capture de l’index pour le callback
            int capturedIndex = i;
            button.onClick.AddListener(() =>
            {
                AddToSlice(capturedIndex, growAmount);
            });

            slices[i] = slice;
        }
    }

    void UpdateSlices()
    {
        float total = 0f;
        foreach (float w in weights)
            total += w;

        float currentAngle = 0f;

        for (int i = 0; i < sliceCount; i++)
        {
            float sliceAngle = (weights[i] / total) * 180f;

            slices[i].startAngle = currentAngle;
            slices[i].endAngle = currentAngle + sliceAngle;

            slices[i].SetVerticesDirty();
            currentAngle += sliceAngle;
        }
    }

    public void AddToSlice(int index, float amount)
    {
        weights[index] += amount;

        if (weights[index] < 0.01f)
            weights[index] = 0.01f;

        NormalizeWeights();
        UpdateSlices();
    }

    void NormalizeWeights()
    {
        float total = 0f;
        foreach (float w in weights)
            total += w;

        for (int i = 0; i < sliceCount; i++)
            weights[i] /= total;
    }
}
