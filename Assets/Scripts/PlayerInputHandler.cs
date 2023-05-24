using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float _horizontalsensitivity;
    public float _verticalsensitivity;
    public static PlayerInputHandler instance;

    private void Awake()
    {
        instance = this;
    }

    public float GetHorizontalMovement()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    
    public float GetVerticalMovement()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public float GetHorizontalLookInput()
    {
        return Input.GetAxisRaw("Mouse X");
    } 
    
    public float GetVerticalLookInput()
    {
        return Input.GetAxisRaw("Mouse Y");
    }
    
    public bool GetSprintInputHeld()
    {
        return Input.GetButton("Sprint");
    }
    
    public bool GetJumpInputDown()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetFireInput()
    {
        return Input.GetButton("Fire1");
    }
    
    public bool GetMeleeAttackInput()
    {
        return Input.GetButton("Fire2");
    }
}
