using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GaugeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Image image;
    private void Start()
    {
        image.fillAmount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetGaugeValue(0.1f);
        }
    }

    public void SetGaugeValue(float value)
    {
        image.fillAmount += value;
    }
}
