using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultTextController : MonoBehaviour {

    private GameObject m_clearText;
    private GameObject m_gameOverText;
    private bool m_endState;

    // Use this for initialization
    void Start () {
        m_clearText = GameObject.Find("ClearText");
        m_gameOverText = GameObject.Find("GameOverText");

        m_clearText.SetActive(false);
        m_gameOverText.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //クリアを表示する関数
    public void ClearText()
    {
        m_clearText.SetActive(true);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameScene");
        }
    }

    //ゲームオーバーを表示する関数
    public void GameOverText()
    {
        m_gameOverText.SetActive(true);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameScene");
        }
    }

    //EndStateの値をget,setするためのプロパティ
    public bool EndState
    {
        get { return m_endState; }
        set { m_endState = value; }
    }
}
