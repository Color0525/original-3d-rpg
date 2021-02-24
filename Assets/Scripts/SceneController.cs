using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// シーンを管理
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// シーン唯一のSceneController
    /// </summary>
    public static SceneController m_Instance { get; private set; }

    //BattleScene移行用戦闘キャラ情報
    public GameObject[] m_PlayerPrefabs { get; private set; }
    public GameObject[] m_EnemyPrefabs { get; private set; }

    //MapScene移行用Player情報
    public Vector3 m_PlayerMapPosition { get; private set; } = Vector3.zero;
    public Quaternion m_PlayerMapRotation { get; private set; } = Quaternion.identity;

    //Gameフラグ
    public bool m_NewGame { get; private set; } = false;
    public bool m_GameOver { get; private set; } = false;

    //現在のクエスト
    public QuestData m_CurrentQuest { get; private set; }

    [SerializeField] string m_mapSceneName = "Map";
    [SerializeField] string m_battleSceneName = "Battle";
    [SerializeField] GameObject m_fadePanelPrefab;
    [SerializeField] float m_fadeTime = 1f;
    [SerializeField] GameObject m_encountEffectPrefab;
    [SerializeField] GameObject m_slideEffectPrefab;
    [SerializeField] QuestData m_initialQuest;

    void Awake()
    {
        //シーンをまたぐSceneControllerを唯一にする
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);

            //初期クエストをセット
            if (m_initialQuest)
            {
                SetQuest(m_initialQuest);
            }
        }
    }

    /// <summary>
    /// クエストをセット
    /// </summary>
    /// <param name="quest"></param>
    void SetQuest(QuestData quest)
    {
        quest.Reset();
        m_CurrentQuest = quest;
    }

    /// <summary>
    /// NewGameを始める
    /// </summary>
    public void CallNewGameLoadMapScene()
    {
        m_NewGame = true;
        StartCoroutine(LoadMapScene());
    }
    /// <summary>
    /// NewGameフラグをfalseに
    /// </summary>
    public void SetNewGameFalse()
    {
        m_NewGame = false;
    }

    /// <summary>
    /// ゲームオーバーか判定し、マップシーン読み込みを呼ぶ
    /// </summary>
    public void CallLoadMapScene(bool gameOverFlag = false)
    {
        m_GameOver = gameOverFlag;
        if (m_GameOver)
        {
            m_PlayerMapPosition = Vector3.zero;
            m_PlayerMapRotation = Quaternion.identity;
        }
        StartCoroutine(LoadMapScene());
    }

    /// <summary>
    /// フェードアウトし、MapSceneを読み込む
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMapScene()
    {
        yield return StartCoroutine(FadeOut(m_fadeTime));
        StartCoroutine(LoadSceneCoroutine(m_mapSceneName));
        //StartCoroutine(FadeIn(m_fadeSpeed));
    }
    //フェードアウト
    IEnumerator FadeOut(float fadeTime = 1f)
    {
        GameObject go = Instantiate(m_fadePanelPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Image fadeImage = go.GetComponent<Image>();
        fadeImage.color = Color.clear;
        Color currentColor = fadeImage.color;
        while (fadeImage.color.a < 1f)
        {
            currentColor.a += 1f / fadeTime * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }
        //Destroy(go);
    }
    //フェードイン
    public IEnumerator FadeIn(float fadeTime = 1f)
    {
        GameObject go = Instantiate(m_fadePanelPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Image fadeImage = go.GetComponent<Image>();
        fadeImage.color = Color.black;
        Color currentColor = fadeImage.color;
        while (fadeImage.color.a > 0f)
        {
            currentColor.a -= 1f / fadeTime * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }
        Destroy(go);
    }
    


    /// <summary>
    /// エンカウントで呼び出す。情報を引き継ぎ、バトルシーン読み込みを呼ぶ
    /// </summary>
    /// <param name="playerPrefabs"></param>
    /// <param name="enemyPrefabs"></param>
    /// <param name="playerMapPos"></param>
    /// <param name="playerMapRotate"></param>
    public void EncountLoadBattleScene(GameObject[] playerPrefabs, GameObject[] enemyPrefabs, Transform playerMapTransform)
    {
        //情報の引き継ぎ
        m_PlayerPrefabs = playerPrefabs;
        m_EnemyPrefabs = enemyPrefabs;
        m_PlayerMapPosition = playerMapTransform.position;
        m_PlayerMapRotation = playerMapTransform.rotation;

        StartCoroutine(LoadBattleScene());
    }
    /// <summary>
    /// エンカウントエフェクトを再生し、BattleSceneを読み込む
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadBattleScene()
    {
        yield return StartCoroutine(EncountEffect());
        StartCoroutine(LoadSceneCoroutine(m_battleSceneName));
        //StartCoroutine(FadeIn(m_fadeSpeed));
        //StartCoroutine(SlideEffect());
    }
    //エンカウントエフェクト
    IEnumerator EncountEffect()
    {
        GameObject go = Instantiate(m_encountEffectPrefab);
        Animator anim = go.GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(go);
    }
    //スライドエフェクト
    public IEnumerator SlideEffect()
    {
        GameObject go = Instantiate(m_slideEffectPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Animator anim = go.GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(go);
    }

    //コルーチンとしてSceneを読み込む
    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
