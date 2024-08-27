using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AccountDate : MonoBehaviour
{
    //�̱��� ����
    static AccountDate instance;

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

    // PostInfo ����ü ����: �������� ������ �����͸� ��� ���� ����ü
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

    [System.Serializable]
    public struct UserAccount
    {
        public string userId;
        public string userPassword;
    }

    public void InAccount()
    {
        // ������ ������ ��ü ����
        UserInfo currentInfo = new UserInfo();
        currentInfo.userId = "";
        currentInfo.userPassword = "";
        currentInfo.userName = "";
        currentInfo.userAge = Convert.ToInt32("");
        currentInfo.userGender = "";
        currentInfo.userFamilly = Convert.ToInt32("");
    }
}

