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
    public class Hub : MonoBehaviour, PlayerInputActions.IPlayerActions
    {

        public event System.Action<Vector2> MoveEvent;
        public event System.Action<Vector2> LookEvent;
        public event System.Action JumpStartEvent;
        public event System.Action JumpEndEvent;

        PlayerInputActions _playerInput;







        Vector2 _moveRawInput;
        Vector2 _lookRawInput;


        public void OnDash(InputAction.CallbackContext context)
        { }
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
            Debug.Log("Jump:" + context.phase.ToString());
        }
        public void OnBoost(InputAction.CallbackContext context)
        { }
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
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

        private void Start()
        {
            if(_playerInput == null)
            { 
                _playerInput = new PlayerInputActions();
                _playerInput.Player.SetCallbacks(instance:this);
            }
            _playerInput.Player.Enable();
        }


        private void Update()
        {
            AU.Debug.Log(_moveRawInput, AU.LogTiming.Update);
            AU.Debug.Log(_lookRawInput, AU.LogTiming.Update);
        }
        private void FixedUpdate()
        {
        }
    }
}