using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方の戦闘コマンドを管理
/// </summary>
public class PlayerCommandController : MonoBehaviour
{
    /// <summary>
    /// 行動者
    /// </summary>
    public BattlePlayerController m_actor; 

    /// <summary>
    /// コマンド使用
    /// </summary>
    public void PlayCommand()
    {
        m_actor.AttackCommand();
    }

}
