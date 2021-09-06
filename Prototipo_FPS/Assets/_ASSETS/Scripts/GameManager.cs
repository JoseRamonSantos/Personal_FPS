using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject BISurfaceG { get => m_bISurfaceG; set => m_bISurfaceG = value; }
    public GameObject BICharG { get => m_bICharG; set => m_bICharG = value; }
    public GameObject BICharRobot { get => m_bICharRobot; set => m_bICharRobot = value; }
    public LayerMask ShootLM { get => m_shootLM; }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
