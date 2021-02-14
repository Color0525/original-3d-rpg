using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 戦闘ユニットのステータス管理（継承用）
/// </summary>
public class BattleStatusControllerBase : MonoBehaviour
{
    [SerializeField] string m_name = null;
    [SerializeField] int m_maxHP = 10;
    [SerializeField] int m_currentHP = 10;
    [SerializeField] int m_maxSP = 0;
    [SerializeField] int m_currentSP = 0;
    [SerializeField] int m_power = 3;
    [SerializeField] StatusIconController m_statusIcon;
    [SerializeField] Vector3 m_offset;
    [SerializeField] GameObject m_damageTextPrefab;
    //public bool m_InAction { get; private set; } = false;
    //public bool m_InAnimation { get; private set; } = false;
    //Skill[] m_skills;
    //Slill m_currentSkill;
    Animator m_anim;

    public static BattleManager m_battleManager;

    private void Awake()//StartBattle前のstate実装後Start()に戻す
    {
        m_battleManager = FindObjectOfType<BattleManager>();
    }

    void Start()
    {
        //m_BattleManager = FindObjectOfType<BattleManager>();
        m_anim = GetComponent<Animator>();
        m_statusIcon.SetupStatus(m_name, m_maxHP, m_currentHP, m_maxSP, m_currentSP);
    }

    /// <summary>
    /// StatusIconをセット
    /// </summary>
    /// <param name="statusIconController"></param>
    public void SetStatusIcon(StatusIconController statusIconController)
    {
        m_statusIcon = statusIconController;
    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void StartAction()
    {
        m_battleManager.StartActingTurn();
    }
    /// <summary>
    /// 行動終了
    /// </summary>
    void EndAction()
    {
        m_battleManager.EndActingTurn();
    }

    /// <summary>
    /// AnimatorのSetTriggerをセット（Animationにする？）
    /// </summary>
    /// <param name="animName"></param>
    public void SetTriggerAnimator(string animName)
    {
        m_anim.SetTrigger(animName);
    }
    public virtual void Hit()/// Attackアニメイベント 
    {
        Debug.Log(this.gameObject.name + " Attack");
    }
    void End()
    {
        EndAction();
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="target"></param>
    public void Attack(BattleStatusControllerBase target)
    {
        target.Damage(m_power);
    }
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="power"></param>
    void Damage(int power)
    {
        UpdateHP(-power);
        DamageText(this.transform.position + m_offset, power);
        if (m_currentHP == 0)
        {
            Debug.Log(this.gameObject.name + " Dead");
            m_anim.SetBool("Death", true);
            m_battleManager.DeleteUnitsList(this);
        }
        else
        {
            Debug.Log($"{this.gameObject.name} {power}Damage @{m_currentHP}");
            m_anim.SetTrigger("GetHit");
        }
    }

    /// <summary>
    /// DamageTextを出す
    /// </summary>
    /// <param name="unitCanvas"></param>
    /// <param name="damage"></param>
    void DamageText(Vector3 unitCanvas, int damage)
    {
        GameObject go = Instantiate(m_damageTextPrefab, GameObject.FindWithTag("MainCanvas").transform);
        go.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, unitCanvas);
        go.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        Destroy(go, 1f);
    }

    /// <summary>
    /// HPを更新
    /// </summary>
    /// <param name="value"></param>
    void UpdateHP(int value = 0)
    {
        m_currentHP = Mathf.Max(m_currentHP + value, 0);
        m_statusIcon.UpdateHPBar(m_maxHP, m_currentHP);
    }
}
