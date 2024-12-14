using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ActivateShockWave : MonoBehaviour
{
    [SerializeField] private VisualEffect particleEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Execute();
        }
    }

    public void Execute()
    {
        if (particleEffect.aliveParticleCount > 0)
        {
            // �Đ����Ȃ�A��U��~
            particleEffect.Stop();
        }

        // �����I�ɍĐ������Z�b�g���ĊJ�n
        particleEffect.Reinit();
        particleEffect.Play();
        // �G�t�F�N�g���Đ������ǂ������m�F
    }
}
