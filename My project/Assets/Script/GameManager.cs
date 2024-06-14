
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform canvas;
    public GameObject infoText;
    // 0 : VS CPU, 1 : Hot Seat, 2: Network
    public int gameMode;

    void Awake()
    {
        instance = this;
        gameMode = 0;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ���� ���� ����
    public void FailedToConnect()
    {
        StartCoroutine(GameManager.instance.PrintMessage("���� ���� ����"));
    }

    // ���� ������
    public void RoomIsFull()
    {
        StartCoroutine(GameManager.instance.PrintMessage("���� ���� á���ϴ�"));
    }

    // �� ���� ����
    public void JoinFailed()
    {
        StartCoroutine(GameManager.instance.PrintMessage("�濡 ������ �� �����ϴ�"));
    }

    // �޽��� ���
    private IEnumerator PrintMessage(string message)
    {
        GameObject temp = Instantiate(infoText, canvas);
        temp.GetComponent<Text>().text = message;
        temp.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(temp);
    }
}
