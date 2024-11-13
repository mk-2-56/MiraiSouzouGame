using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CCT
{ 
    public struct ActionFlags
    {
        public bool noInput;
        public bool boosting;
        public bool jumping;
        public bool drifting;
        public bool launching;
    }
}

public class CCT_Hub : MonoBehaviour
{
    struct InputActions
    { 
        public InputAction moveAction;
        public InputAction lookAction;
        public InputAction dashAction;
        public InputAction driftAction;
        public InputAction jumpAction;
        public InputAction boostAction;
    }
}
