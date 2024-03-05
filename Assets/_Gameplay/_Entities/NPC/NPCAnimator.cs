using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<NPCState, int> _stateFrameMap;
    [SerializeField] int _numTotalFrames;
    [SerializeField] float _xOffset = -0.15f;

    MeshRenderer _mr;
    Material _mat;

    int _currentFrame;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _mr = GetComponent<MeshRenderer>();
        _mat = _mr.material = new(_mr.material);
        _currentFrame = 0;
    }

    public void AnimState(NPCState newState)
    {
        int newFrame = GetStateFrame(newState);
        if (_currentFrame == newFrame) return;
        _mat.mainTextureOffset = GetMatOffset(newFrame);
        _currentFrame = newFrame;
    }

    int GetStateFrame(NPCState state)
    {
        if (!_stateFrameMap.ContainsKey(state)) {
            print($"NPC state {state} needs a frame number in the thing!");
            return 0;
        }
        return _stateFrameMap[state];
    }
    Vector2 GetMatOffset(int frameNum)
    {
        float offsetPer = 1f / _numTotalFrames;
        return new(_xOffset, offsetPer * frameNum);
    }

}
