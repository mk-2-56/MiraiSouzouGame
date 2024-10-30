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

    // *********************************************************************************************************
    // �O���[�o���ϐ�

    // �p�u���b�N�ϐ�
    public static int MaxMarkUI = 10;                                               // UI�̍ő吔(maxMeteor��2�{�͎g�p��)
    public MarkProperties[] MarkProp = new MarkProperties[MaxMarkUI];               // UI�̐ݒ�

    // �v���C�x�[�g�ϐ� 
    [SerializeField] private MeteorMovement meteorMovement;                         // ���΂̏����擾
    [SerializeField] private GameObject PrefabUIObj_Cir;                            // UI�I�u�W�F�N�g�̎擾
    private List<GameObject> MarkUI_Cir = new List<GameObject>();                   // UI�I�u�W�F�N�g���X�g
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
            GameObject obj = Instantiate(PrefabUIObj_Cir); // �I�u�W�F�N�g�𐶐�����
            MarkUI_Cir.Add(obj); // ���X�g�ɒǉ�
            MarkUI_Cir[i].SetActive(false); // ��\���ɂ���
            MarkUI_Cir[i].transform.SetParent(WorldSpaceCanvas); // �e�q�֌W��ݒ�
            MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5); // �T�C�Y��ݒ�(����2,2,2)

            MarkProp[i] = new MarkProperties();
        }
        if (PrefabUIObj_Cir == null)
        {
            Debug.LogError("PrefabUI is not assigned!");
            return;
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
                            MarkUI_Cir[i].SetActive(true); // �\������
                            MarkUI_Cir[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Act2;
                                // �ݒ�̏�����
                                MarkProp[i].Cnt = 0;
                                MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5);
                            }

                            // �A�j���[�V�����Ȃ��̃}�[�N
                            MarkUI_Cir[i + maxMeteor].SetActive(true); // �\������
                            MarkUI_Cir[i + maxMeteor].transform.localScale = new Vector3(1, 1, 1);
                            break;
                        case MarkState.Act2:
                            MarkUI_Cir[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Ending;
                                MarkUI_Cir[i].SetActive(false); // ��\���ɂ���
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
                        targetDirection = MarkUI_Cir[i].transform.position - MainCam.transform.position;
                        //targetDirection.y = 0; // Y�����͖�������

                        if (targetDirection.magnitude > 0.01f) // �����Ȓl���傫���ꍇ
                        {
                            targetDirection.Normalize(); // ���K�����ĕ����x�N�g���𓾂�
                            targetDirection += targetDirection * 2.0f;  // �ǂꂭ�炢�O���ɕ\�������邩�A�����Œ���(2.0f)
                        }
                    }

                    MarkUI_Cir[i].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    MarkUI_Cir[i].transform.rotation = Quaternion.LookRotation(MarkUI_Cir[i].transform.position - MainCam.transform.position);
                    // �A�j���[�V�����Ȃ��̃}�[�N
                    MarkUI_Cir[i + maxMeteor].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    MarkUI_Cir[i + maxMeteor].transform.rotation = Quaternion.LookRotation(MarkUI_Cir[i].transform.position - MainCam.transform.position);
                }
                else
                {
                    MarkUI_Cir[i].SetActive(false); // ��\���ɂ���
                    MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i].markState = MarkState.Waiting;
                    MarkProp[i].Usable = false;

                    // �A�j���[�V�����Ȃ��̃}�[�N
                    MarkUI_Cir[i + maxMeteor].SetActive(false); // �\������
                    MarkUI_Cir[i + maxMeteor].transform.localScale = new Vector3(5, 5, 5);
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

        // �J�����̑O���x�N�g�����擾
        Vector3 cameraForward = Camera.main.transform.forward;
        float closestAngle = float.MaxValue; // �ł��������p�x�������邽�߂̏����l
        Vector3 pos = new Vector3(0, 0, 0);

        for (int i = 0; i < MaxMarkUI; i++)
        {
            if (MarkProp[i].Usable)
            {
                // �I�u�W�F�N�g�ւ̃x�N�g�����擾
                Vector3 directionToObj = (MarkUI_Cir[i].transform.position - Camera.main.transform.position).normalized;

                // �p�x���v�Z
                float angle = Vector3.Angle(cameraForward, directionToObj);

                // �ŏ��̊p�x�����I�u�W�F�N�g��������
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    pos = MarkUI_Cir[i].transform.position;
                }

                usable = true;
            }
        }

        return (usable, pos);
    }
}




//public class Example : MonoBehaviour
//{
//    void Start()
//    {
//        var result = GetBoolAndVector();
//        bool myBool = result.Item1;
//        Vector3 myVector = result.Item2;

//        Debug.Log($"Bool: {myBool}, Vector: {myVector}");
//    }

//    (bool, Vector3) GetBoolAndVector()
//    {
//        bool someCondition = true; // �C�ӂ̏���
//        Vector3 someVector = new Vector3(1, 2, 3); // �C�ӂ�Vector3

//        return (someCondition, someVector);
//    }
//}