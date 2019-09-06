using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * レーザーポインターを出すクラス
 */
public class LaserPointer : MonoBehaviour
{
    private EnemyGenerator m_enemyGenerator;
    private ResultTextController m_resultTextController;
    private PowerTextController m_powerTextController;
    private ButtonController m_buttonController;

    [SerializeField]
    private Transform _RightHandAnchor; // 右手

    [SerializeField]
    private Transform _LeftHandAnchor;  // 左手

    [SerializeField]
    private Transform _CenterEyeAnchor; // 目の中心

    [SerializeField]
    private float _MaxDistance; // 距離

    [SerializeField]
    private LineRenderer _LaserPointerRenderer; // LineRenderer

    [SerializeField]
    private GameObject m_cursor;

    [SerializeField]
    private GameObject m_shotEffect;

    [SerializeField]
    private GameObject m_item;

    [SerializeField]
    private int m_itemPower;

    [SerializeField]
    private GameObject m_itemEffect;

    //敵の最大数と倒した数の変数
    private int m_maxEnemy;
    private int m_enemyNum;

    [SerializeField]
    public SoundFXRef m_hitSound;

    [SerializeField]
    public SoundFXRef m_itemSound;

    // コントローラー
    private Transform Pointer
    {
        get
        {
            // 現在アクティブなコントローラーを取得
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ目の間からビームが出る
            return _CenterEyeAnchor;
        }
    }

    void Start()
    {
        m_enemyGenerator = FindObjectOfType<EnemyGenerator>();
        m_maxEnemy = m_enemyGenerator.MaxEnemy;

        m_resultTextController = FindObjectOfType<ResultTextController>();
        m_powerTextController = FindObjectOfType<PowerTextController>();
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
            var pointer = Pointer; // コントローラーを取得
                                   // コントローラーがない or LineRendererがなければ何もしない
            if (pointer == null || _LaserPointerRenderer == null)
            {
                return;
            }
            // コントローラー位置からRayを飛ばす
            Ray pointerRay = new Ray(pointer.position, pointer.forward);

            // レーザーの起点
            _LaserPointerRenderer.SetPosition(0, pointerRay.origin);

            RaycastHit hitInfo;
            Vector3 hitPosition;
            GameObject item;
            Vector3 TargetPosition = GameObject.Find("Player").transform.position;

            if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance, LayerMask.GetMask("Enemy")))
            {
                // Rayがヒットしたらそこまで
                _LaserPointerRenderer.SetPosition(1, hitInfo.point);

                // Cursolを表示する
                m_cursor.transform.position = hitInfo.point;
                m_cursor.SetActive(true);
                m_cursor.transform.LookAt(TargetPosition);

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && m_resultTextController.EndState == false && m_buttonController.StopState == false)
                {
                    hitPosition = hitInfo.point;
                    hitInfo.collider.transform.root.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().value -= 1;

                    //敵に攻撃した時のParticleを再生する。
                    Instantiate(m_shotEffect, hitPosition, Quaternion.identity);
                    m_hitSound.PlaySoundAt(hitPosition);

                    if (hitInfo.collider.transform.root.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().value == 0)
                    {
                        //敵のオブジェクトを消す
                        Destroy(hitInfo.collider.transform.root.gameObject);
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
            }
            else if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance, LayerMask.GetMask("Item")))
            {
                // Rayがヒットしたらそこまで
                _LaserPointerRenderer.SetPosition(1, hitInfo.point);

                m_cursor.transform.position = hitInfo.point;
                m_cursor.SetActive(true);
                m_cursor.transform.LookAt(TargetPosition);

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && m_resultTextController.EndState == false && m_buttonController.StopState == false)
                {
                    hitPosition = hitInfo.point;
                    Instantiate(m_itemEffect, hitPosition, Quaternion.identity);
                    m_itemSound.PlaySoundAt(hitPosition);
                    Destroy(hitInfo.collider.gameObject);
                    m_powerTextController.NowPowerUp(m_itemPower);
                }
            }
            else
            {
                // Rayがヒットしなかったら向いている方向にMaxDistance伸ばす
                _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _MaxDistance);

                // Cursolを非表示にする
                m_cursor.SetActive(false);
            }
        }
    }
    
    //EnemyNumの値をget,setするためのプロパティ
    public int EnemyNum
    {
        get { return m_enemyNum; }
        private set { m_enemyNum = value; }
    }
}