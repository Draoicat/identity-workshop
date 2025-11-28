using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SemiCircleProportional : MonoBehaviour
{
    public int sliceCount = 6;
    public float innerRadius = 50f;
    public float outerRadius = 120f;
    public int partiNum = 10;

    public float[] weights;
    private RadialSlice[] slices;

    [SerializeField] private AudioSource spawnSource;
    [SerializeField] private AudioClip spawnSound;
    
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioClip clickSound;

    public float growAmount = 0.2f;

    [SerializeField]
    private GameObject[] slicePrefabs;
    private List<GameObject>[] sliceMembers;   
    [SerializeField] private Color[] sliceColors;


    void Start()
    {
        
        if (weights == null || weights.Length != sliceCount)
        {
            weights = new float[sliceCount];
            for (int i = 0; i < sliceCount; i++)
                weights[i] = 1f;
        }
        
        sliceMembers = new List<GameObject>[sliceCount];
        for (int i = 0; i < sliceCount; i++)
            sliceMembers[i] = new List<GameObject>();

        GenerateSlices();
        UpdateSlices();
        
        for (int i = 0; i < 5; i++)
        {
            SpawnInitialMembers(i);
        }
        
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

            slice.color = (sliceColors != null && sliceColors.Length > i) 
                ? sliceColors[i] 
                : Color.white;
            slice.Init(i, this);
            slice.flashColor = slice.color + new Color(0.2f, 0.2f, 0.2f, 0.2f);

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
        clickSource.PlayOneShot(clickSound);
        SpawnPrefabInSlice(index);
        // augmente la part ciblée
        weights[index] += amount;

        // empêche une part de disparaître
        if (weights[index] < 0.01f)
            weights[index] = 0.01f;

        NormalizeWeights();
        UpdateSlices();
        CheckMembersInsideSlices();
    }
    
    private void SpawnInitialMembers(int sliceIndex)
    {
        
        for (int i = 0; i < partiNum; i++)
        {
            SpawnPrefabInSlice(sliceIndex);
        }
    }

    private void SpawnPrefabInSlice(int index)
    {
        RadialSlice slice = slices[index];

        float angleDeg = Random.Range(slice.startAngle, slice.endAngle);
        float angleRad = angleDeg * Mathf.Deg2Rad;

        float radius = Random.Range(innerRadius, outerRadius);

        Vector2 localPos = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;

        GameObject obj = Instantiate(slicePrefabs[index], transform);

        RectTransform rt = obj.transform as RectTransform;
        if (rt != null)
        {
            rt.anchoredPosition = localPos;
        }
        else
        {
            obj.transform.localPosition = localPos;
        }

        obj.transform.SetAsLastSibling();
        sliceMembers[index].Add(obj);
        spawnSource.PlayOneShot(spawnSound);
    }
    
    private void CheckMembersInsideSlices()
    {
        for (int i = 0; i < sliceCount; i++)
        {
            RadialSlice slice = slices[i];

            List<GameObject> stillValid = new List<GameObject>();

            foreach (GameObject member in sliceMembers[i])
            {
                if (member == null) continue;

                Vector2 pos;
                RectTransform rt = member.transform as RectTransform;

                if (rt != null)
                    pos = rt.anchoredPosition;
                else
                    pos = member.transform.localPosition;

                float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
                angle = (angle + 360f) % 360f;

                float dist = pos.magnitude;

                bool inside =
                    dist >= innerRadius &&
                    dist <= outerRadius &&
                    angle >= slice.startAngle &&
                    angle <= slice.endAngle;

                if (!inside)
                {
                    GameObject.Destroy(member);
                }
                else
                {
                    stillValid.Add(member);
                }
            }

            sliceMembers[i] = stillValid;
        }
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
