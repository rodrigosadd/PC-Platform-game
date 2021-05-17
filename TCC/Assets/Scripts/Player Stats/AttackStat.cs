﻿using UnityEngine;
using FMODUnity;

public class AttackStat : Stats
{
    void Update()
    {
        RotateObject();
        CheckPickedUp();
    }

    public void CheckPickedUp()
    {
        float _distanceBetween = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if(_distanceBetween < maxDistancePickedUp && Input.GetButtonDown("Interact"))
        {
            RuntimeManager.PlayOneShot(collectSound, transform.position);
            GameManager.instance.playerStatsData.canAttack = 1;
            GameManager.instance.playerStatsData.ApplySettings();
            GameManager.instance.savePlayerStats.Save();
            gameObject.SetActive(false);
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