using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PowerTextController : MonoBehaviour {

    private GameObject m_powerText;
    private ResultTextController m_resultTextController;

    [SerializeField]
    private int m_maxPower;

    [SerializeField]
    private int m_nowPower;

    // Use this for initialization
    void Start () {
        m_powerText = GameObject.Find("PowerText");
        m_powerText.GetComponent<TextMeshProUGUI>().text = "Power " + m_nowPower + " / " + m_maxPower;
        m_resultTextController = FindObjectOfType<ResultTextController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(m_nowPower <= 0)
        {
            m_resultTextController.EndState = true;
            m_resultTextController.GameOverText();
        }
    }

    //現在の体力の値とスライダーを減らす関数
    public void NowPowerDown(int damage)
    {
        m_powerText = GameObject.Find("PowerText");
        m_nowPower -= damage;

        if (m_nowPower < 0)
        {
            m_nowPower = 0;
        }
        m_powerText.GetComponent<TextMeshProUGUI>().text = "Power " + m_nowPower + " / " + m_maxPower;
    }

    //現在の体力の値とスライダーを増やす関数
    public void NowPowerUp(int power)
    {
        m_powerText = GameObject.Find("PowerText");
        m_nowPower += power;

        if(m_nowPower > m_maxPower)
        {
            m_nowPower = m_maxPower;
        }
        m_powerText.GetComponent<TextMeshProUGUI>().text = "Power " + m_nowPower + " / " + m_maxPower;
    }
}
