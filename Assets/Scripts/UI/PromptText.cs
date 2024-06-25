using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PromptText : NetworkBehaviour
{
    public NetworkVariable<FixedString512Bytes> text;


}
