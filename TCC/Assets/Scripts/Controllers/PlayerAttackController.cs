﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
     public static PlayerAttackController instance;

     public GameObject pickaxe;
     public Transform targetAttack;
     public LayerMask layerObjs;
     public GameObject[] trails;
     public int maxCombo;
     public int currentAttack;
     public float delayNextAttack;
     public float maxDistanceAttack;
     public float attackImpulse;
     public float distanceImpulse;
     public float timeToResetAttack;
     public bool attaking;
     public bool canAttack;
     private float _countdownReset;

// #if UNITY_EDITOR
//      public bool seeAttackRange;
// #endif

     private float _currentMaxSpeed;
     private float _lastAttackTime;
     private Vector3 _finalImpulse;

     void Awake()
     {
          instance = this;
     }

     void LateUpdate()
     {
          CheckPlayerCanToSeePickaxe();

          if(canAttack)
          {
               InputsAttack();
               CanAttack();
               Impulse();
               CheckAttaking();
          }
     }

     public void CheckPlayerCanToSeePickaxe()
     {
          if(canAttack)
          {
               pickaxe.SetActive(true);
          }
          else
          {
               pickaxe.SetActive(false);
          }
     }

     public void InputsAttack()
     {
          if (Input.GetButtonDown("Attack") &&
              !GameManager.instance.settingsData.settingsOpen &&
              !PlayerController.instance.levelMechanics.slowing &&
              !PlayerController.instance.death.dead &&
              !PlayerController.instance.push.pushingObj &&
              !PlayerController.instance.push.setPositionDropObject &&
              !PlayerController.instance.push.droppingObj &&
              !PlayerController.instance.levelMechanics.sliding &&
              PlayerController.instance.movement.isGrounded &&
              PlayerController.instance.movement.canMove)
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
          if (!PlayerController.instance.death.dead &&
               currentAttack != 3 &&
               PlayerController.instance.movement.isGrounded)
          {
               _lastAttackTime = Time.time;
               currentAttack++;

               if (currentAttack == 1)
               {
                    PlayerAnimationController.instance.SetFirstAttack();
                    trails[0].SetActive(true);
                    trails[1].SetActive(true);
                    trails[2].SetActive(true);
               }
               currentAttack = Mathf.Clamp(currentAttack, 0, maxCombo);
          }
     }

     public void SecondAttack()
     {
          if (!PlayerController.instance.death.dead && PlayerController.instance.movement.canMove)
          {
               if (currentAttack >= 2)
               {
                    PlayerAnimationController.instance.SetSecondAttack();
                    trails[0].SetActive(true);
                    trails[1].SetActive(true);
                    trails[2].SetActive(true);
               }
               else
               {
                    PlayerAnimationController.instance.ResetFirstAttack();
                    trails[0].SetActive(false);
                    trails[1].SetActive(false);
                    trails[2].SetActive(false);
                    PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
                    attaking = false;
                    currentAttack = 0;
                    _finalImpulse = Vector3.zero;
               }
          }
     }

     public void FinalAttack()
     {
          if (!PlayerController.instance.death.dead && PlayerController.instance.movement.canMove)
          {
               if (currentAttack >= 3)
               {
                    PlayerAnimationController.instance.SetFinalAttack();
                    trails[0].SetActive(true);
                    trails[1].SetActive(true);
                    trails[2].SetActive(true);
               }
               else
               {
                    PlayerAnimationController.instance.ResetFirstAttack();
                    PlayerAnimationController.instance.ResetSecondAttack();
                    trails[0].SetActive(false);
                    trails[1].SetActive(false);
                    trails[2].SetActive(false);
                    PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
                    attaking = false;
                    currentAttack = 0;
                    _finalImpulse = Vector3.zero;
               }
          }
     }

     public void ResetAttack()
     {
          PlayerAnimationController.instance.ResetAttacks();
          trails[0].SetActive(false);
          trails[1].SetActive(false);
          trails[2].SetActive(false);
          PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
          attaking = false;
          currentAttack = 0;
          _finalImpulse = Vector3.zero;
     }

     public void CheckAttaking()
     {
          if (currentAttack != 0)
          {
               if (_countdownReset < 1)
               {
                    _countdownReset += Time.deltaTime / timeToResetAttack;
               }
               else
               {
                    _countdownReset = 0;
                    ResetAttack();
               }
          }
          else
          {
               _countdownReset = 0;
          }
     }

     public void Attacking()
     {
          if (!PlayerController.instance.death.dead)
          {
               if (PlayerController.instance.levelMechanics.slowing == false)
               {
                    PlayerController.instance.movement.maxSpeed = 0f;
                    attaking = true;
               }
          }
     }

     public void Impulse()
     {
          if (attaking)
          {
               if (_finalImpulse == Vector3.zero)
               {
                    _finalImpulse = PlayerController.instance.transform.position + (PlayerController.instance.characterGraphic.forward * distanceImpulse);
               }
               else
               {
                    if (!Physics.Raycast(PlayerController.instance.characterGraphic.position, PlayerController.instance.characterGraphic.forward, 1.18f))
                    {
                         PlayerController.instance.movement.velocity.y = 0;
                         PlayerController.instance.SetControllerPosition(Vector3.MoveTowards(PlayerController.instance.transform.position, _finalImpulse, attackImpulse * Time.deltaTime));

                         if (_finalImpulse == PlayerController.instance.transform.position)
                         {                              
                              _finalImpulse = Vector3.zero;
                              PlayerController.instance.levelMechanics.sliding = false;
                              attaking = false;
                         }
                    }
                    else
                    {
                         _finalImpulse = Vector3.zero;
                         PlayerController.instance.levelMechanics.sliding = false;
                         attaking = false;
                    }
               }
          }
     }

     public void AttackDetection()
     {
          Collider[] _hitObject = Physics.OverlapSphere(targetAttack.position, maxDistanceAttack, layerObjs);

          foreach (Collider _hit in _hitObject)
          {
               if (_hit.tag == "Enemy")
               {
                    _hit.transform.GetComponent<Enemy>().TakeHit();
               }
               else if (_hit.tag == "Breakable")
               {
                    _hit.transform.GetComponent<BreakableObject>().TakeHit();
               }
               else if(_hit.tag == "Boss")
               {
                    _hit.transform.GetComponentInParent<BossController>().TakeDamage();
               }
          }
     }

     public void CanAttack()
     {
          if (Time.time - _lastAttackTime > delayNextAttack)
          {
               currentAttack = 0;
          }
     }

// #if UNITY_EDITOR
//      void OnDrawGizmos()
//      {
//           if (seeAttackRange)
//           {
//                Gizmos.color = Color.blue;
//                Gizmos.DrawWireSphere(targetAttack.position, maxDistanceAttack);
//           }
//      }
// #endif
}
