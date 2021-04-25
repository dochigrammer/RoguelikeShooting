using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyState
{
    NearbySerach,           // 주변 탐색하는 상태
    ProperLocationMove,     // 적절한 위치로 이동하는 상태
    Idle,                   // 가만히 멈춰있는 상태
    Attack,                 // 공격하는 상태
}
