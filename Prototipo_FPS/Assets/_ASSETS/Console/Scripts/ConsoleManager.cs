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
    private List<ConsoleMessage> m_cMessagesList = new List<ConsoleMessage>();

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
            AddTestMessage();
        }
    }

    public void AddTestMessage()
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("This is a");
        cMessage.AddText("TEST", "red", true);
        cMessage.AddText("message", "red", true);
        cMessage.AddText("(id." + (Random.value * 10000).ToString("F0") + ")", "yellow", false, true);
    }

    public void AddPlayerCauseDamageMessage(Char_Base _sender, Char_Base _receiver, int damage, E_HITBOX_PART _hPart, float _distance)
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText(_sender.transform.name.ToString(), "green", false, true);
        cMessage.AddText(": damage given to");
        cMessage.AddText(_receiver.transform.name.ToString(), "red".ToString(), false, true);
        cMessage.AddText(":");
        cMessage.AddText(damage.ToString(), "red", true);
        cMessage.AddText("(" + _hPart + " - " + _distance.ToString("F0") + "m)");
    }


    private ConsoleMessage NewMessage()
    {
        ShowConsole();

        ConsoleMessage cMessage;
        cMessage = Instantiate(m_pfConsoleMessage, m_content.transform).GetComponentInChildren<ConsoleMessage>();

        if (m_cMessagesList.Count > 0)
        {
            m_cMessagesList[m_cMessagesList.Count - 1].Predeactivate();
        }

        m_cMessagesList.Add(cMessage);

        return cMessage;
    }

    private void UpdateHideConsoleTimer()
    {
        if (m_consoleActivated)
        {
            m_crntTimeToHide -= Time.deltaTime;

            if (m_crntTimeToHide <= 0)
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

        for (int i = 0; i < m_cMessagesList.Count; i++)
        {
            m_cMessagesList[i].Deactivate();
        }
    }

}
