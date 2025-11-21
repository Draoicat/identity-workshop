using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SemiCircleProportional : MonoBehaviour
{
    public int sliceCount = 6;
    public float innerRadius = 50f;
    public float outerRadius = 120f;
    public Color[] sliceColors;

    public float[] weights;
    private RadialSlice[] slices;

    public float growAmount = 0.2f;

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

            RadialSlice slice = go.AddComponent<RadialSlice>();
            slice.innerRadius = innerRadius;
            slice.outerRadius = outerRadius;

            slice.color = sliceColors[i];
            slice.Init(i, this);

            // Raycast filter
            var filter = go.AddComponent<RadialSliceRaycastFilter>();
            filter.innerRadius = innerRadius;
            filter.outerRadius = outerRadius;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(outerRadius * 2f, outerRadius * 2f);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;


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

            // Update raycast filter
            var filter = slices[i].GetComponent<RadialSliceRaycastFilter>();
            filter.startAngle = currentAngle;
            filter.endAngle = currentAngle + sliceAngle;

            currentAngle += sliceAngle;
        }
    }

    public void AddToSlice(int index, float amount)
    {
        // augmente la part ciblée
        weights[index] += amount;

        // empêche une part de disparaître
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

    public float[] GetSlicePercentages()
    {
        float[] percents = new float[sliceCount];

        float totalAngle = 180f;
        float totalWeights = 0f;

        foreach (float w in weights)
            totalWeights += w;

        for (int i = 0; i < sliceCount; i++)
        {
            float sliceAngle = (weights[i] / totalWeights) * totalAngle;
            percents[i] = (sliceAngle / totalAngle) * 100f;
        }
        
        Array.Reverse(percents);
        return percents;
    }
}
