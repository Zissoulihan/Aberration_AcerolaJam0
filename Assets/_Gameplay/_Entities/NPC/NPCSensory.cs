using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSensory : MonoBehaviour
{
    [SerializeField] Transform _visionRoot;
    [SerializeField] float _lookDistance;
    [Range(0f, 360f)]
    [SerializeField] float _lookAngle;
    [SerializeField] LayerMask _layerPlayer;
    [SerializeField] LayerMask _layersVisionObscuring;
    [SerializeField] int _lookBufferSize;
    [SerializeField] float _durationCacheLookResult;

    public float LookDistance => _lookDistance;
    public float LookAngle => _lookAngle;

    float _timeLastLook;

    bool _cachedLookResult;

    public bool LookForPlayer()
    {
        if (Time.time < _timeLastLook + _durationCacheLookResult) {
            return _cachedLookResult;
        }

        _timeLastLook = Time.time;
        Collider[] playerColliders = new Collider[_lookBufferSize];
        int countResults = Physics.OverlapSphereNonAlloc(_visionRoot.position, _lookDistance, playerColliders, _layerPlayer);
        if (countResults <= 0) return _cachedLookResult = false;

        for (int i = 0; i < countResults; i++) {
            Transform playerTf = playerColliders[i].transform;
            Vector3 dirToPlayer = (playerTf.position - _visionRoot.position).normalized;
            
            //Player within view angle?
            if (Vector3.Angle(_visionRoot.forward, dirToPlayer) > _lookAngle / 2f) continue;

            //View obstructed?
            float distToPlayer = Vector3.Distance(_visionRoot.position, playerTf.position);
            RaycastHit[] hits = new RaycastHit[_lookBufferSize];
            if (Physics.RaycastNonAlloc(_visionRoot.position, dirToPlayer, hits, distToPlayer, _layersVisionObscuring) > 0) continue;

            //Confirmed, we have a visual
            return _cachedLookResult = true;
        }

        return _cachedLookResult = false;
    }

    //Credit: Sebastian Lague
    //I don't know trig lol yes it's a problem
    public Vector3 DirFromAngle(float angleDegrees)
    {
        angleDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
