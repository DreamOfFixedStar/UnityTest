
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleLaneGame : MonoBehaviour
{
    public SingleLanePlayer me;
    public SingleLanePlayer you;
    private bool gameOver;
    private bool hotSeatSecondPlayerTurn;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        hotSeatSecondPlayerTurn = false;
        me.Initialize(false);
        if (GameManager.instance.gameMode == 0)
        {
            you.Initialize(true);
        }
        else
        {
            you.Initialize(false);
            MeSetButton(false, false);
            YouSetButton(false, false);
            GameObject canvas = GameObject.Find("Canvas");
            GameObject turn_object = canvas.transform.Find("Turn").gameObject;
            Text turn_text = turn_object.transform.Find("Text").GetComponent<Text>();
            turn_text.text = "1P Turn";
            turn_object.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �� ���� Ŭ��
    public void ClickEndTurn()
    {
        if (!gameOver)
        {
            // �̱� �÷���
            if (GameManager.instance.gameMode == 0)
            {
                if (me.CheckSelectedCard())
                {
                    MeSetButton(false, false);
                    SetEndTurn(false);
                    StartCoroutine("Battle");
                }
                else
                {
                    Debug.Log("Card not selected");
                }
            }
            // �ֽ�Ʈ �÷���
            else
            {
                // ù��° �÷��̾�
                if (!hotSeatSecondPlayerTurn)
                {
                    if (me.CheckSelectedCard())
                    {
                        MeSetButton(false, false);
                        SetEndTurn(false);
                        StartCoroutine(me.StopSelectedAnimation());
                        hotSeatSecondPlayerTurn = true;
                        GameObject canvas = GameObject.Find("Canvas");
                        GameObject turn_object = canvas.transform.Find("Turn").gameObject;
                        Text turn_text = turn_object.transform.Find("Text").GetComponent<Text>();
                        turn_text.text = "2P Turn";
                        turn_object.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Card not selected");
                    }
                }
                // �ι�° �÷��̾�
                else
                {
                    if (you.CheckSelectedCard())
                    {
                        YouSetButton(false, false);
                        SetEndTurn(false);
                        StartCoroutine(you.StopSelectedAnimation());
                        StartCoroutine("Battle");
                    }
                    else
                    {
                        Debug.Log("Card not selected");
                    }
                }
            }
        }
    }

    // �� ����
    public void ClickStartTurn()
    {
        if (!hotSeatSecondPlayerTurn)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject turn_object = canvas.transform.Find("Turn").gameObject;
            turn_object.SetActive(false);
            MeSetButton(true, true);
            SetEndTurn(true);
        }
        else
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject turn_object = canvas.transform.Find("Turn").gameObject;
            turn_object.SetActive(false);
            YouSetButton(true, true);
            SetEndTurn(true);
        }
    }

    // �α��� �޴��� �̵�
    public void LoginMenu()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    // ����
    private IEnumerator Battle()
    {
        // �̱� �÷��̽� AI �÷��̾� ī�� ����
        if (GameManager.instance.gameMode == 0)
        {
            you.AISelectCard();
        }
        yield return StartCoroutine(me.StopSelectedAnimation());
        me.Ready();
        you.Ready();
        yield return new WaitForSeconds(1f);

        int you_damage = me.GetDamage(you);
        int me_damage = you.GetDamage(me);
        StartCoroutine(me.Fight(me_damage));
        yield return StartCoroutine(you.Fight(you_damage));
        YouSetButton(false, false);
        MeSetButton(false, false);
        yield return new WaitForSeconds(0.5f);

        CheckGameOver();
        // �ֽ�Ʈ �÷��̽� ù ��° �÷��̾� ������
        if (GameManager.instance.gameMode == 1)
            hotSeatSecondPlayerTurn = false;
        yield return null;

        GameObject canvas = GameObject.Find("Canvas");
        GameObject turn_object = canvas.transform.Find("Turn").gameObject;
        Text turn_text = turn_object.transform.Find("Text").GetComponent<Text>();
        turn_text.text = "1P Turn";
        if (GameManager.instance.gameMode == 1)
            turn_object.SetActive(true);
        else
            SetEndTurn(true);
    }

    // �������� Ȯ��
    private bool CheckGameOver()
    {
        int me_life = me.GetLife();
        int you_life = you.GetLife();

        if (me_life <= 0 || you_life <= 0)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject gameover_object = canvas.transform.Find("GameOver").gameObject;
            Text gameover_text = gameover_object.transform.Find("Text").GetComponent<Text>();
            if (me_life <= 0 && you_life <= 0)
            {
                gameover_text.text = "Draw";
            }
            else if (me_life <= 0)
            {
                gameover_text.text = "Lose";
            }
            else
            {
                gameover_text.text = "Win";
            }
            gameover_object.SetActive(true);
            gameOver = true;

            return true;
        }

        return false;
    }
    // �� ���� ��ư Ȱ��
    private void SetEndTurn(bool click_Enabled)
    {
        GameObject.Find("EndTurn").GetComponent<Button>().interactable = click_Enabled;
    }

    // ��ư Ȱ�� ����
    private void MeSetButton(bool left_ClickEnabled, bool right_ClickEnabled)
    {
        List<int> remain_cards = me.GetRemainCards();
        GameObject me_object = GameObject.Find("Me");
        foreach (var card in remain_cards)
        {
            string card_name = "Card_" + card.ToString();
            GameObject card_object = me_object.transform.Find(card_name).gameObject;
            card_object.GetComponent<Button>().interactable = left_ClickEnabled;
            card_object.GetComponent<Card>().InfoTextSetActive(false, right_ClickEnabled);
        }
    }

    // ��ư Ȱ�� ����
    private void YouSetButton(bool left_ClickEnabled, bool right_ClickEnabled)
    {
        List<int> remain_cards = you.GetRemainCards();
        GameObject you_object = GameObject.Find("You");
        foreach (var card in remain_cards)
        {
            string card_name = "Card_" + card.ToString();
            GameObject card_object = you_object.transform.Find(card_name).gameObject;
            card_object.GetComponent<Button>().interactable = left_ClickEnabled;
            card_object.GetComponent<Card>().InfoTextSetActive(false, right_ClickEnabled);
        }
    }
}
