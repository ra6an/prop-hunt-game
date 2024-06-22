using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Console
{
    public class SingleLog : MonoBehaviour
    {
        public Color logColor;
        public Color warningColor;
        public Color errorColor;
        public Color exceptionColor;
        public Color assertColor;
        public Text logType;
        public Text logMessage;

        private void Awake()
        {
            logType.GetComponent<Text>();
            logMessage.GetComponent<Text>();
        }

        public void CreateLog(LOG_TYPE type, string msg)
        {
            logType.text = "[" + type + "]: ";
            if(type == LOG_TYPE.LOG)
            {
                logType.color = new Color(logColor.r, logColor.g, logColor.b);
            } else if(type == LOG_TYPE.WARNING)
            {
                logType.color = new Color(warningColor.r, warningColor.g, warningColor.b);
            } else if (type == LOG_TYPE.ERROR)
            {
                logType.color = new Color(errorColor.r, errorColor.g, errorColor.b);
            } else if (type == LOG_TYPE.EXCEPTION)
            {
                logType.color = new Color(exceptionColor.r, exceptionColor.g, exceptionColor.b);
            } else if(type == LOG_TYPE.ASSERT)
            {
                logType.color = new Color(assertColor.r, assertColor.g, assertColor.b);
            }
            logMessage.text = msg;
        }
    }
}
