using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    public struct MeteorProperties
    {
        public Vector3 Pos;
        public Vector3 SpawnPos;                                     // ���΂̃X�|�[���n�_
        public int Time;                                             // �X�|�[�������鎞��
        public bool Use;                                             // �g�p���t���O
        public bool WillUse;                                         // �g�p����\��t���O
        public bool OnGround;                                        // ���n�t���O
        public int CntDespawn;                                       // ���Ԍo�߃f�X�|�[���̃J�E���^�[
    }
    /******************************************************************************************************************************************************************/
    // �O���[�o���ϐ�
    /******************************************************************************************************************************************************************/
    // �p�u���b�N�ϐ�
    public static int MaxMeteor = 5;                                                     // ���΂̍ő吔
    public MeteorProperties[] MeteorProp = new MeteorProperties[MaxMeteor];              // ���΂̐ݒ�

    // �v���C�x�[�g�ϐ� 
    [SerializeField] private GameObject PrefabObj;                                       // ���΃I�u�W�F�N�g�̎擾
    private List<GameObject> Meteor = new List<GameObject>();                            // ���΃I�u�W�F�N�g���X�g
    private float Speed = 0.25f;                                                         // �������x
    private Vector3 Crater = new Vector3(0.0f, 0.0f, 0.0f);                              // ���Ό��̒��S���W
    private float Height = 50.0f;                                                        // ���΂̃X�|�[��y���W�A�������ŃX�|�[�������邩�A�Œ�l
    [SerializeField] private int Interval = 300;                                         // ���̕��΂̔����Ԋu
    private int SpawnTimer = 300;                                                        // �X�|�[���^�C�}�[
    // �����l�[���̕ϐ�
    private int MaxUsingMeteor = 0;                                                      // �i�v���C���[�����̈ʒu�ɂ����1����j���̔���Ŏg�p���镬�΂̍ő吔
    private bool AllStarted = false;                                                     // ���̔���Ŏg�p���镬�΂����ׂďo�����������t���O
    private int CntStartedMeteor = 0;                                                    // �o�����������΂̐��̃J�E���g
    private int EventNum = 0;                                                            // ���΃C�x���g�̔����񐔂̃J�E���g
    private int AddNumforPosTank = 0;                                                    // MeteorsPosTank�Q�Ɨp�J�E���g

    private Vector3[] MeteorsPosTank =                               // ����Position�̒�����
        {
            new Vector3(196.0f, 34.0f, 152.0f),
            new Vector3(220.0f, 36.0f, 151.0f),
            new Vector3(236.0f, 38.0f, 139.0f),

            new Vector3(330.0f, 45.0f, 131.0f),
            new Vector3(358.0f, 44.0f, 150.0f),
            new Vector3(372.0f, 46.0f, 153.0f),
            new Vector3(384.0f, 45.0f, 176.0f),

            new Vector3(0.0f, 0.0f, 90.0f),
            new Vector3(-5.0f, 0.0f, 100.0f),
            new Vector3(5.0f, 0.0f, 110.0f),
        };
    private int[] UseNumAtOnceTank =                                 // 1��̃C�x���g�ŃX�|�[�������镬�΂̐��̒�����
        {
            3,
            4,
            3,
        };
    private int MaxEventNum = 3;                                     // �ő�C�x���g���i��L��UseNumAtOnceTank�ɓ��͂��������̐��j

    private bool SpawnMeteor = false;                                // ���Δ����t���O

    // [SerializeField] ��Œǉ�����
    // testing




    /******************************************************************************************************************************************************************/
    // 
    /******************************************************************************************************************************************************************/
    private void Awake()
    {
        // �I�u�W�F�N�g�𐶐�����\���ŕۊǂ��Ă���
        for (int i = 0; i < MaxMeteor; i++)
        {
            GameObject obj = Instantiate(PrefabObj); // �I�u�W�F�N�g�𐶐�����
            Meteor.Add(obj); // ���X�g�ɒǉ�
            Meteor[i].SetActive(false); // ��\���ɂ���
            // ResetMeteorProperties(i); // ���΂̐ݒ��������
        }

        // �G���[�m�F�F�z��T�C�Y
        if (MeteorProp.Length != Meteor.Count)
        {
            Debug.Log("MeteorProp & Meteor.Length has different array num, crucial error may occur ");
        }

    }
    //private void ResetMeteorProperties(int index)
    //{
    //    MeteorProp[index] = new MeteorProperties
    //    {
    //        Pos = Vector3.zero,
    //        SpawnPos = Vector3.zero,
    //        Time = 0,
    //        Use = false,
    //        WillUse = false,
    //        OnGround = false,
    //        CntDespawn = 0
    //    };
    //}

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //debug
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ActivateMeteors();
            }
        }

        if (SpawnMeteor)
        {
            UpdateSpawnTimer();
            UpdateMeteors();
        }
    }

    private void UpdateSpawnTimer()
    {
        if (!AllStarted)
        {
            SpawnTimer++;
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                if (SpawnTimer >= MeteorProp[i].Time && MeteorProp[i].WillUse)
                {
                    SetMeteorUse();
                    CntStartedMeteor++;
                }
            }
            if (CntStartedMeteor >= MaxUsingMeteor)
            {
                AllStarted = true;
            }
        }
    }
    private void UpdateMeteors()
    {
        int allInactive = 0;    // ���ׂĂ̕��΂��g�p����Ă��Ȃ���Ԃ��m�F�����ׂ�Use�ł͂Ȃ��Ȃ�ASpawnMeteor�t���O��off�ɂ���
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].Use)
            {
                float radius = 0.1f;    // ���΃��f���̔��a
                if (MeteorProp[i].Pos.y < MeteorProp[i].SpawnPos.y - Height - radius)
                {
                    MeteorProp[i].OnGround = true;
                }
                else
                {
                    // ���΂𗎉�������
                    MeteorProp[i].Pos.y -= Speed;

                    // �Ό�����X�|�[���n�_�֌������v�Z
                    Vector3 targetDirection = MeteorProp[i].Pos - Crater;
                    targetDirection.y = 0; // Y�����͖�������

                    if (targetDirection.magnitude > 0.01f) // �����Ȓl���傫���ꍇ
                    {
                        targetDirection.Normalize(); // ���K�����ĕ����x�N�g���𓾂�
                        MeteorProp[i].Pos += targetDirection * Speed * 0.01f;
                    }
                }
            }
            else
            {
                allInactive++;  // ���ׂĂ̕��΂��s�g�p�ɂȂ������A�`�F�b�N
                Meteor[i].SetActive(false); // ��\���ɂ���
            }
        }
        if (allInactive >= MeteorProp.Length)
        {
            // �����A���ׂĂ̕��΂�Use�ł͂Ȃ��Ȃ�����A���Η����C�x���g���~������
            SpawnMeteor = false;
        }


        // �n�ʂɒ��n������A���Ԍo�߂Ńf�X�|�[��������
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].Use && MeteorProp[i].OnGround)
            {
                MeteorProp[i].CntDespawn++;
                if (MeteorProp[i].CntDespawn > 6000)
                {
                    MeteorProp[i].CntDespawn = 0;
                    MeteorProp[i].Use = false;
                }
            }
        }

        // ���΂̐ݒ���I�u�W�F�N�g�֔��f
        {
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                if (MeteorProp[i].Use)
                {
                    Meteor[i].transform.position = MeteorProp[i].Pos;
                }
            }
        }
    }




    /******************************************************************************************************************************************************************/
    // 
    /******************************************************************************************************************************************************************/

    // ���΂̃X�|�[���n�_�𐳊m�Ɍ��߂�
    private void SetMeteorPos(Vector3 pos)
    {
        int SpawnTime = 0;
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (!MeteorProp[i].WillUse)
            {
                MeteorProp[i].SpawnPos = new Vector3(pos.x, pos.y + Height, pos.z);
                MeteorProp[i].Time = SpawnTime;
                MeteorProp[i].WillUse = true;
                return;
            }
            SpawnTime += Interval;
        }

        Debug.Log("(SetMeteorPos) Array is full. Cannot set meteor. ");
    }
    // ���΂��g�p���ɂ���
    private void SetMeteorUse()
    {
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].WillUse && !MeteorProp[i].Use)
            {
                MeteorProp[i].WillUse = false;
                MeteorProp[i].Use = true;
                Meteor[i].SetActive(true); // �\������
                MeteorProp[i].Pos = MeteorProp[i].SpawnPos;
                return;
            }
        }

        Debug.Log("(SetMeteorUse) Array is full. Cannot set meteor. ");
    }
    // ���΃M�~�b�N������������
    private void InitMeteors()
    {
        if (EventNum >= MaxEventNum) return;    // �ő�C�x���g���ɒB������A���s���Ȃ�

        if (!SpawnMeteor)
        {
            SpawnMeteor = true;
            SpawnTimer = 0;
            CntStartedMeteor = 0;
            MaxUsingMeteor = UseNumAtOnceTank[EventNum];
            AllStarted = false;
            // �ڍׂȕ��΂̃X�|�[���n�_�����߂�
            for (int i = 0; i < MaxUsingMeteor; i++)
            {
                Vector3 pos = MeteorsPosTank[i + AddNumforPosTank];
                SetMeteorPos(pos);

                MeteorProp[i].OnGround = false; // ���n���菉����
                MeteorProp[i].CntDespawn = 0;
            }

            // �ŏ��̕��΂��o��������
            SetMeteorUse();
            EventNum++;
            AddNumforPosTank += MaxUsingMeteor;
        }
    }
    // ���΃M�~�b�N��Activate����
    public void ActivateMeteors()
    {
        if (SpawnMeteor)
        {
            // �O�񔭓����������΂��c���Ă�����A�������ď���������
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                MeteorProp[i].WillUse = false;
                if (MeteorProp[i].Use)
                {
                    MeteorProp[i].Use = false;
                    Meteor[i].SetActive(false); // ��\���ɂ���
                }
            }
            SpawnMeteor = false;
        }

        // �����A���ׂẴC�x���g���΂𗎂Ƃ�����A���[�v������
        if (EventNum >= MaxEventNum)
        {
            EventNum = 0;
            AddNumforPosTank = 0;
            CntStartedMeteor = 0;
        }

        InitMeteors();
    }


    // �Q�b�^�[�E�Z�b�^�[
    public List<GameObject> GetInstantiatedMeteors()
    {
        return Meteor; // �C���X�^���X�������I�u�W�F�N�g�̃��X�g��Ԃ�
    }
    public int GetMaxMeteor()
    {
        return MaxMeteor;
    }
    public MeteorProperties GetMeteorProperties(int num)
    {
        return MeteorProp[num];
    }
}

