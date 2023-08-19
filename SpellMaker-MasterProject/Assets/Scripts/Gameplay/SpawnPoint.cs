using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int _teamOwnership;
    public int TeamOwnership => _teamOwnership;
}
