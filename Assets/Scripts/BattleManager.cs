using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘全体を管理
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 現在の戦闘状態
    /// </summary>
    [SerializeField] State m_state = State.StartTurn;

    [SerializeField] GameObject m_CommandWindow;
    [SerializeField] Transform m_playerBattlePosition;
    [SerializeField] Transform m_enemyBattlePosition;

    /// <summary>
    ///戦うプレイヤー
    /// </summary>
    public GameObject[] m_playerPrefabs;
    /// <summary>
    /// 戦うエネミー
    /// </summary>
    public GameObject[] m_enemyPrefabs;

    [SerializeField] List<GameObject> m_playerUnits = new List<GameObject>();
    [SerializeField] List<GameObject> m_enemyUnits = new List<GameObject>();
    /// <summary>
    /// すべての現在戦闘ユニット
    /// </summary>
    [SerializeField] List<GameObject> m_allUnits = new List<GameObject>();
    /// <summary>
    /// m_allUnitに対応する現在の行動ユニット番号
    /// </summary>
    int m_nowNum = 0;

    /// <summary>
    /// バトル中か
    /// </summary>
    bool inBattle = true;
    /// <summary>
    /// 勝利したか
    /// </summary>
    bool won = false;


    // Start is called before the first frame update
    void Start()
    {
        ///SceneControllerからユニット情報を取得
        SceneController sc = SceneController.m_Instance;
        if (sc.m_playerPrefabs.Length > 0)
        {
            m_playerPrefabs = sc.m_playerPrefabs;
        }
        if (sc.m_enemyPrefabs.Length > 0)
        {
            m_enemyPrefabs = sc.m_enemyPrefabs;
        }

        //ユニットをインスタンスしてListにAdd
        foreach (var unit in m_playerPrefabs)
        {
            GameObject player = Instantiate(unit, m_playerBattlePosition.position, m_playerBattlePosition.rotation);
            m_playerUnits.Add(player);
            m_allUnits.Add(player);
        }
        foreach (var unit in m_enemyPrefabs)
        {
            GameObject enemy = Instantiate(unit, m_enemyBattlePosition.position, m_enemyBattlePosition.rotation);
            m_enemyUnits.Add(enemy);
            m_allUnits.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //State管理
        switch (m_state)
        {
            case State.StartTurn:
                //現在ユニット行動開始
                m_allUnits[m_nowNum].GetComponent<BattleStatusControllerBase>().StartAction();
                break;

            case State.ActingTurn:
                //行動中
                break;

            case State.EndTurn:
                //戦闘終了したとき
                if (!inBattle)
                {
                    Debug.Log(won ? "Win" : "Lose");
                    m_state = State.AfterBattle;
                    return;
                }

                //次のターンへ
                m_nowNum++;
                if (m_nowNum >= m_allUnits.Count)
                {
                    m_nowNum = 0;
                }
                m_state = State.StartTurn;
                break;

            case State.AfterBattle:
                //戦闘後Mapシーンへ
                if (Input.anyKeyDown)
                {
                    SceneController.m_Instance.LoadMapScene();
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 戦闘状態を行動中にする
    /// </summary>
    public void StartActingTurn()
    {
        m_state = State.ActingTurn;
    }

    /// <summary>
    /// 戦闘状態をターン終了にする
    /// </summary>
    public void EndActingTurn()
    {
        m_state = State.EndTurn;
    }

    /// <summary>
    /// コマンドセレクトを開始する
    /// </summary>
    /// <param name="actor"></param>
    public void StartCommandSelect(BattlePlayerController actor)
    {
        m_CommandWindow.SetActive(true);
        m_CommandWindow.GetComponent<PlayerCommandController>().m_actor = actor;
    }

    /// <summary>
    /// コマンドセレクトを終了する
    /// </summary>
    public void EndCommandSelect()
    {
        m_CommandWindow.GetComponent<PlayerCommandController>().m_actor = null;
        m_CommandWindow.SetActive(false);
    }

    /// <summary>
    /// 死亡ユニットを現在戦闘ユニットリストから消す
    /// </summary>
    /// <param name="deadUnit"></param>
    public void DeleteUnitsList(GameObject deadUnit)
    {
        m_allUnits.Remove(deadUnit);
        if (deadUnit.GetComponent<BattleEnemyController>())
        {
            m_enemyUnits.Remove(deadUnit);
            if (m_enemyUnits.Count == 0)
            {
                inBattle = false;
                won = true;
            }
        }
        else if (deadUnit.GetComponent<BattlePlayerController>())
        {
            m_playerUnits.Remove(deadUnit);
            if (m_playerUnits.Count == 0)
            {
                inBattle = false;
                won = false;
            }
        }
    }

    /// <summary>
    /// 戦闘状態
    /// </summary>
    public enum State
    {
        StartTurn,
        ActingTurn,
        EndTurn,
        AfterBattle,
    }
}
