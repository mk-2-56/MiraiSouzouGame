using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorKinematicFalser : MonoBehaviour
{
    private Vector3[] originalPositions; // ���̈ʒu��ۑ�����z��
    private Quaternion[] originalRotations; // ���̉�]��ۑ�����z��
    private Vector3[] originalVelocities; // ���̑��x��ۑ�����z��
    private Vector3[] originalAngularVelocities; // ���̊p���x��ۑ�����z��
    
    private MeteorMovement meteorMovement; // MeteorMovement�̎Q�Ƃ�ǉ�

    private bool started = false; // �J�n���̏��������I���Ă���ResetKinematic()�����s�����邽��
    private int despawnCnt = 0; // �j���ɏ��ł���܂ł̃J�E���^�[
    private int meteoNum = 0; // ���ł����镬�΂̔ԍ�
    private bool cntDespawn = false;
    [SerializeField] private int despawnTime = 1500; // �Փˌ�A���ł���܂ł̎���


    void Start()
    {
        // �q�I�u�W�F�N�g�̌��̈ʒu���L�^
        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        originalPositions = new Vector3[childRbs.Length];
        originalRotations = new Quaternion[childRbs.Length];
        originalVelocities = new Vector3[childRbs.Length];
        originalAngularVelocities = new Vector3[childRbs.Length];

        for (int i = 0; i < childRbs.Length; i++)
        {
            originalPositions[i] = childRbs[i].transform.position;
            originalRotations[i] = childRbs[i].transform.rotation; // ��]���L�^
            originalVelocities[i] = childRbs[i].velocity; // ���x���L�^
            originalAngularVelocities[i] = childRbs[i].angularVelocity; // �p���x���L�^
        }

        // ���O�ŃI�u�W�F�N�g��������
        GameObject meteorSpawner = GameObject.Find("MeteorsSpawner");
        // MeteorMovement�R���|�[�l���g���擾
        meteorMovement = meteorSpawner.GetComponent<MeteorMovement>();

        started = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�ƏՓ˂��������m�F
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collision is ok");
            // �q�I�u�W�F�N�g�̂��ׂĂ�Rigidbody���擾
            Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody childRb in childRbs)
            {
                // isKinematic��false�ɂ���
                if (childRb != null)
                {
                    childRb.isKinematic = false;

                    //// �͂������Ă����Ɣ�юU�点��
                    //var random = new System.Random();
                    //var min = -8;
                    //var max = 8;
                    //var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

                    //childRb.AddForce(vect, ForceMode.Impulse);
                    //childRb.AddTorque(vect, ForceMode.Impulse);
                }
            }

            // �v���C���[�ƏՓˌ�A���Ԍo�߂ŏ��ł�����
            if (meteorMovement != null)
            {
                for (int i = 0; i < meteorMovement.MeteorProp.Length; i++)
                {
                    float distanceThreshold = 5.0f;// �ʒu���߂����ǂ����𔻒肷��

                    if (Vector3.Distance(meteorMovement.MeteorProp[i].Pos, transform.position) < distanceThreshold)
                    {
                        meteoNum = i;
                        cntDespawn = true;
                        break;
                    }
                }
            }

            //// �����蔻�������
            //Collider[] childCol = GetComponentsInChildren<Collider>();
            //foreach (Collider childCo in childCol)
            //{
            //    if (childCo != null)
            //    {
            //        childCo.isTrigger = true;
            //    }
            //}
        }
    }

    void Update()
    {
        //�f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetKinematic();
        }
        if (cntDespawn)
        {
            AfterBreakDespawnTimer();
        }
    }

    void OnEnable()
    {
        if (started)
        {
            ResetKinematic();   // �\�����ꂽ��A�I�u�W�F�N�g�j��Ŕ�юU�����j�Ђ����Z�b�g����

            //// �����������蔻��𕜊�������
            //Collider[] childCol = GetComponentsInChildren<Collider>();
            //foreach (Collider childCo in childCol)
            //{
            //    if (childCo != null)
            //    {
            //        childCo.isTrigger = false;
            //    }
            //}
        }
    }

    public void ResetKinematic()
    {
        //Debug.Log("reset is ok");

        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childRbs.Length; i++)
        {
            if (childRbs[i] != null)
            {
                // ���̈ʒu�ɖ߂�
                childRbs[i].transform.position = originalPositions[i];
                // ��]�����̉�]�ɖ߂�
                childRbs[i].transform.rotation = originalRotations[i];
                // �������͂����Z�b�g����
                if (!childRbs[i].isKinematic)
                {
                    childRbs[i].velocity = originalVelocities[i]; // ���̑��x�ɖ߂�
                    childRbs[i].angularVelocity = originalAngularVelocities[i]; // ���̊p���x�ɖ߂�
                }

                // isKinematic��true�ɖ߂�
                childRbs[i].isKinematic = true;
            }
        }
    }

    private void AfterBreakDespawnTimer()
    {
        despawnCnt++;
        if (despawnCnt > despawnTime)
        {
            despawnCnt = 0;
            meteorMovement.MeteorProp[meteoNum].Use = false;
            cntDespawn = false;
        }
    }
}
