using UnityEngine;

public class SemiCircleProportional : MonoBehaviour
{
    public int sliceCount = 6;
    public float innerRadius = 50f;
    public float outerRadius = 120f;

    public float[] weights; // proportions
    private RadialSlice[] slices;

    void Start()
    {
        if (weights == null || weights.Length != sliceCount)
        {
            weights = new float[sliceCount];
            for (int i = 0; i < sliceCount; i++)
                weights[i] = 1f;  // valeurs par défaut égales
        }

        GenerateSlices();
        UpdateSlices();
    }

    void GenerateSlices()
    {
        slices = new RadialSlice[sliceCount];
        for (int i = 0; i < sliceCount; i++)
        {
            GameObject go = new GameObject("Slice" + i, typeof(RadialSlice));
            go.transform.SetParent(transform, false);

            RadialSlice s = go.GetComponent<RadialSlice>();
            s.innerRadius = innerRadius;
            s.outerRadius = outerRadius;

            slices[i] = s;
        }
    }

    void UpdateSlices()
    {
        float total = 0f;
        for (int i = 0; i < weights.Length; i++)
            total += weights[i];

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

    // Appelle ceci pour modifier les proportions
    public void AddToSlice(int index, float amount)
    {
        weights[index] += amount;

        // on empêche les valeurs négatives
        if (weights[index] < 0.01f) weights[index] = 0.01f;

        UpdateSlices();
    }

    public void AddToSliceFromButton(int index)
    {
        AddToSlice(index, 0.5f); // valeur que tu veux
    }
}
