using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static AccountDate;

public class FirstCanvasManager : MonoBehaviour
{
    public TMP_InputField texttId;
    public TMP_InputField textPassword;
    public TMP_InputField textName;
    public TMP_InputField textAge;
    public TextMeshProUGUI textGender;
    public TextMeshProUGUI textFamilly;

    public TMP_InputField logintId;
    public TMP_InputField loginPassword;

    public GameObject joinFullset;
    public GameObject loginFullset;

    public Animator warningAnim;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 1);
    }

    public void JoinFinishClick()
    {
        if(string.IsNullOrEmpty(texttId.text) || string.IsNullOrEmpty(textPassword.text) || string.IsNullOrEmpty(textName.text) || string.IsNullOrEmpty(textAge.text)) 
        {
            print("ĭ�� �����");
            HttpManager.GetInstance().Alert("��ĭ�� ä���ּ���.");
            return;
        }

        JoinJsonConvert();

        LoginPopUp();
    }

    public void LoginClick()
    {
        if (string.IsNullOrEmpty(logintId.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            print("ĭ�� �����");
            HttpManager.GetInstance().Alert("���̵� �� ��й�ȣ�� �Է����ּ���.");
            return;
        }

        LoginJsonConvert();
    }

    void JoinJsonConvert()
    {
        // ������ ������ ��ü ����
        UserInfo userInfo = new UserInfo();
        userInfo.userId = texttId.text;
        userInfo.userPassword = textPassword.text;
        userInfo.userName = textName.text;
        userInfo.userAge = Convert.ToInt32(textAge.text);
        userInfo.userGender = textGender.text;
        userInfo.userFamilly = Convert.ToInt32(textFamilly.text);

        // HttpInfo ��ü ����
        HttpInfo info = new HttpInfo();

        // ��û�� URL ����
        info.url = "-";

        // ������ �����͸� JSON �������� ��ȯ�Ͽ� ����
        info.body = JsonUtility.ToJson(userInfo);

        print(info);

        // ������ Ÿ�� ����
        info.contentType = "application/json";

        //��������Ʈ�� �׳� �ֱ� - ���ٽ� ���  - ���� ���⼱ ���� �ܰ� ����
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            // �����κ��� ���� ���� ���
            print(downloadHandler.text);
        };

        // POST ��û�� ���� �ڷ�ƾ ����
        StartCoroutine(HttpManager.GetInstance().Register(info));
    }

    void LoginJsonConvert()
    {
        // ������ ������ ��ü ����
        UserAccount accountInfo = new UserAccount();
        accountInfo.userId = logintId.text;
        accountInfo.userPassword = loginPassword.text;

        // HttpInfo ��ü ����
        HttpInfo info = new HttpInfo();

        // ��û�� URL ����
        info.url = "-";

        // ������ �����͸� JSON �������� ��ȯ�Ͽ� ����
        info.body = JsonUtility.ToJson(accountInfo);

        print(info);

        // ������ Ÿ�� ����
        info.contentType = "application/json";

        //��������Ʈ�� �׳� �ֱ� - ���ٽ� ���  - ���� ���⼱ ���� �ܰ� ����
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            // �����κ��� ���� ���� ���
            print(downloadHandler.text);
        };

        // POST ��û�� ���� �ڷ�ƾ ����
        StartCoroutine(HttpManager.GetInstance().Login(info));
    }

    public void JoinPopUp()
    {
        logintId.text = "";
        loginPassword.text = "";

        joinFullset.SetActive(true);
        loginFullset.SetActive(false);
    }

    public void LoginPopUp()
    {
        texttId.text = "";
        textPassword.text = "";
        textName.text = "";
        textAge.text = "";

        joinFullset.SetActive(false);
        loginFullset.SetActive(true);
    }
}
