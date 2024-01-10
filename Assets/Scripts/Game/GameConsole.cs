using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GameConsole : MonoBehaviour
{
    [Serializable]
    public class Command
    {
        public string keyword;
        public UnityEvent onCommandExecuted;
    }

    public static GameConsole instance;

    public Transform consoleOutput;
    public TMP_InputField consoleInput;
    public GameObject consoleOutputPrefab;
    public GameObject consolePanel;
    public bool consoleActive = false;

    public List<Command> commands = new List<Command>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of GameConsole. Destroying the new one.");
            Destroy(gameObject);
        }
    }

    public void Output(string content)
    {
        string timeString = DateTime.Now.ToString("HH:mm");
        string formattedContent = $"{timeString} - {content}";

        GameObject textOutput = Instantiate(consoleOutputPrefab, consoleOutput);
        TMP_Text textObj = textOutput.GetComponent<TMP_Text>();
        textObj.text = formattedContent;
    }

    private void Mode()
    {
        GameObject activePlayer = FindActivePlayer();

        if (activePlayer != null)
        {
            consoleActive = !consoleActive;
            consolePanel.SetActive(consoleActive);
            if(consoleActive == true)
            {
                activePlayer.GetComponent<PlayerController>().playerState = PlayerState.CanNotInput;
                activePlayer.GetComponentInChildren<PlayerCamera>().forceLock = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }else{
                activePlayer.GetComponent<PlayerController>().playerState = PlayerState.CanInput;
                activePlayer.GetComponentInChildren<PlayerCamera>().forceLock = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }else
        {
            consoleActive = !consoleActive;
            consolePanel.SetActive(consoleActive);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete)) { Mode(); }
        if (Input.GetKeyDown(KeyCode.Return)) { Execute(); }
    }

    public void Execute()
    {
        if (consoleActive == false) return;
        //if (GameManager.instance.connected == true && GameManager.instance.isHost == false){return;}
        if (consoleInput.text != null)
        {
            string inputText = consoleInput.text.ToLower();

            foreach (var command in commands)
            {
                if (inputText.StartsWith(command.keyword.ToLower()))
                {
                    command.onCommandExecuted.Invoke();
                    consoleInput.text = "";
                    return;
                }
            }
            consoleInput.text = "";
            Output("Command not recognized: " + inputText);
        }
    }
    public GameObject FindActivePlayer()
    {
        GameObject activePlayer = GameObject.FindWithTag("ActivePlayer");
        return activePlayer;
    }

    public void ForceQuit()
    {
        Application.Quit();
        Output("Application have stop running...");
    }

}
