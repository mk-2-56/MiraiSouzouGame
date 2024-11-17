using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PressStartText : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>()
            .DOFade(0.0f, 1.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }
}
