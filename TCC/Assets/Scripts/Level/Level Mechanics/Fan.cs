﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
     public float fanSpeed;

     void Update()
     {
          RotateFan();
     }

     public void RotateFan()
     {
          this.gameObject.transform.Rotate(0, fanSpeed, 0);
     }
}
