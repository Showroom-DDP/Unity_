using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

                ParseUserInfo(webRequest.downloadHandler);
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
        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(json);
        Debug.Log("User ID: " + userInfo.userId);
        Debug.Log("User Name: " + userInfo.userName);
        // �ʿ��� �ٸ� �ʵ�鵵 ��� ����
    }
}