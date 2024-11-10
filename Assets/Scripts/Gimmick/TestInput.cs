using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    // Start is called before the first frame update
    public ObjectFactory ObjectFactory;
    [SerializeField] int numberOfPebbles = 10;
    [SerializeField] float explosionForce = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            TriggerExplosionEffect();
/*            ObjectFactory.generateObj(ObjectType.objType_Pebble);
*/        }
    }

    void TriggerExplosionEffect()
    {
        for (int i = 0; i < numberOfPebbles; i++)
        {
            // ���΂𐶐�
            GameObject pebble = ObjectFactory.generateObj(
                ObjectType.objType_Pebble,
                transform.position, // 覐΂̈ʒu����ɐ���
                Random.rotation     // �����_���ȉ�]
            );

            Rigidbody rb = pebble.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = pebble.AddComponent<Rigidbody>();
            }

            // �����_���ȕ����x�N�g���𐶐�
            Vector3 randomDirection = Random.onUnitSphere; // ���a1�̋��ʏ�̃����_���Ȉʒu���擾
            rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse); // �����_�������ɗ͂�������
        }
    }
}
