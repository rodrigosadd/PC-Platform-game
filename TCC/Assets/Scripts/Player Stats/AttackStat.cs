﻿using UnityEngine;
using FMODUnity;

public class AttackStat : Stats
{
    void Update()
    {
        RotateObject();
        CheckPickedUp();
        CountdownToReturnPlayerTarget();
    }

    public void CheckPickedUp()
    {
        float _distanceBetween = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if(_distanceBetween < maxDistancePickedUp && 
            Input.GetButtonDown("Interact") &&
            PlayerController.instance.movement.canMove &&
            !PlayerAttackController.instance.attaking &&
            PlayerAttackController.instance.currentAttack == 0 &&
            PlayerController.instance.jump.currentJump <= 0)
        {
            RuntimeManager.PlayOneShot(collectSound, transform.position);
            GameManager.instance.playerStatsData.canAttack = 1;
            GameManager.instance.playerStatsData.ApplySettings();
            GameManager.instance.savePlayerStats.Save();
            SeeObjectDrop();
        }
    } 

#if UNITY_EDITOR
     void OnDrawGizmos()
     {
          Gizmos.color = Color.yellow;
          Gizmos.DrawWireSphere(transform.position, maxDistancePickedUp);
     }
#endif
}
