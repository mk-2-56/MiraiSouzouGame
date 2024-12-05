using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Funka : MonoBehaviour
{
    [SerializeField] private float interval = 10f; // ランダム値をチェックする間隔（秒）
    [SerializeField] private AudioSource funkaAudioSource;
    [SerializeField] private uint funkaPerTimes = 10;
    [SerializeField] private float PerTime = 0.1f;

    private int minValue = 0;
    private int maxValue = 10; // 修正：範囲を0〜1にする
    private int targetValue = 5;

    void Start()
    {
        StartCoroutine(FunkaTrigger());
    }

    private IEnumerator FunkaTrigger()
    {
        while (true)
        {
            int randomValue = Random.Range(minValue, maxValue);

            if (randomValue == targetValue)
            {
                VisualEffect vfx = GetComponent<VisualEffect>();
                if (vfx != null)
                {
                    StartCoroutine(FunkaPerTime(vfx));
                }
                else
                {
                    Debug.LogWarning("VisualEffectコンポーネントが見つかりません！");
                }

                if (funkaAudioSource != null && funkaAudioSource.clip != null)
                {
                    Transform parentTransform = this.transform.parent;
                    if (parentTransform != null)
                    {
                        AudioSource.PlayClipAtPoint(funkaAudioSource.clip, parentTransform.position);
                    }
                }
                else
                {
                    Debug.LogWarning("AudioSourceまたはそのクリップが設定されていません！");
                }
            }

            yield return new WaitForSeconds(interval); // 指定した間隔で次のチェックを実行
        }
    }

    private IEnumerator FunkaPerTime(VisualEffect vfx)
    {
        vfx.SendEvent("Funka");
        yield return new WaitForSeconds(PerTime);
    }
}