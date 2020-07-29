using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    PlayerCharacter character;
    PlayerController controller;
    public Text goldCount;
    public Text time;
    public Text deadTimes;
    public Text percentage;
    public Text goldCountEndPanel;
    public Text timeEndPanel;
    public Text goldCountCompletePanel;
    public Text timeCompletePanel;
    public GameObject gameStartPanel;
    public GameObject gameEndPanel;
    public GameObject gameCompletePanel;
    InputField randomNum;
    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        controller = FindObjectOfType<PlayerController>();
        randomNum = GetComponentInChildren<InputField>();
    }

    public void OnClick_RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void OnClick_ContinueGameFromCheckpoint()
    {
        gameEndPanel.SetActive(false);
        goldCount.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
        character.recoverTransform();
        controller.Initialize();
    }

    public void OnClick_GetInputNumber()
    {
        gameStartPanel.SetActive(false);
        goldCount.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
        
        int num = int.Parse(randomNum.text);
        Random.InitState(num);
        GameObject Env = GameObject.Find("Env");
        GameObject Level = new GameObject ("Level");
        Level.transform.parent = Env.transform;
        Level.AddComponent<GenerateScene>();
        Destroy(Level.GetComponent("GenerateScene"));
        character.gameObject.SetActive(true);
        character.isAlive = true;
        character.particle[4].Stop();
    }

    public void GameStart()
    {
        gameStartPanel.SetActive(true);
        goldCount.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
    }

    public void GameEnd()
    {
        gameEndPanel.SetActive(true);
        goldCount.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
    }

    public void GameComplete()
    {
        gameCompletePanel.SetActive(true);
        goldCount.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!character) return;

        int val = (int)(character.percentage*100);
        goldCount.text = "Coin Count: "+character.coinCount.ToString();
        time.text = "Time: "+character.gameTime.ToString("#0.00")+"s";

        deadTimes.text = "Attempt# "+character.deadTimes.ToString();
        percentage.text = "Complete "+val.ToString()+"%";
        //goldCountEndPanel.text = "Coin Count: "+character.coinCountSaved.ToString();
        //timeEndPanel.text = "Time: "+character.gameTimeSaved.ToString("#0.00")+"s";
        goldCountEndPanel.text = goldCount.text;
        timeEndPanel.text = time.text;

        goldCountCompletePanel.text = goldCount.text;
        timeCompletePanel.text = time.text;
    }
}