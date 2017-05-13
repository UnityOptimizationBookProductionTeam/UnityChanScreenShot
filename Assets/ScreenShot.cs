using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour {
    private const int Depth = 32;

    private const string FileHead = "UnityChan_";
    private const string FileConnect = "_";
    private const string FileConnect2 = "-";

    [SerializeField]
    private UnityChan.FaceUpdate _faceUpdate;

    [SerializeField]
    private MeshRenderer _plane;

    [SerializeField]
    private Canvas _ui;

    private bool _start;

    void Update() {
        if (!_start && Input.GetKeyDown(KeyCode.O)) {
            _faceUpdate.isGUI = false;
            _plane.enabled = false;
            _ui.enabled = false;

            _start = true;
        }
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest);

        if (!_start) {
            return;
        }

        SaveRenderTexture(src);

        _faceUpdate.isGUI = true;
        _plane.enabled = true;
        _ui.enabled = true;

        _start = false;
    }

    public void SaveRenderTexture(RenderTexture dest) {
        RenderTexture original = RenderTexture.active;

        RenderTexture.active = dest;

        int w = Screen.width, h = Screen.height;
        var tex2d = new Texture2D(w, h, TextureFormat.ARGB32, false, true);
        var rect = new Rect(0.0f, 0.0f, (float)w, (float)h);

        tex2d.ReadPixels(rect, 0, 0);
        tex2d.Apply(false, false);

        RenderTexture.active = original;

        byte[] bin = tex2d.EncodeToPNG();

        System.IO.File.WriteAllBytes(CreateFileName(), bin);
    }

    private string CreateFileName() {
        System.DateTime dt = System.DateTime.Now;

        return FileHead + dt.Year.ToString() + dt.Month.ToString("D2") + dt.Day.ToString("D2") + FileConnect +
            dt.Hour.ToString("D2") + FileConnect2 + dt.Minute.ToString("D2") + FileConnect2 + dt.Second.ToString("D2") + ".png";
    }
}
