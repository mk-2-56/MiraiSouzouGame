using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Rendering;
public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private List<CinemachineVirtualCamera> VirtualCameras;
    [SerializeField] private float[] switchTimes; // �؂�ւ��̃^�C�~���O�i�b�j

    void Start()
    {
        if (cinemachineBrain == null)
        {
            Debug.Log("Brain not found!");
        }
        if (VirtualCameras == null) return;
        for(int i = 0; i < VirtualCameras.Count; i++)
        {//��Ԗڂ���D�揇�ʂ�����
/*            VirtualCameras[i].Priority = VirtualCameras.Count - i + 1;
*/            VirtualCameras[i].Priority = 0;    
        }
        VirtualCameras[0].Priority = 10;

        StartCoroutine(SwitchVCameras());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SwitchVCameras()
    {
        for (int i = 0; i < VirtualCameras.Count; i++)
        {
            // �S�J������Priority�����Z�b�g
            ResetVCamerasPriority();

            // ���݂̃J�������A�N�e�B�u��
            VirtualCameras[i].Priority = 10;

            Debug.Log($"�J����{i + 1}���A�N�e�B�u�ɂȂ�܂���");

            // ���̐؂�ւ����Ԃ�҂�
            if (i < switchTimes.Length)
                yield return new WaitForSeconds(switchTimes[i]);
        }
    }

    private void ResetVCamerasPriority()
    {
        foreach (var cam in VirtualCameras)
        {
            cam.Priority = 0;
        }
    }
}
