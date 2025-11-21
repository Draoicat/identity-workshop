using UnityEngine;

public class RadialSliceSeatsSpawner : MonoBehaviour
{
    public GameObject seatPrefab;

    [Header("Disposition des sièges")]
    public int rows = 3;          
    public int seatsPerRow = 4;   
    public float rowSpacing = 15f;

    private RadialSlice slice;
    private Transform seatContainer;

    void Awake()
    {
        slice = GetComponent<RadialSlice>();

        seatContainer = new GameObject("Seats").transform;
        seatContainer.SetParent(transform, false);
    }

    public void SpawnSeats()
    {
        // Nettoyer les anciens sièges
        foreach (Transform c in seatContainer)
            Destroy(c.gameObject);

        float start = slice.startAngle * Mathf.Deg2Rad;
        float end = slice.endAngle * Mathf.Deg2Rad;

        float sliceAngleRad = end - start;

        for (int row = 0; row < rows; row++)
        {
            float radius = slice.innerRadius + rowSpacing * (row + 1);

            for (int s = 0; s < seatsPerRow; s++)
            {
                float t = (seatsPerRow == 1) ? 0.5f : (float)s / (seatsPerRow - 1);
                float angle = start + sliceAngleRad * t;

                Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                GameObject seat = Instantiate(seatPrefab, seatContainer);
                RectTransform rt = seat.GetComponent<RectTransform>();
                rt.anchoredPosition = pos;

                // Rotation vers le centre
                float rot = angle * Mathf.Rad2Deg - 90f;
                rt.localRotation = Quaternion.Euler(0, 0, rot);
            }
        }
    }
}
