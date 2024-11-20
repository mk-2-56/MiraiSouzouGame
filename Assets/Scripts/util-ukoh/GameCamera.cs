using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ukoh-2024-10-28
/// GameCamera  プレイヤーカメラ
/// 
/// シンプルなカメラ制御なだけ
/// todo inertia機能
/// </summary>

public class GameCamera : MonoBehaviour
{
    public void SetPlayerReference(GameObject pc)
    {
        _rFacing    = pc.transform.Find("Facing");
        _rCamFacing = pc.transform.Find("CamFacing");

        _rCog       = _rFacing.Find("Cog");

        _rCChub     = pc.GetComponent<CC.Hub>();
        _rCChub.LookEvent += Look;

        transform.SetParent(null);
    }

    //_____________Parameters
    [SerializeField] float param_lerpSpeed  = 0.8f;
    [SerializeField] float param_lerpSpeedPos = 0.95f;
    [SerializeField] bool  param_locking = true;

    //_____________Members
    CC.Hub      _rCChub;
    Transform   _rFacing;
    Transform   _rCog;
    Transform   _rCamFacing;

    Vector2 _camRotation = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent)
        {
            SetPlayerReference(transform.parent.gameObject);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            param_locking = !param_locking;
    }

    private void FixedUpdate()
    {
        float dtime = Time.fixedDeltaTime;
        transform.position = Vector3.Lerp( transform.position, _rCog.position, param_lerpSpeedPos * dtime);

        if(param_locking)
        {
            Quaternion tarRot; 
            if (_rCog.position - transform.position != Vector3.zero)
                tarRot = Quaternion.LookRotation(_rCog.position - transform.position);
            else
                tarRot = _rFacing.rotation;
            transform.rotation = _rCamFacing.rotation = 
                Quaternion.Lerp(_rCamFacing.rotation, tarRot, param_lerpSpeed * dtime);
            Vector3 tmp= transform.rotation.eulerAngles;
            _camRotation = new Vector2(tmp.y, -tmp.x);
            AU.Debug.Log(_camRotation, AU.LogTiming.Fixed);
        }

    }

    void Look(Vector2 input)
    {
        if (param_locking)
            return;

        _camRotation += input;
        _camRotation.y = Mathf.Clamp(_camRotation.y, -70.0f, 85.0f);

        _rCamFacing.rotation = Quaternion.Euler(0, _camRotation.x, 0);
        transform.rotation   = Quaternion.Euler(-_camRotation.y, _camRotation.x, 0);
    }
}
