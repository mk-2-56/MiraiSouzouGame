using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class PlayerStanby : MonoBehaviour
    {
        public bool stanby;


        void Stanby()
        {
            CC.Hub CChub = GetComponent<CC.Hub>();

            stanby = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            stanby = false;

            CC.Hub CChub = GetComponent<CC.Hub>();
            CChub.StanbyEvent += Stanby;
            
        }

    }
}