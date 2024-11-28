using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    private bool OnGround = false;

    void Start()
    {
        OnGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OnGround && gameObject.activeSelf)
        {
            StartCoroutine(DeactivateObjectAfterTime());
        }

    }
    // 指定時間後に非アクティブ
    private IEnumerator DeactivateObjectAfterTime()
    {
        // 指定した時間待機
        yield return new WaitForSeconds(5f);

        OnGround = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnGround = true;

    }

}
