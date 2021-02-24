using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// BattleSceneを管理
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 現在の戦闘状態
    /// </summary>
    [SerializeField] BattleState m_battleState = BattleState.BeginBattle;

    [SerializeField] Transform m_playerBattlePosition;
    [SerializeField] Transform m_enemyBattlePosition;
    //[SerializeField] GameObject m_statusPanel;
    //[SerializeField] GameObject m_statusIconPrefab;
    [SerializeField] CinemachineVirtualCamera m_beginBattleCamera;
    [SerializeField] float m_beginBattleTime = 2f;
    //[SerializeField] Animator m_slideEffectPanel;
    [SerializeField] GameObject m_commandWindow;
    [SerializeField] Transform m_commandArea;
    [SerializeField] GameObject m_commandButtonPrefab;
    [SerializeField] PlayableDirector m_winCutScene;
    [SerializeField] PlayableDirector m_loseCutScene;
    //PlayableDirector m_ResultCutScene;
    [SerializeField] float m_delayAtEndTurn = 1f;

    /// <summary>
    ///戦うプレイヤー
    /// </summary>
    [SerializeField] GameObject[] m_playerPrefabs;
    /// <summary>
    /// 戦うエネミー
    /// </summary>
    [SerializeField] GameObject[] m_enemyPrefabs;
    //プレイヤー、エネミーの戦闘ユニットリスト
    [SerializeField] List<BattlePlayerController> m_playerUnits = new List<BattlePlayerController>();
    [SerializeField] List<BattleEnemyController> m_enemyUnits = new List<BattleEnemyController>();
    /// <summary>
    /// すべての現在戦闘ユニット
    /// </summary>
    [SerializeField] List<BattleStatusControllerBase> m_allUnits = new List<BattleStatusControllerBase>();
    /// <summary>
    /// m_allUnitに対応する現在の行動ユニット番号
    /// </summary>
    int m_currentNum = 0;

    /// <summary>
    /// バトル中か
    /// </summary>
    bool m_inBattle = true;
    /// <summary>
    /// 勝利したか
    /// </summary>
    bool m_won = false;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        StartCoroutine(SceneController.m_Instance.FadeIn());

        //SceneControllerからユニット情報を取得
        SceneController sc = SceneController.m_Instance;
        if (sc.m_PlayerPrefabs != null)
        {
            m_playerPrefabs = sc.m_PlayerPrefabs;
        }
        if (sc.m_EnemyPrefabs != null)
        {
            m_enemyPrefabs = sc.m_EnemyPrefabs;
        }

        //ユニットをインスタンスしてListにAdd
        foreach (var unit in m_playerPrefabs)
        {
            GameObject player = Instantiate(unit, m_playerBattlePosition);

            ////statusIconをセット
            //GameObject statusIcon = Instantiate(m_statusIconPrefab, m_statusPanel.transform);
            //player.GetComponent<BattlePlayerController>().SetStatusIcon(statusIcon.GetComponent<StatusIconController>());

            m_playerUnits.Add(player.GetComponent<BattlePlayerController>());
            m_allUnits.Add(player.GetComponent<BattleStatusControllerBase>());
        }
        foreach (var unit in m_enemyPrefabs)
        {
            GameObject enemy = Instantiate(unit, m_enemyBattlePosition);
            m_enemyUnits.Add(enemy.GetComponent<BattleEnemyController>());
            m_allUnits.Add(enemy.GetComponent<BattleStatusControllerBase>());
        }

        //戦闘開始演出
        StartCoroutine(BeginBattle(m_beginBattleTime));
    }

    // Update is called once per frame
    void Update()
    {
        //State管理
        switch (m_battleState)
        {
            case BattleState.BeginBattle:
                //開始演出
                break;

            case BattleState.StartTurn:
                //現在ユニット行動開始
                //StartCoroutine(TurnStart(m_allUnits[m_currentNum]));
                m_allUnits[m_currentNum].GetComponent<BattleStatusControllerBase>().StartAction();
                break;

            case BattleState.InAction:
                //行動中
                break;

            case BattleState.EndTurn:
                //戦闘終了したときResultTimelineを再生しAfterBattleへ
                if (!m_inBattle)
                {
                    //＜playerステータス引き継ぎ
                    (m_won ? m_winCutScene : m_loseCutScene).Play();

                    //クエストのタスクチェック
                    if (SceneController.m_Instance.m_CurrentQuest)
                    {
                        foreach (var enemyObj in m_enemyPrefabs)
                        {
                            if (SceneController.m_Instance.m_CurrentQuest.CheckTarget(enemyObj))
                            {
                                SceneController.m_Instance.m_CurrentQuest.AddQuestCount();
                            }
                        }
                    }
                    
                    m_battleState = BattleState.AfterBattle;
                    return;
                }

                //次のターンへ
                m_currentNum++;
                if (m_currentNum >= m_allUnits.Count)
                {
                    m_currentNum = 0;
                }
                m_battleState = BattleState.StartTurn;
                break;

            case BattleState.AfterBattle:
                //クリックでMapシーンへ
                if (Input.anyKeyDown)
                {
                    SceneController.m_Instance.CallLoadMapScene(!m_won);
                    //m_resultCutScene.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
    }

    //IEnumerator TurnStart(GameObject actionUnits)
    //{
    //    yield return null;//StartCoroutine(SlideEffect());
    //    actionUnits.GetComponent<BattleStatusControllerBase>().StartAction();
    //}

    //IEnumerator SlideEffectAndSetState(BattleState nextState)
    //{
    //    //m_slideEffectPanel.SetActive(true);
    //    //Animator anim = m_slideEffectPanel.GetComponent<Animator>();
    //    //while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //    //{
    //    //    yield return new WaitForEndOfFrame();
    //    //}
    //    //m_slideEffectPanel.SetActive(false);
    //    yield return null;
    //    m_battleState = nextState;
    //}

    //public void UpdateState(BattleState state)
    //{
    //    m_battleState = state;
    //}

    /// <summary>
    /// 戦闘開始演出
    /// </summary>
    /// <param name="aimTime"></param>
    /// <returns></returns>
    IEnumerator BeginBattle(float aimTime)
    {
        yield return StartCoroutine(SceneController.m_Instance.SlideEffect());
        yield return new WaitForSeconds(aimTime);
        m_beginBattleCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(Camera.main.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime);
        m_battleState = BattleState.StartTurn;
    }

    /// <summary>
    /// 戦闘状態を行動中にする
    /// </summary>
    public void StartActingTurn()
    {
        m_battleState = BattleState.InAction;
    }

    /// <summary>
    /// 戦闘状態をターン終了にする
    /// </summary>
    public void EndActingTurn()
    {
        StartCoroutine(DelayAndUpdateState(m_delayAtEndTurn, BattleState.EndTurn));
        //m_battleState = BattleState.EndTurn;
    }

    /// <summary>
    /// Delayし、戦闘状態を更新する
    /// </summary>
    /// <param name="delayTime"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    IEnumerator DelayAndUpdateState(float delayTime, BattleState state)
    {
        yield return new WaitForSeconds(delayTime);
        m_battleState = state;
    }

    /// <summary>
    /// コマンドセレクトを開始する
    /// </summary>
    /// <param name="actor"></param>
    public void StartCommandSelect(BattlePlayerController actor)//(Skill[] skills, BPC bpc)
    {
        m_commandWindow.SetActive(true);
        //foreach (var skill in skills)
        //{
            GameObject go = Instantiate(m_commandButtonPrefab, m_commandArea);
            CommandButtonController pcc = go.GetComponent<CommandButtonController>();
        //    pcc.m_skill = skill;
            pcc.m_actor = actor;
        //}
    }

    /// <summary>
    /// コマンドセレクトを終了する
    /// </summary>
    public void EndCommandSelect()
    {
        foreach (Transform child in m_commandArea)
        {
            Destroy(child.gameObject);
        }
        m_commandWindow.SetActive(false);
    }

    ///// <summary>
    ///// 死亡ユニットを現在戦闘ユニットリストから消し、どちらかの陣営が全滅したならバトル終了状態に
    ///// </summary>
    ///// <param name="deadUnit"></param>
    //public void DeleteUnitsList(BattleStatusControllerBase deadUnit)
    //{
    //    m_allUnits.Remove(deadUnit);
    //    if (deadUnit.gameObject.GetComponent<BattleEnemyController>())
    //    {
    //        m_enemyUnits.Remove(deadUnit.gameObject.GetComponent<BattleEnemyController>());
    //        if (m_enemyUnits.Count == 0)
    //        {
    //            m_inBattle = false;
    //            m_won = true;
    //        }
    //    }
    //    else if (deadUnit.gameObject.GetComponent<BattlePlayerController>())
    //    {
    //        m_playerUnits.Remove(deadUnit.gameObject.GetComponent<BattlePlayerController>());
    //        if (m_playerUnits.Count == 0)
    //        {
    //            m_inBattle = false;
    //            m_won = false;
    //        }
    //    }
    //}

    /// <summary>
    /// 死亡エネミーを現在戦闘ユニットリストから消し、全滅したなら勝利でバトル終了
    /// </summary>
    /// <param name="deadEnemy"></param>
    public void DeleteEnemyList(BattleEnemyController deadEnemy)
    {
        m_allUnits.Remove(deadEnemy);
        m_enemyUnits.Remove(deadEnemy);
        if (m_enemyUnits.Count == 0)
        {
            m_inBattle = false;
            m_won = true;
        }
    }
    /// <summary>
    /// 死亡プレイヤーを現在戦闘ユニットリストから消し、全滅したなら敗北でバトル終了
    /// </summary>
    /// <param name="deadPlayer"></param>
    public void DeletePlayerList(BattlePlayerController deadPlayer)
    {
        m_allUnits.Remove(deadPlayer);
        m_playerUnits.Remove(deadPlayer);
        if (m_playerUnits.Count == 0)
        {
            m_inBattle = false;
            m_won = false;
        }
    }

    /// <summary>
    /// 戦闘状態
    /// </summary>
    enum BattleState
    {
        BeginBattle,
        StartTurn,
        InAction,
        EndTurn,
        AfterBattle,
    }
}
