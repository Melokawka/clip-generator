using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

public class RecorderArc : MonoBehaviour
{
    private int fps;
    private int resolution;
    private float duration;
    private string savePath;
    private float rotationSpeed;
    
    private int frameCount = 0;
    private float timeElapsed = 0f;

    public async Task Record(int fps, int resolution, float duration, string savePath, float rotationSpeed)
    {
        this.fps = fps;
        this.resolution = resolution;
        this.duration = duration;
        this.savePath = savePath;
        this.rotationSpeed = rotationSpeed;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        await CaptureFramesArcAsync();
    }

    private Task CaptureFramesArcAsync()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        StartCoroutine(CaptureFramesArc(tcs));
        return tcs.Task;
    }

    IEnumerator CaptureFramesArc(TaskCompletionSource<bool> tcs)
    {
        while (timeElapsed < duration)
        {
            yield return new WaitForSeconds(1f / fps);
            CaptureFrameArc();
            timeElapsed += 1f / fps;
        }

        Debug.Log($"Recording complete for {gameObject.name}!");
        tcs.SetResult(true);  // Notify that the recording is done
    }

    void CaptureFrameArc()
    {
        GameObject pivot = this.transform.parent.gameObject;
        float angleStep = rotationSpeed / fps;
        pivot.transform.Rotate(Vector3.up, angleStep);

        // Create a render texture for capturing
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        Camera cam = GetComponent<Camera>();
        cam.targetTexture = rt;

        Texture2D screenShot = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        screenShot.Apply();

        string filename = Path.Combine(savePath, $"frame_{frameCount:D04}.png");
        File.WriteAllBytes(filename, screenShot.EncodeToPNG());

        Debug.Log($"Saved: {filename}");

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        Destroy(screenShot);

        frameCount++;
    }
}
