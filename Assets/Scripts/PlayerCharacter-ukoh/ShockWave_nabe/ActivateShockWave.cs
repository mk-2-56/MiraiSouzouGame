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
            // エフェクトが再生中かどうかを確認
            if (particleEffect.aliveParticleCount > 0)
            {
                // 再生中なら、一旦停止
                particleEffect.Stop();
            }

            // 強制的に再生をリセットして開始
            particleEffect.Reinit();
            particleEffect.Play();
        }
    }
}
