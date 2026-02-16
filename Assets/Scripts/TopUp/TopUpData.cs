using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TopUpData", menuName = "TopUp/TopUpData")]
public class TopUpData : ScriptableObject
{
    public List<int> topUps;
}
