using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Move Input")]
    [SerializeField] GameEventVector2 _evInputMove;
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _evInputMove.TriggerEvent(context.ReadValue<Vector2>().normalized);
    }

    [Header("Interaction Input")]
    [SerializeField] GameEventVoid _evInteractPress;
    [SerializeField] GameEventVoid _evInteractRelease;
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started) _evInteractPress.TriggerEvent();
        if (context.canceled) _evInteractRelease.TriggerEvent();
    }

    [Header("Mouse Input")]
    [SerializeField] SharedVariableVector2 _svInputMousePos;
    public void OnUpdateMousePos(InputAction.CallbackContext context)
    {
        _svInputMousePos.Set(context.ReadValue<Vector2>());
    }

    [SerializeField] SharedVariableVector2 _svInputMouseDelta;
    public void OnUpdateMouseDelta(InputAction.CallbackContext context)
    {
        _svInputMouseDelta.Set(context.ReadValue<Vector2>());
    }
}
