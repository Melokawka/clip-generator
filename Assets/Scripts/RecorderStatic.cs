using UnityEngine;

using System.Collections;
using System.IO;
using System.Threading.Tasks;
public class RecorderStatic : MonoBehaviour
{
    private int fps;
    private int resolution;
    private float duration;
    private string savePath;
    
    private int frameCount = 0;
    private float timeElapsed = 0f;

    public async Task Record(int fps, int resolution, float duration, string savePath)
    {
        this.fps = fps;
        this.resolution = resolution;
        this.duration = duration;
        this.savePath = savePath;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        await CaptureFramesStaticAsync();
    }

    private Task CaptureFramesStaticAsync()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        StartCoroutine(CaptureFramesStatic(tcs));
        return tcs.Task;
    }

    IEnumerator CaptureFramesStatic(TaskCompletionSource<bool> tcs)
    {
        while (timeElapsed < duration)
        {
            yield return new WaitForSeconds(1f / fps);
            CaptureFrameStatic();
            timeElapsed += 1f / fps;
        }

        Debug.Log($"Recording complete for {gameObject.name}!");
        tcs.SetResult(true);  // Notify that the recording is done
    }

    void CaptureFrameStatic()
    {
        // Create a render texture for capturing
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        Camera cam = GetComponent<Camera>();
        cam.targetTexture = rt;

        // Capture the image
        Texture2D screenShot = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        screenShot.Apply();

        // Save image to file
        string filename = Path.Combine(savePath, $"frame_{frameCount:D04}.png");
        File.WriteAllBytes(filename, screenShot.EncodeToPNG());

        Debug.Log($"Saved: {filename}");

        // Cleanup
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        Destroy(screenShot);

        frameCount++;
    }
}