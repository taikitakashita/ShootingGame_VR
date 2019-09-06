using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShotCursor : MonoBehaviour
{
    //　カーソルに使用するテクスチャ
    [SerializeField]
    private Texture2D cursor;

    private EnemyGenerator m_enemyGenerator;
    private ResultTextController m_resultTextController;
    private PowerTextController m_powerController;
    private ButtonController m_buttonController;

    //敵の最大数と倒した数の変数
    private int m_maxEnemy;
    private int m_enemyNum;

    [SerializeField]
    private GameObject m_item;

    [SerializeField]
    private int m_itemPower;

    [SerializeField]
    private GameObject m_shotEffect;

    [SerializeField]
    private GameObject m_itemEffect;

    private AudioSource m_audioSource;

    [SerializeField]
    private AudioClip m_attackSound;

    void Start()
    {
        //　カーソルを自前のカーソルに変更
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);

        //MaxEnemyの値の取得
        m_enemyGenerator = FindObjectOfType<EnemyGenerator>();
        m_maxEnemy = m_enemyGenerator.MaxEnemy;

        m_resultTextController = FindObjectOfType<ResultTextController>();

        m_powerController = FindObjectOfType<PowerTextController>();

        m_audioSource = GetComponent<AudioSource>();

        m_buttonController = FindObjectOfType<ButtonController>();
    }

    void Update()
    {
        // すべての敵を倒したらクリアを表示する
        if (m_enemyNum == m_maxEnemy)
        {
            m_resultTextController.EndState = true;
            m_resultTextController.ClearText();
        }
        else
        {
            if (m_resultTextController.EndState == false && m_buttonController.StopState == false)
            {
                //　マウスの左クリックで撃つ
                if (Input.GetButtonDown("Fire1"))
                {
                    ShotEnemy();
                }
            }

        }
    }

    //　敵を撃つ
    void ShotEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 hitPosition;
        GameObject item;

        if (Physics.Raycast(ray, out hit, 30f, LayerMask.GetMask("Enemy")))
        {
            hitPosition = hit.point;
            hit.collider.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().value -= 1;

            //敵に攻撃した時のParticleを再生する。
            Instantiate(m_shotEffect, hitPosition, Quaternion.identity);
            m_audioSource.PlayOneShot(m_attackSound);

            if (hit.collider.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().value == 0)
            {
                //敵のオブジェクトを消す
                Destroy(hit.collider.gameObject);
                m_enemyNum += 1;

                //回復アイテムを落とす
                int num = Random.Range(1, 100);
                if (num <= 40)
                {
                    item = Instantiate(m_item) as GameObject;
                    item.transform.position = new Vector3(hitPosition.x, hitPosition.y + 1, hitPosition.z);
                    float itemRotx = Random.Range(0, 360);
                    float itemRoty = Random.Range(0, 360);
                    float itemRotz = Random.Range(0, 360);
                    item.transform.rotation = Quaternion.Euler(itemRotx, itemRoty, itemRotz);
                }
            }
        }

        //回復アイテムを打った時のParticleを再生する。
        if (Physics.Raycast(ray, out hit, 30f, LayerMask.GetMask("Item")))
        {
            hitPosition = hit.point;
            Instantiate(m_itemEffect, hitPosition, Quaternion.identity);
            Destroy(hit.collider.gameObject);
            m_powerController.NowPowerUp(m_itemPower);
        }
    }

    //EnemyNumの値をget,setするためのプロパティ
    public int EnemyNum
    {
        get { return m_enemyNum; }
        private set { m_enemyNum = value; }
    }
}