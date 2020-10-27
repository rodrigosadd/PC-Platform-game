﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
     public int maxCombo;
     public int currentAttack;
     public float delayNextAttack;
     public bool attaking;
     private float _currentMaxSpeed;
     private float _lastAttackTime;

     void Update()
     {
          InputsAttack();
          CanAttack();
     }

     public void InputsAttack()
     {
          if (Input.GetButtonDown("Fire1") && PlayerController.instance.movement.slowed == false)
          {
               if (!attaking)
               {
                    _currentMaxSpeed = PlayerController.instance.movement.fixedMaxSpeed;
               }
               FirstAttack();
          }
     }

     public void FirstAttack()
     {
          _lastAttackTime = Time.time;
          currentAttack++;

          if (currentAttack == 1)
          {
               PlayerController.instance.animator.SetBool("First Attack", true);
          }
          currentAttack = Mathf.Clamp(currentAttack, 0, maxCombo);
     }

     public void SecondAttack()
     {
          if (currentAttack >= 2)
          {
               PlayerController.instance.animator.SetBool("Second Attack", true);
          }
          else
          {
               PlayerController.instance.animator.SetBool("First Attack", false);
               PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
               attaking = false;
               currentAttack = 0;
          }
     }

     public void FinalAttack()
     {
          if (currentAttack >= 3)
          {
               PlayerController.instance.animator.SetBool("Final Attack", true);
          }
          else
          {
               PlayerController.instance.animator.SetBool("Second Attack", false);
               currentAttack = 0;
          }
     }

     public void ResetAttack()
     {
          PlayerController.instance.animator.SetBool("First Attack", false);
          PlayerController.instance.animator.SetBool("Second Attack", false);
          PlayerController.instance.animator.SetBool("Final Attack", false);
          PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
          attaking = false;
          currentAttack = 0;
     }

     public void Attaking()
     {
          attaking = true;
          if (PlayerController.instance.movement.slowed == false)
          {
               PlayerController.instance.movement.maxSpeed = 2f;
          }
     }

     public void CanAttack()
     {
          if (Time.time - _lastAttackTime > delayNextAttack)
          {
               currentAttack = 0;
          }
     }
}