using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnPoints", menuName = "Spawn Points")]
public class SpawnPoints : ScriptableObject
{
    public List<Vector3> points;
}
