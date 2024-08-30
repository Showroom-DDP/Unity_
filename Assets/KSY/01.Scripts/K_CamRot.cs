﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_CamRot : MonoBehaviour
{
    public bool cam;
    public float rotSpeed;
    float my;
    float mx;


    void Update()
    {
        if (cam)
        {
            my -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
            my = Mathf.Clamp(my, -80f, 80f);
            
        }
        else
        {
            mx += Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
            //transform.localEulerAngles += new Vector3(0, mx * Time.deltaTime* rotSpeed, 0);
        }
        transform.localEulerAngles = new Vector3(my, mx, 0);
    }
}
