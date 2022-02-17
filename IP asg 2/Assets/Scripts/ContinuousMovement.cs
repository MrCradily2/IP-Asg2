/**
Author: Bryan Gregory Soh

Name of Class: character movement

Description of Class: this using the vr controller and produces movement using the left to move and the right to rotate the player.

Date Created: 8/02/2022
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float Speed = 1;
    public XRNode inputSource;

    private XRRig origin;
    private Vector2 inputAxis;
    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        origin = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }
    private void FixedUpdate()
    {
        //gets player rotation
        Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);

        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * Speed);
    }
}
