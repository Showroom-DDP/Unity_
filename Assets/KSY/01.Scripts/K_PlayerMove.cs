﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class K_PlayerMove : MonoBehaviourPun
{
    public enum PlayerState
    {
        Move,
        Click
    }
    public PlayerState currState;
    public float moveSpeed;
    Animator myAnim;
    Transform myBody;
    public GameObject[] bodys;
    bool lockmode;

    Vector3 dir;
    float v;
    float h;

    void Start()
    {
        currState = PlayerState.Move;
        lockmode = true;
        myAnim = GetComponentInChildren<Animator>();
        myBody = transform.GetChild(0);
        SetCursorLock();
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currState == PlayerState.Move) ChangeState(PlayerState.Click);
            else if (currState == PlayerState.Click) ChangeState(PlayerState.Move);
            //lockmode = !lockmode;
            //SetCursorLock();
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        switch (currState)
        {
            case PlayerState.Move:
                Move();
                break;
            case PlayerState.Click:
                break;
        }
    }


    public void ChangeState(PlayerState state)
    {
        currState = state;
        switch (currState)
        {
            case PlayerState.Move:
                Cursor.lockState = CursorLockMode.Locked;
                K_UIManager.GetInstance().img_Aim.SetActive(true);
                break;
            case PlayerState.Click:
                if(myAnim!=null) myAnim.SetBool("Move", false);
                Cursor.lockState = CursorLockMode.Confined;
                K_UIManager.GetInstance().img_Aim.SetActive(false);
                break;
            default:
                break;
        }
    }

    void Move()
    {
        
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        dir = new Vector3(h, 0, v);
        if (myAnim == null) myAnim = GetComponentInChildren<Animator>();
        if(dir.magnitude > 0)
        {
            myAnim?.SetBool("Move", true);
        }
        else
        {
            myAnim?.SetBool("Move", false);
        }
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
    }

    void SetCursorLock()
    {
        if (lockmode)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }


    public void SetAvatar(GameObject bodyObject)
    {
        if (myBody.childCount > 0) Destroy(myBody.GetChild(0).gameObject);
        GameObject go = Instantiate(bodyObject, myBody);
        go.transform.localPosition = Vector3.zero;
        myAnim = GetComponentInChildren<Animator>();
    }

    public void ChangeToMove()
    {
        ChangeState(PlayerState.Move);
    }
}
