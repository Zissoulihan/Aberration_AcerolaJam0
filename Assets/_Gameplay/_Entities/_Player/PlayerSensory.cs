using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensory : MonoBehaviour
{
    [SerializeField] Transform _sensorGround;
    [SerializeField] LayerMask _layersGround;
    [SerializeField] float _distanceCheckGround;
    [SerializeField] int _hitBufferSize;

    public bool IsGrounded()
    {
        RaycastHit[] results = new RaycastHit[_hitBufferSize];
        return Physics.RaycastNonAlloc(new(_sensorGround.position, Vector3.down), results, _distanceCheckGround, _layersGround) > 0;
    }
}
