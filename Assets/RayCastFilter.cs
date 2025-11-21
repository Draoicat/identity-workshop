using UnityEngine;
using UnityEngine.UI;

public class RadialSliceRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    public float startAngle;
    public float endAngle;
    public float innerRadius = 0f;
    public float outerRadius = 200f;  // Défini par toi ou auto-calculé

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        RectTransform rt = transform as RectTransform;

        // Position locale du clic dans l'objet
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, sp, eventCamera, out local);

        float distance = local.magnitude;
        if (distance < innerRadius || distance > outerRadius)
            return false;

        float angle = Mathf.Atan2(local.y, local.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        // Demi-cercle sur 180°
        if (startAngle < endAngle)
            return angle >= startAngle && angle <= endAngle;

        return angle >= startAngle || angle <= endAngle;
    }
}
