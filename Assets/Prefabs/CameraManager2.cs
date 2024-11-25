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
    [SerializeField] private float[] switchTimes; // 切り替えのタイミング（秒）

    void Start()
    {
        if (cinemachineBrain == null)
        {
            Debug.Log("Brain not found!");
        }
        if (VirtualCameras == null) return;
        for(int i = 0; i < VirtualCameras.Count; i++)
        {//一番目から優先順位をつける
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
            // 全カメラのPriorityをリセット
            ResetVCamerasPriority();

            // 現在のカメラをアクティブ化
            VirtualCameras[i].Priority = 10;

            Debug.Log($"カメラ{i + 1}がアクティブになりました");

            // 次の切り替え時間を待つ
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
