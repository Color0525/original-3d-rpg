using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// BattleSceneを管理
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 現在の戦闘状態
    /// </summary>
    [SerializeField] State m_state = State.StartTurn;

    [SerializeField] Transform m_playerBattlePosition;
    [SerializeField] Transform m_enemyBattlePosition;
    [SerializeField] GameObject m_statusPanel;
    [SerializeField] GameObject m_statusIconPrefab;
    [SerializeField] GameObject m_commandWindow;
    [SerializeField] GameObject m_commandButtonPrefab;
    [SerializeField] GameObject m_canvas;
    [SerializeField] GameObject m_damageTextPrefab;


    /// <summary>
    ///戦うプレイヤー
    /// </summary>
    [SerializeField] GameObject[] m_playerPrefabs;
    /// <summary>
    /// 戦うエネミー
    /// </summary>
    [SerializeField] GameObject[] m_enemyPrefabs;

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

            //statusIconをセット
            GameObject statusIcon = Instantiate(m_statusIconPrefab, m_statusPanel.transform);
            player.GetComponent<BattlePlayerController>().SetupStatusIcon(statusIcon.GetComponent<StatusIconController>());

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
                    Debug.Log(SceneController.m_Instance);
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

    

    private Vector3 offset = new Vector3(0, 1.6f, 0);

    
    /// <summary>
    /// DamageTextを出す
    /// </summary>
    /// <param name="thisCanvas"></param>
    /// <param name="damage"></param>
    public void DamageText(Vector3 thisCanvas, int damage)
    {
        GameObject go = Instantiate(m_damageTextPrefab, m_canvas.transform);
        go.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, thisCanvas);
        go.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        Destroy(go, 1f);
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
