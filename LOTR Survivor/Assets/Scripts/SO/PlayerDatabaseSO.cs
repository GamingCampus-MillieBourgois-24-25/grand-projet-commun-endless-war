using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerDatabase", menuName = "Player Database")]
public class PlayerDatabaseSO : ScriptableObject
{
    [Header("Liste des personnages disponibles")]
    public List<PlayerStatsSO> allCharacters = new List<PlayerStatsSO>();
}
