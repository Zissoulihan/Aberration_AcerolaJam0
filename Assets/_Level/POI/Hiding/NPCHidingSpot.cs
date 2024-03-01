using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHidingSpot : MonoBehaviour
{
    [SerializeField] Transform _hideSpot;
    public bool Claimed { get; private set; }
    public Vector3 HidingPosition => _hideSpot.position;

    public void CallDibs()
    {
        if (Claimed) return;

        Claimed = true;
    }
}
