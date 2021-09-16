using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class GameManager : TemporalSingleton<GameManager>
{
    [SerializeField]
    private GameObject m_bISurfaceG = null;

    [SerializeField]
    private GameObject m_bICharG = null;

    [SerializeField]
    private GameObject m_bICharRobot = null;

    [SerializeField]
    private LayerMask m_shootLM = 1;

    [Space]
    [SerializeField]
    private E_INFINITE_AMMO m_infiniteAmmo = E_INFINITE_AMMO.DEFAULT;
    
    [Space]
    [SerializeField]
    private Transform m_enemiesTParent = null;

    [Header("SPAWN ENEMIES")]
    [SerializeField]
    private bool m_spawnEnemies = true;
    [SerializeField]
    private Char_Enemy[] m_spawnEnemiesList;
    [SerializeField]
    private float m_maxEnemies = 10;
    [SerializeField]
    private float m_timeToSpawn = 2;
    [SerializeField]
    private float m_timeToSpawnDeviation = 0;
    [SerializeField]
    private Collider[] m_spawnAreaList = null;



    public GameObject BISurfaceG { get => m_bISurfaceG; set => m_bISurfaceG = value; }
    public GameObject BICharG { get => m_bICharG; set => m_bICharG = value; }
    public GameObject BICharRobot { get => m_bICharRobot; set => m_bICharRobot = value; }
    public LayerMask ShootLM { get => m_shootLM; }
    public E_INFINITE_AMMO InfiniteAmmo { get => m_infiniteAmmo; }



    private void Start()
    {
        if (!m_enemiesTParent)
        {
            m_enemiesTParent = transform.Find("/Enemies");
        }

        SpawnInitialEnemies();
    }

    private void Update()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region SPAWN ENEMIES
    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < m_maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (!CanSpawnEnemies())
        {
            Debug.LogError(transform.name + " can NOT SPAWN ENEMY");
            return;
        }

        //Get rnd enemy
        int rndE = Random.Range(0, m_spawnEnemiesList.Length);

        //Get rndPos
        int rndSA = Random.Range(0, m_spawnAreaList.Length);
        Vector3 rndPos = Utility.GetRandomPointInBound(m_spawnAreaList[rndSA].bounds);

        //Adjust to NavMeshArea
        NavMeshHit nVHit;

        if (NavMesh.SamplePosition(rndPos, out nVHit, 99, m_spawnEnemiesList[rndE].GetComponent<NavMeshAgent>().areaMask))
        {
            rndPos = nVHit.position;
        }

        Quaternion rot = Quaternion.LookRotation(Char_Player.Instance.transform.position - rndPos, Vector3.up);
        
        Instantiate(m_spawnEnemiesList[rndE], rndPos, rot, m_enemiesTParent);
    }

    public void RemoveEnemy()
    {
        float t = Random.Range(m_timeToSpawn - m_timeToSpawnDeviation, m_timeToSpawn + m_timeToSpawnDeviation);

        StartCoroutine(SpawnEnemyRoutine(t));
    }

    private IEnumerator SpawnEnemyRoutine(float _time)
    {
        yield return new WaitForSeconds(_time);
        SpawnEnemy();
    }

    private bool CanSpawnEnemies()
    {

        bool enemies = m_spawnEnemiesList.Length > 0 && m_spawnEnemiesList[0] != null;

        bool spawn= m_spawnEnemies && enemies && m_spawnAreaList.Length > 0;

        return spawn;
    }
    #endregion
}
