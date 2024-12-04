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
    // 指定時間後に非アクティブ
    private IEnumerator DeactivateObjectAfterTime()
    {
        // 指定した時間待機
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.SE_Pubble);
        StartCoroutine(DeactivateObjectAfterTime());
    }

}
