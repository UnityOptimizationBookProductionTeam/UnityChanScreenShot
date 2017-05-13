using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IK : MonoBehaviour {
    public enum Type {
        NONE,
        LH,
        RH,
        LF,
        RF
    }

    [SerializeField]
    private Type _type;

    public Type type {
        get {
            return _type;
        }
    }

    [SerializeField]
    private Transform _targetObj;

    public Transform targetObj {
        get {
            return _targetObj;
        }
    }

    [SerializeField]
    private Transform _ikObj;

    public Transform ikObj {
        get {
            return _ikObj;
        }
    }

    private bool _ikActive;

    public bool ikActive {
        set {
            _ikActive = value;
        }
    }

    protected Animator _animator;

    private AvatarIKGoal _ikGoal;

    private bool _initTargetObj;

    public bool initTargetObj {
        set {
            _initTargetObj = value;
        }
    }

    private bool _lastActive;

    void Start() {
        _animator = GetComponent<Animator>();

        if (_type == Type.RH) {
            _ikGoal = AvatarIKGoal.RightHand;
        }
        else if (_type == Type.LH) {
            _ikGoal = AvatarIKGoal.LeftHand;
        }
        else if (_type == Type.RF) {
            _ikGoal = AvatarIKGoal.RightFoot;
        }
        else if (_type == Type.LF) {
            _ikGoal = AvatarIKGoal.LeftFoot;
        }
    }

    void OnAnimatorIK(int layerIndex) {
        if (_animator != null && _type != Type.NONE) {
            if (_ikActive) {
                if (!_initTargetObj) {
                    _initTargetObj = true;
                    _targetObj.SetPositionAndRotation(_animator.GetIKPosition(_ikGoal), _animator.GetIKRotation(_ikGoal));
                }

                if (_ikActive != _lastActive) {
                    _ikObj.gameObject.SetActive(true);
                    _ikObj.SetParent(_targetObj, false);
                    _ikObj.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                    _ikObj.localPosition = Vector3.zero;
                    _ikObj.localRotation = Quaternion.identity;
                }
            }

            if (_initTargetObj) {
                _animator.SetIKPositionWeight(_ikGoal, 1.0f);
                _animator.SetIKRotationWeight(_ikGoal, 1.0f);
                _animator.SetIKPosition(_ikGoal, _targetObj.position);
                _animator.SetIKRotation(_ikGoal, _targetObj.rotation);
            }

            _lastActive = _ikActive;
        }
    }
}
