using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class RadialSlice : MaskableGraphic, IPointerClickHandler
{
    public float startAngle;
    public float endAngle;

    public float innerRadius = 50f;
    public float outerRadius = 100f;

    public int resolution = 300;

    public int sliceIndex;
    private SemiCircleProportional controller;



    public void Init(int index, SemiCircleProportional ctrl)
    {
        sliceIndex = index;
        controller = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.AddToSlice(sliceIndex, controller.growAmount);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float angleStep = (endAngle - startAngle) / resolution;
        float startRad = startAngle * Mathf.Deg2Rad;
        float angleStepRad = angleStep * Mathf.Deg2Rad;

        for (int i = 0; i < resolution; i++)
        {
            float a0 = startRad + angleStepRad * i;
            float a1 = a0 + angleStepRad;

            Vector2 inner0 = new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * innerRadius;
            Vector2 inner1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * innerRadius;
            Vector2 outer0 = new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * outerRadius;
            Vector2 outer1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * outerRadius;

            int idx = vh.currentVertCount;

            vh.AddVert(inner0, color, Vector2.zero);
            vh.AddVert(inner1, color, Vector2.zero);
            vh.AddVert(outer1, color, Vector2.zero);
            vh.AddVert(outer0, color, Vector2.zero);

            vh.AddTriangle(idx, idx + 1, idx + 2);
            vh.AddTriangle(idx, idx + 2, idx + 3);
        }
    }
}