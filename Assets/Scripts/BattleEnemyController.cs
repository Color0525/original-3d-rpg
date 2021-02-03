using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵戦闘時の行動
/// </summary>
public class BattleEnemyController : BattleStatusControllerBase
{
    /// <summary>
    /// 行動(敵)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        Attack(FindObjectOfType<BattlePlayerController>());
        EndAction();
    }
}
