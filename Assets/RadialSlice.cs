using Core;
using System.Collections; // Ajout requis pour les Coroutines
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Event = Core.Event; // Assurez-vous que le type Event correspond à votre namespace Core

[RequireComponent(typeof(CanvasRenderer))]
public class RadialSlice : MaskableGraphic, IPointerClickHandler
{
    // --- Variables de Définition de la Part (Pas de Changement) ---
    public float startAngle;
    public float endAngle;

    public float innerRadius = 50f;
    public float outerRadius = 100f;

    public int resolution = 300;

    public int sliceIndex;
    private SemiCircleProportional controller;
     private float scaleAmount = 1.1f;
    private RectTransform rectTransform; // Référence au RectTransform

    private bool canClick = false;

    // --- NOUVELLES VARIABLES POUR LE FEEDBACK VISUEL ---
    [Header("Feedback Visuel")]
    [SerializeField] public Color flashColor = new Color(1f, 1f, 1f, 0.8f); // Couleur de l'effet "Flash" (Blanc semi-transparent par défaut)
    [SerializeField] public float flashDuration = 0.15f; // Durée du flash en secondes

    private Color originalColor; // Stocke la couleur de base de la part
    private Coroutine flashCoroutine; // Référence à la coroutine pour éviter les conflits
    // -----------------------------------------------------

    // --- Méthodes de Gestion des Clics (Pas de Changement) ---
    private void ActivateClick(Event _)
    {
        canClick = true;
    }

    private void DeactivateClick(Event _)
    {
        canClick = false;
    }

    // --- Méthode d'Initialisation Modifiée ---
    public void Init(int index, SemiCircleProportional ctrl)
    {
        sliceIndex = index;
        controller = ctrl;

        // Stocke la couleur initiale (définie dans l'Inspecteur ou par le code du contrôleur)
        originalColor = color;

        // Abonnement aux événements
        EventManager.Instance.OnVoteStarted += ActivateClick;
        EventManager.Instance.OnVoteEnded += DeactivateClick;
        rectTransform = GetComponent<RectTransform>();
    }

    // --- NOUVELLE MÉTHODE PUBLIQUE POUR DÉCLENCHER LE FLASH ---
    public void FlashFeedback()
    {
        // Arrête toute coroutine de flash en cours pour éviter les sauts
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        // Lance la coroutine de flash
        flashCoroutine = StartCoroutine(DoFlash());
    }

    // Coroutine qui gère le changement de couleur temporaire
    private IEnumerator DoFlash()
    {
        // Assurez-vous que le RectTransform est accessible
        if (rectTransform == null) yield break;

        // 1. Début de l'effet (Scale UP et Flash Color)
        color = flashColor;
        rectTransform.localScale = Vector3.one * scaleAmount; // Met à l'échelle
        SetVerticesDirty();

        // 2. Attend la durée du flash
        yield return new WaitForSeconds(flashDuration);

        // 3. Fin de l'effet (Scale DOWN et Couleur Originale)
        color = originalColor;
        rectTransform.localScale = Vector3.one; // Revient à l'échelle normale (1, 1, 1)
        SetVerticesDirty();

        flashCoroutine = null;
    }
    // -----------------------------------------------------

    // --- Gestion du Clic (Ajout de l'Appel FlashFeedback) ---
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return;

        // Déclenche le feedback visuel immédiatement
        FlashFeedback();

        // Déclenche la logique du contrôleur (son, poids, spawn)
        controller.AddToSlice(sliceIndex, controller.growAmount);
    }

    // --- OnPopulateMesh (Pas de Changement) ---
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // ... (le contenu de OnPopulateMesh reste inchangé car il utilise la variable 'color' de MaskableGraphic)
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

            // Utilise la variable 'color' modifiée par le flash
            vh.AddVert(inner0, color, Vector2.zero);
            vh.AddVert(inner1, color, Vector2.zero);
            vh.AddVert(outer1, color, Vector2.zero);
            vh.AddVert(outer0, color, Vector2.zero);

            vh.AddTriangle(idx, idx + 1, idx + 2);
            vh.AddTriangle(idx, idx + 2, idx + 3);
        }
    }
}