using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTransparency : MonoBehaviour
{
    //private Transform player; // �v���C���[��Transform
    //[SerializeField] private float fadeDistance = 5f; // ���������n�܂鋗��
    //[SerializeField] private float fadeSpeed = 2f; // �t�F�[�h�̃X�s�[�h

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

    //    // �v���C���[�Ɩ؂̋������v�Z
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    // �����ɂ���ē����x���v�Z
    //    if (distance < fadeDistance)
    //    {
    //        currentTransparency = Mathf.Lerp(currentTransparency, 0f, Time.deltaTime * fadeSpeed);
    //    }
    //    else
    //    {
    //        currentTransparency = Mathf.Lerp(currentTransparency, 1f, Time.deltaTime * fadeSpeed);
    //    }

    //    // �V�F�[�_�[�̓����x�v���p�e�B���X�V
    //    treeMaterial.SetFloat("_Transparency", currentTransparency);
    //}

    [Range(0, 1)] public float alpha = 1f; // �A���t�@�l���C���X�y�N�^����ݒ�\
    private Material _material;

    void Start()
    {
        // �I�u�W�F�N�g��Material���擾�i�C���X�^���X�����邱�Ƃő��̃I�u�W�F�N�g�ɉe�����Ȃ��j
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;

            // �����_�����O���[�h��Transparent�ɕύX
            SetMaterialToTransparent(_material);
        }
    }

    void Update()
    {
        if (_material != null)
        {
            // Material�̐F���擾����Alpha�l��ݒ�
            Color color = _material.color;
            color.a = alpha; // �����x�𒲐�
            _material.color = color;
        }
    }

    // �}�e���A���̃����_�����O���[�h��Transparent�ɐݒ肷��֐�
    private void SetMaterialToTransparent(Material material)
    {
        material.SetFloat("_Mode", 3); // �W���V�F�[�_�[�ł�3��Transparent���[�h
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = 3000;
    }
}
