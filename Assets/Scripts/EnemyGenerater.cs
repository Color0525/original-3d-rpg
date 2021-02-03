using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// //Enemyを生成する
/// </summary>
public class EnemyGenerater : MonoBehaviour
{
    /// <summary>
    /// 出現させるEnemyのPrefub
    /// </summary>
    [SerializeField] GameObject m_mapEnemyPrefab;
    /// <summary>
    /// 出現数
    /// </summary>
    [SerializeField] int m_instanceNum = 3;
    /// <summary>
    /// 生成する四角範囲の大きさ
    /// </summary>
    [SerializeField] float m_generateRange = 20f;
    /// <summary>
    /// プレイヤーとの最低距離
    /// </summary>
    [SerializeField] float m_playerDis = 10f;
    /// <summary>
    /// 生成済みかどうか
    /// </summary>
    [SerializeField] bool m_generated = false;

    void Update() 
    {
        if (FindObjectOfType<HumanoidController>() && !m_generated)
        {
            Vector3 playerPos = FindObjectOfType<HumanoidController>().transform.position;
            for (int i = 0; i < m_instanceNum; i++)
            {
                float randomX = Random.Range(this.transform.position.x - m_generateRange, this.transform.position.x + m_generateRange);
                float randomZ = Random.Range(this.transform.position.z - m_generateRange, this.transform.position.z + m_generateRange);
                Vector3 instancePos = new Vector3(randomX, this.transform.position.y, randomZ);
                if (Vector3.Distance(instancePos, playerPos) > m_playerDis)
                {
                    Instantiate(m_mapEnemyPrefab, instancePos, Quaternion.identity);
                }
            }

            m_generated = true;
        }
    }
    ///// <summary>
    ///// 敵を生成
    ///// </summary>
    //public void GenerateEnemy()
    //{
    //    for (int i = 0; i < m_instanceNum; i++)
    //    {        
    //        float randomX = Random.Range(this.transform.position.x - m_generateRange, this.transform.position.x + m_generateRange);
    //        float randomZ = Random.Range(this.transform.position.z - m_generateRange, this.transform.position.z + m_generateRange);
    //        Vector3 instancePos = new Vector3(randomX, this.transform.position.y, randomZ);
    //        Vector3 playerPos = FindObjectOfType<HumanoidController>().transform.position;
    //        if (Vector3.Distance(instancePos, playerPos) > m_playerDis)
    //        {
    //            Instantiate(m_mapEnemyPrefab, instancePos, Quaternion.identity);
    //        }
    //    }
    //}
}
