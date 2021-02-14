﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObjectsDoor : OpenDoor
{
     public BreakableObject[] breakableObjects;
     private Vector3 _targetInitialLPos;
     private Vector3 _targetInitialRPos;

     void Start()
     {
          StartPositionTargets();
     }

     void Update()
     {
          CheckObjectsBroken();
     }

     public void CheckObjectsBroken()
     {
          bool _isComplete = true;

          for (int i = 0; i < breakableObjects.Length; i++)
          {
               if (!breakableObjects[i].triggerBroken)
               {
                    _isComplete = false;
                    break;
               }
          }

          if (_isComplete)
          {
               CanOpenDoor();
          }
     }
}
