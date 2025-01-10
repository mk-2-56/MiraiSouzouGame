using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    // �w�莞�Ԍ�ɔ�A�N�e�B�u
    private IEnumerator DeactivateObjectAfterTime()
    {
        // �w�肵�����ԑҋ@
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.SE_Pubble);
        StartCoroutine(DeactivateObjectAfterTime());
    }

}
