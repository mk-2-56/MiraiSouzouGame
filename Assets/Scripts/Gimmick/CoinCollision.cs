using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public CoinScore coinScore;
    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter()
    {
        //if (collision.gameObject.CompareTag("Player"))
        {
            coinScore.AddCoin();
            //����������
            Destroy(gameObject);            
        }
    }
}
