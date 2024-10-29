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
    }
    // *********************************************************************************************************
    // �O���[�o���ϐ�

    // �p�u���b�N�ϐ�
    public static int MaxMeteor = 5;                                                     // ���΂̍ő吔
    public MeteorProperties[] MeteorProp = new MeteorProperties[MaxMeteor];              // ���΂̐ݒ�

    // �v���C�x�[�g�ϐ� 
    [SerializeField] private GameObject PrefabObj;                                       // ���΃I�u�W�F�N�g�̎擾
    private List<GameObject> Meteor = new List<GameObject>();                            // ���΃I�u�W�F�N�g���X�g
    private float Speed = 0.02f;                                                         // �������x
    private Vector3 Crater = new Vector3(-2.0f, 0.0f, -30.0f);                             // ���Ό��̒��S���W
    private float Height = 20.0f;                                                        // ���΂̃X�|�[��y���W�A�������ŃX�|�[�������邩�A�Œ�l
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
            new Vector3(0.0f, 0.0f, -10.0f),
            new Vector3(-5.0f, 0.0f, 0.0f),
            new Vector3(5.0f, 0.0f, 10.0f),

            new Vector3(0.0f, 0.0f, 20.0f),
            new Vector3(-5.0f, 0.0f, 30.0f),
            new Vector3(5.0f, 0.0f, 40.0f),
            new Vector3(0.0f, 0.0f, 50.0f),

            new Vector3(-5.0f, 0.0f, 60.0f),
            new Vector3(0.0f, 0.0f, 70.0f),
            new Vector3(5.0f, 0.0f, 80.0f),
        };
    private int[] UseNumAtOnceTank =                                 // 1��̃C�x���g�ŃX�|�[�������镬�΂̐��̒�����
        {
            3,
            4,
            3,
        };
    private int MaxEventNum = 3;                                     // �ő�C�x���g���i��L��UseNumAtOnceTank�ɓ��͂��������̐��j



    // [SerializeField] ��Œǉ�����
    // testing
    private bool SpawnMeteor = false;                                // ���Δ����t���O
    private int CntDespawn = 0;


    private void Awake()
    {
        // �I�u�W�F�N�g�𐶐�����\���ŕۊǂ��Ă���
        for (int i = 0; i < MaxMeteor; i++)
        {
            GameObject obj = Instantiate(PrefabObj); // �I�u�W�F�N�g�𐶐�����
            Meteor.Add(obj); // ���X�g�ɒǉ�
            Meteor[i].SetActive(false); // ��\���ɂ���
        }
        // �ݒ�̏�����
        for (int i = 0; i < MaxMeteor; i++)
        {
            MeteorProp[i].Pos = new Vector3(0, 0, 0);
            MeteorProp[i].SpawnPos = new Vector3(0, 0, 0);
            MeteorProp[i].Time = 0;
            MeteorProp[i].Use = false;
            MeteorProp[i].WillUse = false;
            MeteorProp[i].OnGround = false;
        }
        // ���̑��A�ϐ��̏�����
        AllStarted = false;
        MaxUsingMeteor = 0;
        CntStartedMeteor = 0;
        EventNum = 0;
        AddNumforPosTank = 0;

        // �G���[�m�F�F�z��T�C�Y
        if (MeteorProp.Length != Meteor.Count)
        {
            Debug.Log("MeteorProp & Meteor.Length has different array num, crucial error may occur ");
        }

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (EventNum >= MaxEventNum)
            {
                EventNum = 0;
                AddNumforPosTank = 0;
                CntStartedMeteor = 0;
            }
            // 1�L�[�������ꂽ��ASpawnMeteor�t���O��On�ɂ��āA���΂̏����ݒ������
            ActivateMeteors();
        }

        if (SpawnMeteor)
        {
            if (!AllStarted)
            {
                SpawnTimer++; // �^�C�}�[���X�V

                for (int i = 0; i < MeteorProp.Length; i++)
                {
                    // Interval�ɒB�����玟�̕��΂��X�|�[��
                    if (SpawnTimer >= MeteorProp[i].Time && MeteorProp[i].WillUse)
                    {
                        SetMeteorUse();
                        CntStartedMeteor++;
                    }
                }

                if (CntStartedMeteor >= MaxUsingMeteor) AllStarted = true;
            }



            int allInactive = 0;    // ���ׂĂ̕��΂��g�p����Ă��Ȃ���Ԃ��m�F
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                if (MeteorProp[i].Use)
                {
                    float Ground = 0.0f;    // �n�\�̍��x
                    if (MeteorProp[i].Pos.y < Ground)
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
                            MeteorProp[i].Pos += targetDirection * (Speed);
                        }
                    }
                }
                else
                {
                    allInactive++;
                }
            }
            if (allInactive >= MeteorProp.Length)
            {
                // �����A���ׂĂ̕��΂��������I������A���Η����C�x���g���~������
                SpawnMeteor = false;
            }

            int CntOnGroundMeteor = 0;
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                // ���΂����n������A
                if (MeteorProp[i].OnGround)
                {
                    CntOnGroundMeteor++;
                }
            }
            if (CntOnGroundMeteor >= MaxUsingMeteor)
            {
                CntDespawn++;
                if (CntDespawn > 3000)
                {
                    for (int i = 0; i < MaxUsingMeteor; i++)
                    {
                        // ���΂�����
                        MeteorProp[i].OnGround = false;
                        MeteorProp[i].Use = false;
                        Meteor[i].SetActive(false); // ��\���ɂ���
                    }
                }
            }
        }
        // ���΂̐ݒ���I�u�W�F�N�g�֔��f
        {
            int Index = 0;
            while (Index < MeteorProp.Length)
            {
                if (MeteorProp[Index].Use)
                {
                    Meteor[Index].transform.position = MeteorProp[Index].Pos;
                }
                Index++;
            }
        }
    }

    // ���΂̃X�|�[���n�_�𐳊m�Ɍ��߂�
    public void SetMeteorPos(Vector3 pos)
    {
        int Index = 0;
        int SpawnTime = 0;
        while (Index < MeteorProp.Length)
        {
            if (!MeteorProp[Index].WillUse)
            {
                MeteorProp[Index].SpawnPos = new Vector3(pos.x, Height, pos.z);
                MeteorProp[Index].Time = SpawnTime;
                MeteorProp[Index].WillUse = true;
                return;
            }
            Index++;
            SpawnTime += Interval;
        }

        Debug.Log("(SetMeteorPos) Array is full. Cannot set meteor. ");
    }
    // ���΂��g�p���ɂ���
    public void SetMeteorUse()
    {
        int Index = 0;
        while (Index < MeteorProp.Length)
        {
            if (MeteorProp[Index].WillUse && !MeteorProp[Index].Use)
            {
                MeteorProp[Index].WillUse = false;
                MeteorProp[Index].Use = true;
                Meteor[Index].SetActive(true); // �\������
                MeteorProp[Index].Pos = MeteorProp[Index].SpawnPos;
                return;
            }
            Index++;
        }

        Debug.Log("(SetMeteorUse) Array is full. Cannot set meteor. ");
    }
    // ���΃M�~�b�N��Activate����
    public void ActivateMeteors()
    {
        if (EventNum >= MaxEventNum) return;    // �ő�C�x���g���ɒB������A���s���Ȃ�

        if (!SpawnMeteor)
        {
            SpawnMeteor = true;
            // �ϐ��̏�����
            SpawnTimer = 0;
            CntStartedMeteor = 0;
            MaxUsingMeteor = UseNumAtOnceTank[EventNum];
            AllStarted = false;
            CntDespawn = 0;
            // �ڍׂȕ��΂̃X�|�[���n�_�����߂�
            for (int i = 0; i < MaxUsingMeteor; i++)
            {
                Vector3 pos = MeteorsPosTank[i + AddNumforPosTank];
                SetMeteorPos(pos);
            }

            // �ŏ��̕��΂��o��������
            SetMeteorUse();

            EventNum++;
            AddNumforPosTank += MaxUsingMeteor;
        }
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

