/**
Author: Bryan Gregory Soh

Name of Class: allows object to be infront of the player

Description of Class: checks the players position and rotation and offsets it to be in the front of the camara

Date Created: 17/02/2022
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followThePlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void FixedUpdate()
    {
        //checks for the camara position and adjuct the object accordingly
        transform.position = target.position + Vector3.up * offset.y +
            Vector3.ProjectOnPlane(target.right, Vector3.up).normalized  * offset.x +
            Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * offset.z;

        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}
