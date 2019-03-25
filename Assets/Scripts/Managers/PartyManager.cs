using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PartyManager : NetworkBehaviour {

    public enum StateFSM
    {
        WaitForPlayers,
        Battle,
        End,
        Restart
    }

    [SerializeField] private TMPro.TMP_Text timerText;

    [SerializeField] private int maxPlayer;
    [SerializeField] private int minPlayer;

    [SyncVar] private int playerAlive = 0;
    [SyncVar (hook = "OnTimeLeftChange")] private int timeLeft = 31;

    [SyncVar] public StateFSM state = StateFSM.WaitForPlayers;

    [SyncVar] private bool coroutineRunning = false;

    private GameObject[] _players;

    void Start()
    {
        StartCoroutine(FSMCorutine());
    }

    IEnumerator FSMCorutine()
    {
        while (true)
        {
            UpdateFSM();
            yield return new WaitForSeconds(2.0f);
        }
    }

    void UpdateFSM()
    {
        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        switch (state)
        {
            case StateFSM.WaitForPlayers:
                if (playerCount == maxPlayer)
                {
                    if (coroutineRunning)
                    {
                        StopCoroutine("StartWithMinPlayer");
                        Debug.Log("Stop corou");
                        CmdSetCoroutine(false);
                        timerText.gameObject.SetActive(false);
                    }
                    state = StateFSM.Battle;
                    playerAlive = playerCount;
                }
                if (playerCount == minPlayer && !coroutineRunning) {
                    StartCoroutine("StartWithMinPlayer");
                }
                break;
            case StateFSM.Battle:
                Debug.Log("Battle");
                _players = GameObject.FindGameObjectsWithTag("Player");
                GameObject.FindWithTag("Zone").GetComponent<Zone>().StartScaleZone();
                timerText.gameObject.SetActive(false);
                if (IsOnePlayerLeft())
                {
                    state = StateFSM.End;
                }
                break;
            case StateFSM.End:
                //End
                break;
            case StateFSM.Restart:
                break;
        }
    }

    [Command]
    void CmdSetCoroutine(bool act)
    {
        coroutineRunning = act;
    }

    IEnumerator StartWithMinPlayer()
    {
        if (!isServer)
            yield break;
        CmdSetCoroutine(true);
        timerText.gameObject.SetActive(true);
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        CmdSetCoroutine(false);
        state = StateFSM.Battle;
        Debug.Log("End corou");
        playerAlive = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    void OnTimeLeftChange(int time)
    {
        timerText.gameObject.SetActive(true);
        timerText.text = time + (time <= 1 ? " seconde" : " secondes");
    }

    bool IsOnePlayerLeft()
    {
        int nb = 0;

        foreach (var obj in _players)
            if (obj.activeSelf)
                nb++;
        return nb <= 1;
    }

    GameObject GetOnePlayerLeft()
    {
        foreach (var obj in _players)
            if (obj.activeSelf)
                return obj;
        return null;
    }
}
