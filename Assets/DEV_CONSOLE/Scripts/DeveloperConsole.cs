using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Console
{
    public enum LOG_TYPE
    {
        LOG,
        WARNING,
        ERROR,
        EXCEPTION,
        ASSERT
    }

    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole()
        {
            string addMessage = " command has been added to the console.";

            DeveloperConsole.AddCommandsToConsole(Command, this);
            Debug.Log(Name + addMessage);
        }

        public abstract void RunCommand();
    }

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance {  get; private set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas consoleCanvas;
        public ScrollRect scrollRect;
        public InputField consoleInput;
        public GameObject consoleLogs;
        public Text consoleText;
        public Text inputText;
        [Header("Prefab Log GO")]
        public GameObject logMsgPrefab;

        private void Awake()
        {
            if (Instance != null) return;

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
        }

        private void Start()
        {
            consoleCanvas.gameObject.SetActive(false);
            CreateCommands();
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string _message = "[" + type.ToString() + "] " + logMessage;
            if(type == LogType.Log)
            {
                AddMessageToConsole(LOG_TYPE.LOG, logMessage);
            } else if(type == LogType.Warning)
            {
                AddMessageToConsole(LOG_TYPE.WARNING, logMessage);
            } else if(type == LogType.Error)
            {
                AddMessageToConsole(LOG_TYPE.ERROR, logMessage);
            } else if(type == LogType.Exception)
            {
                AddMessageToConsole(LOG_TYPE.EXCEPTION, logMessage);
            } else if(type == LogType.Assert)
            {
                AddMessageToConsole(LOG_TYPE.ASSERT, logMessage);
            }
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void CreateCommands()
        {
            CommandQuit.CreateCommand();
        }

        public static void AddCommandsToConsole(string _name, ConsoleCommand _command)
        {
            if(!Commands.ContainsKey(_name))
            {
                Commands.Add(_name, _command);
            }
        }

        private void Update()
        {
            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                ToggleDevConsole();
            }

            if(consoleCanvas.gameObject.activeInHierarchy)
            {
                if(Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    if(inputText.text != "")
                    {
                        AddMessageToConsole(LOG_TYPE.LOG, inputText.text);
                        ParseInput(inputText.text);
                        consoleInput.text = string.Empty;
                        consoleInput.Select();
                        consoleInput.ActivateInputField();
                    }
                }
            }
        }

        private void AddMessageToConsole(LOG_TYPE type, string msg)
        {
            GameObject go = Instantiate(logMsgPrefab);
            go.GetComponent<SingleLog>().CreateLog(type, msg);

            go.transform.SetParent(consoleLogs.transform, false);

            
            //consoleText.text += msg + "\n";
        }

        //public static void AddStaticMessageToConsole(string msg)
        //{
        //    DeveloperConsole.Instance.consoleText.text = msg + "\n";
        //}

        private void ParseInput(string input) 
        {
            string[] _input = input.Split(null);

            if (_input.Length == 0 || _input == null)
            {
                //Debug.LogWarning("Command not recognized.");
                AddMessageToConsole(LOG_TYPE.WARNING, "Command not recognized.");
                return;
            }

            if (!Commands.ContainsKey(_input[0]))
            {
                //Debug.LogWarning("Command not recognized.");
                AddMessageToConsole(LOG_TYPE.WARNING, "Command not recognized.");
            } else
            {
                Commands[_input[0]].RunCommand();
            }
        }

        private void ToggleDevConsole()
        {
            consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);
            if(consoleCanvas.gameObject.activeInHierarchy)
            {
                consoleInput.ActivateInputField();
                consoleInput.Select();
            }
            else
            {

            }
        }
    }
}
