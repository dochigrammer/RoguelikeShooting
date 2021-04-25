using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static bool IsNearlyEqual( this Vector3 _lhs, Vector3 _rhs, float _error_tolerance = float.Epsilon )
    {
        return Vector3.Distance(_lhs , _rhs) < _error_tolerance;
    }
}
