using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    [SerializeField] BattleState m_battleState = BattleState.StartTurn;

    [SerializeField] Transform m_playerBattlePosition;
    [SerializeField] Transform m_enemyBattlePosition;
    //[SerializeField] GameObject m_statusPanel;
    //[SerializeField] GameObject m_statusIconPrefab;
    [SerializeField] GameObject m_commandWindow;
    [SerializeField] GameObject m_commandButtonPrefab;
    //[SerializeField] GameObject m_slideEffectPanel;
    [SerializeField] PlayableDirector m_winCutScene;
    [SerializeField] PlayableDirector m_loseCutScene;
    PlayableDirector m_ResultCutScene;

    /// <summary>
    ///戦うプレイヤー
    /// </summary>
    [SerializeField] GameObject[] m_playerPrefabs;
    /// <summary>
    /// 戦うエネミー
    /// </summary>
    [SerializeField] GameObject[] m_enemyPrefabs;

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
    bool inBattle = true;
    /// <summary>
    /// 勝利したか
    /// </summary>
    bool won = false;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        
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
            GameObject player = Instantiate(unit, m_playerBattlePosition.position, m_playerBattlePosition.rotation);

            ////statusIconをセット
            //GameObject statusIcon = Instantiate(m_statusIconPrefab, m_statusPanel.transform);
            //player.GetComponent<BattlePlayerController>().SetStatusIcon(statusIcon.GetComponent<StatusIconController>());

            m_playerUnits.Add(player.GetComponent<BattlePlayerController>());
            m_allUnits.Add(player.GetComponent<BattleStatusControllerBase>());
        }
        foreach (var unit in m_enemyPrefabs)
        {
            GameObject enemy = Instantiate(unit, m_enemyBattlePosition.position, m_enemyBattlePosition.rotation);
            m_enemyUnits.Add(enemy.GetComponent<BattleEnemyController>());
            m_allUnits.Add(enemy.GetComponent<BattleStatusControllerBase>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //State管理
        switch (m_battleState)
        {
            //case BattleState.StartDirecting:
            //    StartCoroutine(SlideEffectAndSetState(BattleState.StartTurn));
            //    break;

            case BattleState.StartTurn:
                //現在ユニット行動開始
                //StartCoroutine(TurnStart(m_allUnits[m_currentNum]));
                m_allUnits[m_currentNum].GetComponent<BattleStatusControllerBase>().StartAction();
                break;

            case BattleState.ActingTurn:
                //行動中
                
                break;

            case BattleState.EndTurn:
                //戦闘終了したときResultTimelineを再生しAfterBattleへ
                if (!inBattle)
                {
                    foreach (BattleStatusControllerBase unit in m_allUnits)
                    {
                       unit.gameObject.SetActive(false);
                    }
                    (won ? m_winCutScene : m_loseCutScene).Play();
                    //Instantiate(m_resultTimelinePrefab);//setactiv?
                    //StartCoroutine(DelayAndUpdateState(0.5f, BattleState.AfterBattle));
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
                    SceneController.m_Instance.CallLoadMapScene();
                    //m_resultCutScene.gameObject.SetActive(false);
                }
                ////クリックでMapシーンへ
                //if (Input.anyKeyDown)
                //{
                //    SceneController.m_Instance.CallLoadMapScene();
                //}
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
    /// 戦闘状態を行動中にする
    /// </summary>
    public void StartActingTurn()
    {
        m_battleState = BattleState.ActingTurn;
    }

    /// <summary>
    /// 戦闘状態をターン終了にする
    /// </summary>
    public void EndActingTurn()
    {
        StartCoroutine(DelayAndUpdateState(0.5f, BattleState.EndTurn));
        //m_battleState = BattleState.EndTurn;
    }
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
            GameObject go = Instantiate(m_commandButtonPrefab, m_commandWindow.transform);
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
        foreach (Transform child in m_commandWindow.transform)
        {
            Destroy(child.gameObject);
        }
        m_commandWindow.SetActive(false);
    }

    /// <summary>
    /// 死亡ユニットを現在戦闘ユニットリストから消す
    /// </summary>
    /// <param name="deadUnit"></param>
    public void DeleteUnitsList(BattleStatusControllerBase deadUnit)
    {
        m_allUnits.Remove(deadUnit);
        if (deadUnit.gameObject.GetComponent<BattleEnemyController>())
        {
            m_enemyUnits.Remove(deadUnit.gameObject.GetComponent<BattleEnemyController>());
            if (m_enemyUnits.Count == 0)
            {
                inBattle = false;
                won = true;
                m_ResultCutScene = m_winCutScene;
            }
        }
        else if (deadUnit.gameObject.GetComponent<BattlePlayerController>())
        {
            m_playerUnits.Remove(deadUnit.gameObject.GetComponent<BattlePlayerController>());
            if (m_playerUnits.Count == 0)
            {
                inBattle = false;
                won = false;
                //m_ResultCutScene = m_loseCutScene;
            }
        }
    }

    /// <summary>
    /// 戦闘状態
    /// </summary>
    enum BattleState
    {
        //StartDirecting,
        StartTurn,
        ActingTurn,
        EndTurn,
        AfterBattle,
    }
}
