using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AccountDate : MonoBehaviour
{
    //�̱��� ����
    public static AccountDate instance;

    // �̱��� �ν��Ͻ��� ��ȯ�ϴ� �޼ҵ�
    public static AccountDate GetInstance()
    {
        if (instance == null)
        {
            // ���ο� ���� ������Ʈ ����
            GameObject go = new GameObject();

            // �̸� ����
            go.name = "HttpManager";

            // HttpManager ������Ʈ�� �߰�
            go.AddComponent<AccountDate>();
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            // �ν��Ͻ� ����
            instance = this;

            // �� ��ȯ �� ��ü �ı� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
            Destroy(gameObject);
        }
    }

    // ȸ�� ���Խ� ���� ����
    [System.Serializable]
    public struct UserInfo
    {
        public string userId;
        public string userPassword;
        public string userName;
        public int userAge;
        public string userGender;
        public int userFamilly;
    }

    //�α��� �� ���� ����
    [System.Serializable]
    public struct UserAccount
    {
        public string userId;
        public string userPassword;
    }

    [System.Serializable]
    public struct UserLoginInfo
    {
        public string userName;
        public string userId;
    }

    UserLoginInfo currentInfo = new UserLoginInfo();

    //�α��� �� ���� ����
    public void InAccount(string token, string name)
    {
        currentInfo.userId = token;
        currentInfo.userName = name;
    }

    private void Start()
    {
        
    }
}

