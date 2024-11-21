using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public class Hub : MonoBehaviour/*PlayerInputActions.IPlayerActions*/
    {

        public event System.Action<Vector3> MoveEvent;
        public event System.Action<Vector2> LookEvent;
        public event System.Action JumpStartEvent;
        public event System.Action JumpEndEvent;
        public event System.Action BoostStartEvent;
        public event System.Action BoostEndEvent;
        public event System.Action DriftStartEvent;
        public event System.Action DriftEndEvent;
        public event System.Action DashEvent;

        PlayerInputActions _playerInput;

        Vector2 _moveRawInput;
        Vector2 _lookRawInput;

        public void OnDash(InputAction.CallbackContext context)
        {
            DashEvent();
        }
        public void OnJump(InputAction.CallbackContext context)
        { 
            switch(context.phase)
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
            _moveRawInput = context.ReadValue<Vector2>();
            MoveEvent(_moveRawInput);
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            _lookRawInput = context.ReadValue<Vector2>();
            LookEvent(_lookRawInput);
        }

        public void OnDrift(InputAction.CallbackContext ctx)
        {
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
            //if(_playerInput == null)
            //{ 
            //    _playerInput = new PlayerInputActions();
            //    _playerInput.Player.SetCallbacks(instance:this);
            //}
            //_playerInput.Player.Enable();
        }
    }
}