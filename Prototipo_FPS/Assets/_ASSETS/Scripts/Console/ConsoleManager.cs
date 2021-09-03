using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager Instance = null;

    [SerializeField]
    private GameObject m_pfConsoleMessage = null;
    [SerializeField]
    private GameObject m_content = null;
    [SerializeField]
    private GameObject m_scroll = null;

    [Space]
    [SerializeField]
    private List<ConsoleMessage> m_cMessageList = new List<ConsoleMessage>();

    [Space]

    [SerializeField]
    private float m_maxTimeToHide = 4;
    private float m_crntTimeToHide = 0;
    private bool m_consoleActivated = false;

    private void Awake()
    {
        Instance = this;

        HideConsole();
    }

    private void Update()
    {
        UpdateHideConsoleTimer();

        //DEBUG
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddTextMessage();
        }
    }

    public void AddTextMessage()
    {
        ConsoleMessage cMessage;

        cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("This is a");
        cMessage.AddText("TEST", "red", true);
        cMessage.AddText("message", "red", true);
        cMessage.AddText("(id." + (Random.value * 10000).ToString("F0") + ")", "yellow", false, true);
    }


    private ConsoleMessage NewMessage()
    {
        ShowConsole();

        ConsoleMessage cMessage;
        cMessage = Instantiate(m_pfConsoleMessage, m_content.transform).GetComponentInChildren<ConsoleMessage>();

        m_cMessageList.Add(cMessage);

        return cMessage;
    }

    private void UpdateHideConsoleTimer()
    {
        if (m_consoleActivated)
        {
            m_crntTimeToHide -= Time.deltaTime;

            if(m_crntTimeToHide <= 0)
            {
                HideConsole();
            }
        }
    }

    private void ShowConsole()
    {
        m_scroll.gameObject.SetActive(true);
        m_crntTimeToHide = m_maxTimeToHide;
        m_consoleActivated = true;
    }

    private void HideConsole()
    {
        m_scroll.gameObject.SetActive(false);
        m_consoleActivated = false;
    }

}
