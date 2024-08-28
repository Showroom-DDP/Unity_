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

    UserInfo currentInfo = new UserInfo();

    public void InAccount(string id, string name, int age, string gender, int familly)
    {
        currentInfo.userId = id;
        currentInfo.userName = name;
        currentInfo.userAge = Convert.ToInt32(age);
        currentInfo.userGender = gender;
        currentInfo.userFamilly = Convert.ToInt32(familly);
    }

    private void Start()
    {
        
    }
}

