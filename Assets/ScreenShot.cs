using UnityEngine;

public class ScreenShot : MonoBehaviour {
    private const int SizeW = 512;
    private const int SizeH = 512;
    private const int Depth = 24;

    private const string FileHead = "UnityChan_";
    private const string FileConnect = "_";
    private const string FileConnect2 = "-";

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Camera _camera;

    private RenderTexture _renderTexture;

    private bool _screenShot;

    void Update() {
        if (!_screenShot && Input.GetKeyDown(KeyCode.O)) {
            StartScreenShot();
        }
    }

    public void OnPostRender() {
        _mainCamera.enabled = true;
        _camera.enabled = false;

        _screenShot = true;

        SaveScreenShot();
    }

    public void StartScreenShot() {
        if (_screenShot) {
            return;
        }

        _mainCamera.enabled = false;
        _camera.enabled = true;

        Transform mc = _mainCamera.transform;
        Transform c = _camera.transform;

        c.position = mc.position;
        c.rotation = mc.rotation;

        _camera.fieldOfView = _mainCamera.fieldOfView;

        _renderTexture = RenderTexture.GetTemporary(SizeW, SizeH, Depth, RenderTextureFormat.ARGB32);

        _camera.targetTexture = _renderTexture;

        _screenShot = false;
    }

    public void SaveScreenShot() {
        if (!_screenShot) {
            return;
        }

        RenderTexture original = RenderTexture.active;

        RenderTexture.active = _renderTexture;

        var tex2d = new Texture2D(SizeW, SizeH, TextureFormat.ARGB32, false, false);
        var rect = new Rect(0.0f, 0.0f, SizeW, SizeH);

        tex2d.ReadPixels(rect, 0, 0);
        tex2d.Apply(false, false);

        RenderTexture.active = original;

        RenderTexture.ReleaseTemporary(_renderTexture);
        _renderTexture = null;

        byte[] bin = tex2d.EncodeToPNG();

        System.DateTime dt = System.DateTime.Now;

        string fileName = FileHead + dt.Year.ToString() + dt.Month.ToString("D2") + dt.Day.ToString("D2") + FileConnect +
            dt.Hour.ToString("D2") + FileConnect2 + dt.Minute.ToString("D2") + FileConnect2 + dt.Second.ToString("D2") + ".png";

        System.IO.File.WriteAllBytes(fileName, bin);

        _screenShot = false;
    }
}
