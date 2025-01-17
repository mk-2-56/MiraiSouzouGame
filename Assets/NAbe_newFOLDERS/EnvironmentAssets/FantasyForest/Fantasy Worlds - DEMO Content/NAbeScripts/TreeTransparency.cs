using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTransparency : MonoBehaviour
{
    //private Transform player; // プレイヤーのTransform
    //[SerializeField] private float fadeDistance = 5f; // 透明化が始まる距離
    //[SerializeField] private float fadeSpeed = 2f; // フェードのスピード

    //private Renderer treeRenderer;
    //private Material treeMaterial;
    //private float currentTransparency = 1f;

    //void Start()
    //{
    //    treeRenderer = GetComponent<Renderer>();
    //    if (treeRenderer != null)
    //    {
    //        treeMaterial = treeRenderer.material;
    //    }

    //    player = transform.Find("Facing");
    //}

    //void Update()
    //{
    //    if (treeMaterial == null || player == null) return;

    //    // プレイヤーと木の距離を計算
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    // 距離によって透明度を計算
    //    if (distance < fadeDistance)
    //    {
    //        currentTransparency = Mathf.Lerp(currentTransparency, 0f, Time.deltaTime * fadeSpeed);
    //    }
    //    else
    //    {
    //        currentTransparency = Mathf.Lerp(currentTransparency, 1f, Time.deltaTime * fadeSpeed);
    //    }

    //    // シェーダーの透明度プロパティを更新
    //    treeMaterial.SetFloat("_Transparency", currentTransparency);
    //}

    [Range(0, 1)] public float alpha = 1f; // アルファ値をインスペクタから設定可能
    private Material _material;

    void Start()
    {
        // オブジェクトのMaterialを取得（インスタンス化することで他のオブジェクトに影響しない）
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;

            // レンダリングモードをTransparentに変更
            SetMaterialToTransparent(_material);
        }
    }

    void Update()
    {
        if (_material != null)
        {
            // Materialの色を取得してAlpha値を設定
            Color color = _material.color;
            color.a = alpha; // 透明度を調整
            _material.color = color;
        }
    }

    // マテリアルのレンダリングモードをTransparentに設定する関数
    private void SetMaterialToTransparent(Material material)
    {
        material.SetFloat("_Mode", 3); // 標準シェーダーでは3がTransparentモード
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = 3000;
    }
}
