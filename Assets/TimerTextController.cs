using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerTextController : MonoBehaviour
{
    private GameObject m_timerText;

    private ResultTextController m_resultTextController;

    [SerializeField]
    private float m_timeLimit;

    // Use this for initialization
    void Start()
    {
        m_timerText = GameObject.Find("TimerText");
        m_timerText.GetComponent<TextMeshProUGUI>().text = "Time " + m_timeLimit + "s";

        m_resultTextController = FindObjectOfType<ResultTextController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_resultTextController.EndState == false)
        {
            m_timeLimit -= Time.deltaTime;
            int seconds = (int)m_timeLimit;
            m_timerText.GetComponent<TextMeshProUGUI>().text = "Time " + seconds + "s";
        }

        if (m_timeLimit <= 0)
        {
            m_resultTextController.EndState = true;
            m_resultTextController.GameOverText();
        }
    }
}