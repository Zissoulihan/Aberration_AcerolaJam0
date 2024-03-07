using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHidingSpot : MonoBehaviour
{
    [SerializeField] Transform _hideSpot;

    public static List<NPCHidingSpot> HidingSpots;
    public bool Claimed { get; private set; }
    public Vector3 HidingPosition => _hideSpot.position;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (HidingSpots == null) HidingSpots = new List<NPCHidingSpot>();
        HidingSpots.Add(this);
    }

    public void CallDibs()
    {
        if (Claimed) return;

        Claimed = true;
        HidingSpots?.Remove(this);
    }
    public void Relinquish()
    {
        if (!Claimed) return;
        Claimed = false;
        Init();
    }
}
