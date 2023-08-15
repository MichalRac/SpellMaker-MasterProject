using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointProvider : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    public List<SpawnPoint> SpawnPoints => _spawnPoints;

    public List<SpawnPoint> GetTeamSpawnPoints(int teamId) 
    { 
        return _spawnPoints.Where(sp => sp.TeamOwnership == teamId).ToList(); 
    }
}
