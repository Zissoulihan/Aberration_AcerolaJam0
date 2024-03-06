using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] PlayerAccelerationData _speedData;
    [SerializeField] SharedVariableVector3 _svPlayerPosition;
    [Header("Gravity & Jumping")]
    [SerializeField] PlayerAccelerationData _gravityData;
    [SerializeField] float _durationCoyoteTime;
    [SerializeField] float _airSpeedMult;
    [Header("Mouse Look")]
    [SerializeField] Vector2 _mouseSensitivity;     //TODO: Move to prefs & SV
    [SerializeField] Transform _camPivot;
    [SerializeField] float _clampXRotation = 90f;
    [Header("Input")]
    [SerializeField] GameEventVector2 _evInputMovement;
    [SerializeField] SharedVariableVector2 _svInputMouseDelta;
    [Header("GameState")]
    [SerializeField] SharedVariableBool _svGamePaused;

    PlayerSensory _sensory;

    Transform _tf;
    Rigidbody _rb;

    Vector3 _moveVelocity;
    Vector2 _moveInput;
    Vector2 _lastMoveInput;

    float _xLookRotation;
    float _yLookRotation;

    float _gravity;
    float _currentJumpForce;

    float _timeLastGrounded;
    float _timeLastIdle;

    bool _consideredGrounded;


    private void OnEnable()
    {
        _evInputMovement.Subscribe(OnMoveInput);
    }
    private void OnDisable()
    {
        _evInputMovement.Unsubscribe(OnMoveInput);
    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _tf = transform;
        _rb = GetComponent<Rigidbody>();

        _sensory = GetComponentInChildren<PlayerSensory>();

        //TODO: Relocate this
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnMoveInput(Vector2 moveInput)
    {
        _lastMoveInput = _moveInput;
        _moveInput = moveInput;
    }

    private void Update()
    {
        if (_svGamePaused.Value) return;
        CheckGrounded();
        HandleMoveInput();
        ApplyMouseLook();
    }

    void CheckGrounded()
    {
        if (_sensory.IsGrounded()) {
            _consideredGrounded = true;
            _timeLastGrounded = Time.time;
            return;
        }
        _consideredGrounded = Time.time - _timeLastGrounded <= _durationCoyoteTime;
    }

    void HandleMoveInput()
    {
        if (_moveInput == Vector2.zero || resetAccelTime()) {
            _timeLastIdle = Time.time;
        }

        Vector3 moveDir = (_tf.right * _moveInput.x) + (_tf.forward * _moveInput.y);
        float effMoveSpd = GetEffectiveMoveSpeed();

        _moveVelocity = moveDir.normalized * effMoveSpd;

        bool resetAccelTime()
        {
            return Vector2.Dot(_lastMoveInput, _moveInput) < 0;
        }
    }
    void ApplyMouseLook()
    {
        if (_svInputMouseDelta.Value == Vector2.zero) return;

        float senseX = _mouseSensitivity.x;
        float senseY = _mouseSensitivity.y;

        float mouseX = _svInputMouseDelta.Value.x * senseX * Time.deltaTime;
        float mouseY = _svInputMouseDelta.Value.y * senseY * Time.deltaTime;

        _xLookRotation += mouseY;
        _xLookRotation = Mathf.Clamp(_xLookRotation, -_clampXRotation, _clampXRotation);

        _yLookRotation -= mouseX;
        _yLookRotation %= 360f;

        _camPivot.localRotation = Quaternion.Euler(-_xLookRotation, 0f, 0f);
        _tf.localRotation = Quaternion.Euler(0f, -_yLookRotation, 0f);
    }

    private void FixedUpdate()
    {
        if (_svGamePaused.Value) return;
        ApplyMovement();
        ApplyGravity();
        BroadcastPlayerPos();
    }

    void ApplyMovement()
    {
        _rb.velocity = new(_moveVelocity.x, _rb.velocity.y, _moveVelocity.z);
    }
    void ApplyGravity()
    {
        //Probably still need to apply gravity during coyote time?
        //Although it doesn't apply to the coyote...
        if (_consideredGrounded) {
            _gravity = 0f;
        }
        else {
            _gravity = _gravityData.GetSpeed(_timeLastGrounded, Time.time);
        }

        //Apply Gravity & (TODO) Jump Force
        _rb.AddForce(Vector3.down * (_gravity - _currentJumpForce) * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
    void BroadcastPlayerPos()
    {
        _svPlayerPosition.Set(_rb.position);
    }

    float GetEffectiveMoveSpeed()
    {
        float spd = _speedData.GetSpeed(_timeLastIdle, Time.time);
        return _consideredGrounded ? spd : spd * _airSpeedMult;
    }

}

[System.Serializable]
public struct PlayerAccelerationData
{
    public float InitialSpeed;
    public float MaxSpeed;
    public float DurationAccelerate;
    public AnimationCurve AccelerationCurve;

    public PlayerAccelerationData(float initialSpeed, float maxSpeed, float durationAccelerate, AnimationCurve accelerationCurve)
    {
        InitialSpeed = initialSpeed;
        MaxSpeed = maxSpeed;
        DurationAccelerate = durationAccelerate;
        AccelerationCurve = accelerationCurve;
    }

    public float GetSpeed(float moveStartTime, float currentTime)
    {
        float t = GetEffectiveMoveTime(moveStartTime, currentTime);
        float valueT = AccelerationCurve.Evaluate(t);
        return Mathf.Lerp(InitialSpeed, MaxSpeed, valueT);
    }

    public float GetEffectiveMoveTime(float startTime, float currentTime)
    {
        return (currentTime - startTime) / DurationAccelerate;
    }
}