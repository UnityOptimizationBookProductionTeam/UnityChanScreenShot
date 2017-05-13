using UnityEngine;
using UnityEngine.UI;

public class Posture : MonoBehaviour {
    private const string Pos = "Pos";
    private const string Rot = "Rot";

    private const string Play = "Play";
    private const string Pause = "Pause";

    private const float KeyScale = 0.01f;
    private const float KeyAngle = 5.0f;

    [SerializeField]
    private float _animSpeed = 0.25f;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GameObject _unityChan;

    [SerializeField]
    private Transform _ikObj;

    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private Light[] _lightList;

    [SerializeField]
    private Text _lhText;

    [SerializeField]
    private Text _rhText;

    [SerializeField]
    private Text _lfText;

    [SerializeField]
    private Text _rfText;

    [SerializeField]
    private Text _modeText;

    [SerializeField]
    private Text _pauseText;

    private Text[] _ikTextList;

    private IK[] _ikList;

    private IK _ikNow;

    private bool[] _ikEnabled;

    private bool _modeRot;

    private int _index = -1;

    private Color _defaultColor;

    void Start() {
        const float c = 0.196f;
        _defaultColor = new Color(c, c, c);

        _animator.speed = _animSpeed;

        IK[] list = _unityChan.GetComponents<IK>();

        _ikList = new IK[4];

        for (int i = 0, len = list.Length; i < 4; ++i) {
            for (int j = 0; j < len; ++j) {
                if (i == 0) {
                    if (list[j].type == IK.Type.LH) {
                        _ikList[0] = list[j];
                        break;
                    }
                }
                if (i == 1) {
                    if (list[j].type == IK.Type.RH) {
                        _ikList[1] = list[j];
                        break;
                    }
                }
                if (i == 2) {
                    if (list[j].type == IK.Type.LF) {
                        _ikList[2] = list[j];
                        break;
                    }
                }
                if (i == 3) {
                    if (list[j].type == IK.Type.RF) {
                        _ikList[3] = list[j];
                        break;
                    }
                }
            }
        }

        _ikTextList = new Text[4];

        _ikTextList[0] = _lhText;
        _ikTextList[1] = _rhText;
        _ikTextList[2] = _lfText;
        _ikTextList[3] = _rfText;

        _ikEnabled = new bool[4];
    }

    void Update() {
        // Light.
        Quaternion rot = _camera.transform.rotation;

        for (int i = 0, len = _lightList.Length; i < len; ++i) {
            _lightList[i].transform.rotation = rot;
        }

        // Pause.
        if (Input.GetKeyDown(KeyCode.P)) {
            float s, now = _animator.speed;

            if (now != 0.0f) {
                s = 0.0f;
                _pauseText.text = Pause;
            }
            else {
                s = _animSpeed;
                _pauseText.text = Play;
            }

            _animator.speed = s;
        }

        CheckIKControll();
    }

    private void CheckIKControll() {
        if (_ikNow != null) {
            Transform t = _ikNow.targetObj;

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(KeyAngle, Vector3.up);
                }
                else {
                    t.localPosition += -_camera.right * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(-KeyAngle, Vector3.up);
                }
                else {
                    t.localPosition += _camera.right * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(-KeyAngle, Vector3.right);
                }
                else {
                    t.localPosition += _camera.up * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(KeyAngle, Vector3.right);
                }
                else {
                    t.localPosition += -_camera.up * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(KeyAngle, Vector3.forward);
                }
                else {
                    t.localPosition += _camera.forward * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                if (_modeRot) {
                    t.localRotation *= Quaternion.AngleAxis(-KeyAngle, Vector3.forward);
                }
                else {
                    t.localPosition += -_camera.forward * KeyScale;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W)) {
                ClearIK();
            }
        }
    }

    public void OnMode() {
        _modeRot = !_modeRot;

        _modeText.text = _modeRot ? Rot : Pos;
    }

    public void OnNowIKCkear() {
        if (_ikNow != null) {
            _ikNow.initTargetObj = false;
        }

        if (_index >= 0) {
            _ikTextList[_index].color = _defaultColor;
            _ikObj.SetParent(null);
            _ikObj.gameObject.SetActive(false);
        }
    }

    public void OnIKLH() {
        _index = 0;
        ChangeIK();
    }

    public void OnIKRH() {
        _index = 1;
        ChangeIK();
    }

    public void OnIKLF() {
        _index = 2;
        ChangeIK();
    }

    public void OnIKRF() {
        _index = 3;
        ChangeIK();
    }

    private void ChangeIK() {
        for (int i = 0; i < 4; ++i) {
            if (i != _index) {
                _ikEnabled[i] = false;
                _ikTextList[i].color = _defaultColor;
                _ikList[i].ikActive = false;
            }
        }

        bool now = !_ikEnabled[_index];

        _ikEnabled[_index] = now;
        _ikList[_index].ikActive = now;

        if (now) {
            _ikNow = _ikList[_index];
            _ikTextList[_index].color = Color.red;
        }
        else {
            _ikNow = null;
            _ikTextList[_index].color = _defaultColor;
            _ikObj.SetParent(null);
            _ikObj.gameObject.SetActive(false);

            _index = -1;
        }
    }

    private void ClearIK(int index = -1) {
        if (index < 0) {
            for (int i = 0; i < 4; ++i) {
                _ikList[i].initTargetObj = false;
            }
        }
        else {
            _ikList[index].initTargetObj = false;
        }
    }
}