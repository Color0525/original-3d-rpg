using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クエストデータ
/// </summary>
[CreateAssetMenu(fileName = "NewQuestData", menuName = "CreateQuestData")]
public class QuestData : ScriptableObject
{
    //[SerializeField] string m_QuestTaskText;
    [SerializeField] GameObject m_target = null;
    [SerializeField] int m_taskValue = 1;
    int m_taskCount = 0;
    public bool m_ClearFlag { get; private set; } = false;
    public bool m_Clear { get; private set; } = false;

    /// <summary>
    /// リセットする
    /// </summary>
    public void Reset()
    {
        m_taskCount = 0;
        m_ClearFlag = false;
        m_Clear = false;
    }

    /// <summary>
    /// クエストのタスクテキストを返す
    /// </summary>
    /// <param name="questText"></param>
    /// <param name="questCount"></param>
    /// <param name="questTask"></param>
    /// <returns></returns>
    public string QuestTaskText()
    {
        return $"{m_target.GetComponent<BattleStatusControllerBase>().m_Name}を{m_taskValue}体倒す {m_taskCount}/{m_taskValue}";
    }

    /// <summary>
    /// ターゲットが合っているかチェック
    /// </summary>
    /// <param name="enemyObject"></param>
    /// <returns></returns>
    public bool CheckTarget(GameObject enemyObject)
    {
        return enemyObject == m_target ? true : false; 
    }

    /// <summary>
    /// クエストカウントを+1
    /// </summary>
    public void AddQuestCount()
    {
        m_taskCount = Mathf.Min(m_taskValue, m_taskCount + 1);
        m_ClearFlag = m_taskCount >= m_taskValue ? true : false;
    }

    public void QuestClear()
    {
        m_Clear = true;
    }
}
