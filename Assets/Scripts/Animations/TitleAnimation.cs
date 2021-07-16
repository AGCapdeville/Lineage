using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class TitleAnimation : MonoBehaviour
{
    public float duration = 2f;
    private void Start()
    {
        CanvasGroup canvasgroup = this.gameObject.GetComponent<CanvasGroup>();
        TextMeshProUGUI infoTextTMPro = this.gameObject.GetComponent<TextMeshProUGUI>();

        canvasgroup.alpha = 0f;
        canvasgroup.transform.position = new Vector2(Screen.width/2, Screen.height);

        LeanTween.alphaCanvas(canvasgroup, 1.0f, duration).setDelay(0.5f);
        LeanTween.moveY(canvasgroup.GetComponent<RectTransform>(), 150f, duration).setEase(LeanTweenType.easeInOutCubic);
    }
}