using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyNumTextController : MonoBehaviour {

    private GameObject m_enemyNumText;

    private EnemyGenerator m_enemyGenerator;
    private LaserPointer m_laserPointer;

    //敵の最大数と倒した数の変数
    private int m_maxEnemy;
    private int m_enemyNum;

    // Use this for initialization
    void Start () {
        //MaxEnemyの値の取得
        m_enemyGenerator = FindObjectOfType<EnemyGenerator>();
        m_maxEnemy = m_enemyGenerator.MaxEnemy;

        m_laserPointer = FindObjectOfType<LaserPointer>();
        m_enemyNum = m_laserPointer.EnemyNum;

        m_enemyNumText = GameObject.Find("EnemyNumText");
        m_enemyNumText.GetComponent<TextMeshProUGUI>().text = "Enemy " + m_maxEnemy + " / " + m_maxEnemy;
    }
	
	// Update is called once per frame
	void Update () {
        m_enemyNum = m_laserPointer.EnemyNum;
        m_enemyNumText.GetComponent<TextMeshProUGUI>().text = "Enemy " + (m_maxEnemy - m_enemyNum) + " / " + m_maxEnemy;
    }
}
