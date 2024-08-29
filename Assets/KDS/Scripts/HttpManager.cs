using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using static AccountDate;

// HttpInfo Ŭ���� ����: HTTP ��û ���� ������ ��� Ŭ����
public class HttpInfo
{
    // ��û�� URL
    public string url = "";

    // ������ ������
    public string body = "";

    // ������ Ÿ��
    public string contentType = "";

    // ��û�� �Ϸ�Ǹ� ȣ��� ��������Ʈ
    public Action<DownloadHandler> onComplete;
}

public class HttpManager : MonoBehaviour
{

    public GameObject alertFullset;
    public TextMeshProUGUI alertText;

    //�̱��� ����
    static HttpManager instance;

    // �̱��� �ν��Ͻ��� ��ȯ�ϴ� �޼ҵ�
    public static HttpManager GetInstance()
    {
        if (instance == null)
        {
            // ���ο� ���� ������Ʈ ����
            GameObject go = new GameObject();

            // �̸� ����
            go.name = "HttpManager";

            // HttpManager ������Ʈ�� �߰�
            go.AddComponent<HttpManager>();
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

    //GET : �������� �����͸� ��ȸ ��û
    public IEnumerator Get(HttpInfo info)
    {
        // GET ��û ����
        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            // ��û ���� �� ���� ���
            yield return webRequest.SendWebRequest();

            // ��û �Ϸ� �� ó��
            DoneRequest(webRequest, info);
        }
    }

    //Post : �����͸� ������ ����
    public IEnumerator Post(HttpInfo info)
    {
        //����Ƽ�� http ��� ���
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            //������ ��û ������
            yield return webRequest.SendWebRequest();

            //�������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    /*
    public IEnumerator Login(HttpInfo info)
    {
        // GET ��û ����
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            // ��û ���� �� ���� ���
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseUserInfo(webRequest.downloadHandler);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("Login failed: " + webRequest.error);
            }
        }
    }
    */

    public IEnumerator Login(HttpInfo info)
    {
        // GET ��û ����
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(info.body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", info.contentType);

            // ��û ���� �� ���� ���
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseUserInfo(webRequest.downloadHandler);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
                Alert("�α��� ����!");
            }
            else
            {
                Debug.Log("Login failed: " + webRequest.error);
                Alert("���̵� Ȥ�� ��й�ȣ�� Ʋ�Ƚ��ϴ�.");
            }
        }
    }

    public IEnumerator Register(HttpInfo info)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(info.body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", info.contentType);

            // ��û ���� �� ���� ���
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registration successful: " + webRequest.downloadHandler.text);
                Alert("ȸ�� ���� ����");
            }
            else
            {
                Debug.Log("Registration failed: " + webRequest.error);
                Alert("ȸ�� ���� ����");
            }
        }
    }


    // ���� ���ε带 form-data�� ó���ϴ� �ڷ�ƾ
    public IEnumerator UploadFileByFormData(HttpInfo info)
    {
        //info.data�� �ִ� ������ byte �迭�� �о����
        byte[] data = File.ReadAllBytes(info.body);

        //data�� MultipartForm���� ����
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, "image.jpg", info.contentType));

        //����Ƽ�� http ��� ���
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            //������ ��û ������
            yield return webRequest.SendWebRequest();

            //�������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    //���� ���ε�
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        //info.data�� �ִ� ������ byte �迭�� �о����
        byte[] data = File.ReadAllBytes(info.body);

        //����Ƽ�� http ��� ���
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            //���ε��ϴ� ������
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            //����޴� ������ ����
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            //������ ��û ������
            yield return webRequest.SendWebRequest();

            //�������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    //��������Ʈ �ޱ�
    public IEnumerator DownloadSprite(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(info.url))
        {
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }
    }

    //����� �ޱ�
    public IEnumerator DownloadAudio(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(info.url, AudioType.WAV))
        {
            yield return webRequest.SendWebRequest();

            DownloadHandlerAudioClip handler = webRequest.downloadHandler as DownloadHandlerAudioClip;
            //handler.audioClip; �� Audiosource�� �����ϰ� �÷���

            DoneRequest(webRequest, info);
        }
    }

    // ��û �Ϸ� �� ȣ��Ǵ� �޼ҵ�
    void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
    {
        //���࿡ ����� �����̶��
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //����� �����͸� ��û�� Ŭ������ ������.
            if (info.onComplete != null)
            {
                info.onComplete(webRequest.downloadHandler);
            }
        }
        //�׷��� �ʴٸ� (Error ���)
        else
        {
            //Error�� ���� ���� ���
            Debug.LogError("Net Error = " + webRequest.error);
        }
    }

    // ���� �����͸� UserInfo ����ü�� ��ȯ�ϴ� ����
    void ParseUserInfo(DownloadHandler downloadHandler)
    {
        string json = downloadHandler.text;
        UserLoginInfo userInfo = JsonUtility.FromJson<UserLoginInfo>(json);
        Debug.Log("User Name: " + userInfo.userName);
        Debug.Log("User Token: " + userInfo.userId);

        AccountDate.instance.InAccount(userInfo.userId, userInfo.userName);
        // �ʿ��� �ٸ� �ʵ�鵵 ��� ����
    }

    public void Alert(string text)
    {
        StartCoroutine(PopUpTime(text));
    }

    IEnumerator PopUpTime(string text)
    {
        alertText.text = text;
        alertFullset.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        alertFullset.SetActive(false);
    }
}