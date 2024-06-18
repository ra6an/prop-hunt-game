using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public struct LogObject
{
    public string time;
    public FixedString512Bytes message;
    public FixedString128Bytes from;
    public bool logToOwnerOnly;
    public bool logToEveryone;
    public bool logToHiders;
}

public class LogsController : MonoBehaviour
{
    private LogObject[] logs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
