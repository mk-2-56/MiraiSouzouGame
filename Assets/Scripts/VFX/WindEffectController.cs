using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetComponent<PlayerEffectDispatcher>().BoostStartE += ActiveWindEffect;
        transform.parent.GetComponent<PlayerEffectDispatcher>().BoostEndE += DisableWindEffect;

        DisableWindEffect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveWindEffect()
    {
        this.enabled = true;
    }

    public void DisableWindEffect()
    {
        this.enabled = false;
    }
}
