using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinController : MonoBehaviour
{
    private Animator m_anim;
    private ResultTextController m_resultTextController;
    private PowerTextController m_powerTextController;
    private ButtonController m_buttonController;
    private GameObject m_damageImage;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private int m_enemyDamage;

    [SerializeField]
    private int m_moveStopDistance;

    [SerializeField]
    public SoundFXRef m_damageSound;

    // Use this for initialization
    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_anim.SetInteger("battle", 1);

        m_resultTextController = FindObjectOfType<ResultTextController>();
        m_powerTextController = FindObjectOfType<PowerTextController>();
        m_buttonController = FindObjectOfType<ButtonController>();

        m_damageImage = GameObject.Find("DamageImage");
        m_damageImage.GetComponent<Image>().color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NowPosition = transform.position;
        Vector3 TargetPosition = GameObject.Find("Player").transform.position;

        float distance = Vector3.Distance(NowPosition, TargetPosition);
        float step = m_moveSpeed * Time.deltaTime;

        this.transform.LookAt(TargetPosition);

        if (m_buttonController.StopState == false)
        {
            if (distance > m_moveStopDistance)
            {
                transform.position = Vector3.MoveTowards(NowPosition, TargetPosition, step);
                m_anim.SetInteger("moving", 2);
            }
            else
            {
                m_anim.SetInteger("moving", 0);

                if (m_resultTextController.EndState == false)
                {
                    int num = Random.Range(1, 500);
                    if (num < 2)
                    {
                        m_anim.SetInteger("moving", 3);
                        m_damageImage.GetComponent<Image>().color = new Color(0.5f, 0f, 0f, 0.5f);
                        m_powerTextController.NowPowerDown(m_enemyDamage);
                        m_damageSound.PlaySoundAt(transform.position);
                    }
                    m_damageImage.GetComponent<Image>().color = Color.Lerp(m_damageImage.GetComponent<Image>().color, Color.clear, Time.deltaTime);
                }
            }
        }
    }
}
