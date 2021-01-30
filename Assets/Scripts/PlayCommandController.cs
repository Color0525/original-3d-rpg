using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCommandController : MonoBehaviour
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
