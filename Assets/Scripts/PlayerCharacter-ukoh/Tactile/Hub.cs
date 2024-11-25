using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEditor;
/// <summary>
/// ukoh-2024-10-28
/// ÉvÉåÉCÉÑÅ[Controller ì¸óÕÇ‚stateä«óùÇ»Ç«Ç™
/// 
/// 
/// 
/// </summary>


namespace CC
{
    using System;
    [CustomEditor(typeof(Hub))]
    public class HubUI : Editor
    {
        SerializedProperty timescale;
        SerializedProperty disableInput;

        void OnEnable()
        { 
            timescale    = serializedObject.FindProperty("_timescale");
            disableInput = serializedObject.FindProperty("_disableInput");
        }
        public override void OnInspectorGUI()
        { 
            DrawDefaultInspector();
            EditorGUILayout.Slider(timescale, 0.01f, 1.0f, new GUIContent("TimeScale"));
            if (GUILayout.Button(new GUIContent("DsiableInput")))
            { disableInput.boolValue = !disableInput.boolValue;}

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class Hub : MonoBehaviour/*PlayerInputActions.IPlayerActions*/
    {
        public bool disableInput
        { 
            set{ _disableInput = value;}
        }

        public void FreezePlayer()
        {
            _oldVelocity = _rRb.velocity;
            _rRb.isKinematic = true;
        }

        public void UnfreezePlayer()
        {
            _rRb.isKinematic = false;
            _rRb.velocity = _oldVelocity;
        }

        public event System.Action<Vector3> MoveEvent;
        public event System.Action<Vector2> LookEvent;
        public event System.Action JumpStartEvent;
        public event System.Action JumpEndEvent;
        public event System.Action BoostStartEvent;
        public event System.Action BoostEndEvent;
        public event System.Action DriftStartEvent;
        public event System.Action DriftEndEvent;
        public event System.Action DashEvent;

        public delegate void AdditionFixedOperation(Rigidbody sender, Quaternion terrianRot);
        public event AdditionFixedOperation FixedEvent;

        [SerializeField] bool  _disableInput;
        [SerializeField] float _timescale = 1;

        Rigidbody _rRb;
        Vector2 _moveRawInput;
        Vector2 _lookRawInput;

        PlayerMovementParams _rMovementParams;

        float _timescaleOld;
        Vector3 _oldVelocity;

        public void OnDash(InputAction.CallbackContext context)
        {
            if(_disableInput)
                return;
            DashEvent();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (_disableInput)
                return;
            switch (context.phase)
            { 
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    JumpStartEvent();
                    break;
                case InputActionPhase.Canceled:
                    JumpEndEvent();
                    break;
            }
        }
        public void OnBoost(InputAction.CallbackContext context)
        {
            if (_disableInput)
                return;
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    BoostStartEvent();
                    break;
                case InputActionPhase.Canceled:
                    BoostEndEvent();
                    break;
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        { }
        public void OnPrevious(InputAction.CallbackContext context)
        { }
        public void OnNext(InputAction.CallbackContext context)
        { }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (_disableInput)
            {
                _moveRawInput = Vector2.zero;
                return;
            }
            _moveRawInput = context.ReadValue<Vector2>();
            MoveEvent(_moveRawInput);
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            if (_disableInput)
            {
                _moveRawInput = Vector2.zero;
                return;
            }
            _lookRawInput = context.ReadValue<Vector2>();
            LookEvent(_lookRawInput);
        }

        public void OnDrift(InputAction.CallbackContext ctx)
        {
            if (_disableInput)
                return;
            switch (ctx.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    DriftStartEvent();
                    break;
                case InputActionPhase.Canceled:
                    DriftEndEvent();
                    break;
            }
        }

        private void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rMovementParams = GetComponent<CC.Basic>().GetPlayerMovementParams();
        }

        private void Update()
        {
            if(_timescale != _timescaleOld)
                Time.timeScale = _timescale;
        }

        private void FixedUpdate()
        {
            FixedEvent?.Invoke(_rRb, _rMovementParams.terrianRotation);
        }
    }
}