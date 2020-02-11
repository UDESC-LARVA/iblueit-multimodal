using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.LeavesGame
{
    public class Rotator : MonoBehaviour
    {
        public float velocity = -1.5f;
        void Update()
        {
            //transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
            transform.Translate(new Vector3(velocity * Time.deltaTime, 0));
        }
    }
}
