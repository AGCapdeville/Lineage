using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuAnimation : MonoBehaviour
{
    public float duration = 2f;
    public float delay = 1f;
    private void Start()
    {
        CanvasGroup canvasgroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasgroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasgroup, 1.0f, duration).setDelay(1f);
    }
}