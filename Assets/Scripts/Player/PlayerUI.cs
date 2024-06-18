using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    // Start is called before the first frame update

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if(canvas != null )
        {
            promptText = canvas.transform.Find("PromptText").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        
    }

    public void UpdateText(string promptMessage)
    {
        if (!promptText) return;
        promptText.text = promptMessage;
    }
}
