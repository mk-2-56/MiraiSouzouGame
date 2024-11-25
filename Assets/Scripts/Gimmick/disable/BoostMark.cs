using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,
        Act1,
        Act2,
        Ending
    }
    public class MarkProperties
    {
        public MarkState markState;                                  // UI�̃X�e�[�g���
        public int Cnt;                                              // �J�E���^�[
        public bool Usable;                                          // �v���C���[���p�\�t���O

        // �R���X�g���N�^
        public MarkProperties()
        {
            markState = MarkState.Waiting;
            Cnt = 0;
            Usable = false;
        }
    }
    private int waitAppear = 10;                                     // �\��
    private int waitExpand = 120;                                    // �g��

    /******************************************************************************************************************************************************************/
    // �O���[�o���ϐ�
    /******************************************************************************************************************************************************************/

    // �p�u���b�N�ϐ�
    public static int MaxMarkUI = 10;                                               // UI�̍ő吔(maxMeteor��2�{�͎g�p��)
    public MarkProperties[] MarkProp = new MarkProperties[MaxMarkUI];               // UI�̐ݒ�
    public ObjectPool BoostMarkPool;

    // �v���C�x�[�g�ϐ� 
    [SerializeField] private MeteorMovement meteorMovement;                         // ���΂̏����擾

    private MeteorMovement.MeteorProperties[] meteorProperties;                     // ���ΐݒ�̏����擾
    private Transform MainCam;                                                      // �J�����̏��
    private Transform WorldSpaceCanvas;                                             // WorldSpaceCanvas�̏��



    // Start is called before the first frame update
    void Start()
    {

        MainCam = Camera.main.transform;                                                // �J�����̏����擾

        WorldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpace").transform;    // UI�̐e�����擾

        for (int i = 0; i < MaxMarkUI; i++)
        {
            BoostMarkPool.GetList()[i].transform.SetParent(WorldSpaceCanvas); // �e�q�֌W��ݒ�
            BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5); // �T�C�Y��ݒ�(����2,2,2)
            MarkProp[i] = new MarkProperties();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // �擾�ł��Ȃ��ꍇ�A���s���Ȃ��i�G���[���O�Ώ��̂��߁j
        if (meteorMovement == null)
        {
            return;
        }

        // ���΂̐ݒ�����擾
        int maxMeteor = MeteorMovement.MaxMeteor;
        meteorProperties = new MeteorMovement.MeteorProperties[maxMeteor];

        for (int i = 0; i < maxMeteor; i++)
        {
            meteorProperties[i] = meteorMovement.GetMeteorProperties(i);
        }
        // ���΃I�u�W�F�N�g�̏����擾
        List<GameObject> instantiatedMeteors = meteorMovement.GetInstantiatedMeteors();



        if (instantiatedMeteors.Count >= maxMeteor)
        {
            for (int i = 0; i < maxMeteor; i++)
            {
                // ��苗���߂Â�����A�\��������
                float distance = Vector3.Distance(MainCam.transform.position, instantiatedMeteors[i].transform.position);
                if (meteorProperties[i].Use && distance <= 40.0f)
                {
                    float val = 1.0f / waitExpand * 4;
                    switch (MarkProp[i].markState)
                    {
                        case MarkState.Waiting:
                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitAppear)
                            {
                                MarkProp[i].markState = MarkState.Act1;
                                MarkProp[i + maxMeteor].markState = MarkState.Act1;
                                // �ݒ�̏�����
                                MarkProp[i].Cnt = 0;
                            }
                            break;
                        case MarkState.Act1:
                            MarkProp[i].Usable = true;
                            BoostMarkPool.GetList()[i].SetActive(true); // �\������
                            BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);
                            //BoostMarkPool.GetList()[i].SetActive(true); // �\������
                            //BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Act2;
                                // �ݒ�̏�����
                                MarkProp[i].Cnt = 0;
                                BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);

                                //BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);
                            }

                            // �A�j���[�V�����Ȃ��̃}�[�N
                            BoostMarkPool.GetList()[i + maxMeteor].SetActive(true); // �\������
                            BoostMarkPool.GetList()[i + maxMeteor].transform.localScale = new Vector3(1, 1, 1);
                            break;
                        case MarkState.Act2:
                            BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Ending;
                                BoostMarkPool.GetList()[i].SetActive(false); // ��\���ɂ���
                                // �ݒ�̏�����
                                MarkProp[i].Cnt = 0;
                            }
                            break;
                        case MarkState.Ending:

                            break;
                    }


                    // �I�u�W�F�N�g�̑O����UI��\�������邽�߁Aposition�𒲐�
                    Vector3 targetDirection;
                    {
                        targetDirection = BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position;
                        //targetDirection.y = 0; // Y�����͖�������

                        if (targetDirection.magnitude > 0.01f) // �����Ȓl���傫���ꍇ
                        {
                            targetDirection.Normalize(); // ���K�����ĕ����x�N�g���𓾂�
                            targetDirection += targetDirection * 2.0f;  // �ǂꂭ�炢�O���ɕ\�������邩�A�����Œ���(2.0f)
                        }
                    }

                    BoostMarkPool.GetList()[i].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    BoostMarkPool.GetList()[i].transform.rotation = Quaternion.LookRotation(BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position);
                    // �A�j���[�V�����Ȃ��̃}�[�N
                    BoostMarkPool.GetList()[i + maxMeteor].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    BoostMarkPool.GetList()[i + maxMeteor].transform.rotation = Quaternion.LookRotation(BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position);
                }
                else
                {
                    BoostMarkPool.GetList()[i].SetActive(false); // ��\���ɂ���
                    BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i].markState = MarkState.Waiting;
                    MarkProp[i].Usable = false;

                    // �A�j���[�V�����Ȃ��̃}�[�N
                    BoostMarkPool.GetList()[i + maxMeteor].SetActive(false); // �\������
                    BoostMarkPool.GetList()[i + maxMeteor].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i + maxMeteor].markState = MarkState.Waiting;
                }
            }
        }
        else
        {
            Debug.LogWarning("Not enough instantiated Meteors.");
        }

    }


    // �u�[�X�g�}�[�N���p�\�t���O�Q�b�^�[�i�v���C���[�u�[�X�g�p�j
    public (bool, Vector3) ObjectBoostUsable()
    {
        bool usable = false;
        Vector3 pos = new Vector3(0, 0, 0);


        Vector3 cameraForward = Camera.main.transform.forward; // �J�����̑O���x�N�g�����擾
        float closestAngle = float.MaxValue; // �ł��������p�x�������邽�߂̏����l


        for (int i = 0; i < MaxMarkUI; i++)
        {
            if (MarkProp[i].Usable)
            {
                // �I�u�W�F�N�g�ւ̃x�N�g�����擾
                Vector3 directionToObj = (BoostMarkPool.GetList()[i].transform.position - Camera.main.transform.position).normalized;

                // �p�x���v�Z
                float angle = Vector3.Angle(cameraForward, directionToObj);

                // �ŏ��̊p�x�����I�u�W�F�N�g��������
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    pos = BoostMarkPool.GetList()[i].transform.position;
                }

                usable = true;
            }
        }

        return (usable, pos);
    }
}