using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FocusEffectController : MonoBehaviour
{ 
    public PlayerEffectDispatcher effectDispatcher
    {
        set { dispatcher = value; }
    }

    [SerializeField] private FullScreenPassRendererFeature postProcess;
    private PlayerEffectDispatcher dispatcher;

    // Start is called before the first frame update
    private void Start()
    {
        dispatcher.BoostStartE  += ActiveFocusEffect;
        dispatcher.BoostEndE    += DisableFocusEffect;

        DisableFocusEffect();

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnApplicationQuit()
    {
        postProcess.SetActive(false);

    }

    private void ActiveFocusEffect()
    {
        postProcess.SetActive(true);
    }

    private void DisableFocusEffect()
    {
        postProcess.SetActive(false);
    }
}
