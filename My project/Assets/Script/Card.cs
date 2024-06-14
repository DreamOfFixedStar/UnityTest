
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardType;
    public GameObject infoText;
    public bool rightClickEnabled;
    private GameObject instanceInfoText;

    void Awake()
    {
        rightClickEnabled = true;
        instanceInfoText = Instantiate(infoText, transform);
        Vector2 position = new Vector2(0, 100);
        instanceInfoText.transform.localPosition = position;
        instanceInfoText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && rightClickEnabled)
        {
            instanceInfoText.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData event_Data)
    {
        if (event_Data.button == PointerEventData.InputButton.Right && rightClickEnabled)
        {
            instanceInfoText.SetActive(true);
            transform.SetAsLastSibling();
        }
    }

    // ���� �ؽ�Ʈ Ȱ�� ����
    public void InfoTextSetActive(bool active, bool right_ClickEnabled)
    {
        instanceInfoText.SetActive(active);
        rightClickEnabled = right_ClickEnabled;
    }


    // ī�� ���� ����
    public void SetInfo()
    {
        instanceInfoText.GetComponent<Text>().text = GetInfo();
    }

    // ī�� ���� ��������
    public string GetInfo()
    {
        switch (cardType)
        {
            case 0:
                return "����� ī����ڰ� 1�� �ƴҰ��,\n����� ī����ڸ�ŭ �ݰ� �� ����մϴ�";
            case 1:
                return "��뿡�� �ݵ�� 1�� ������� �ݴϴ�.";
            case 2:
                return "��뺸�� ū ������ ��� 2�� ������� �ݴϴ�.";
            case 3:
                return "��뺸�� ū ������ ��� 3�� ������� �ݴϴ�.";
        }

        return "";
    }

    // ī�� ���� �ִϸ��̼�
    public void AnimationSet(bool selected)
    {
        GetComponent<Animator>().SetBool("Selected", selected);
    }
}
