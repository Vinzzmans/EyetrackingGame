using UnityEngine;

public class WebcamCheck : MonoBehaviour
{
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // Prüfe, ob Kameras gefunden wurden
        if (devices.Length > 0)
        {
            Debug.Log("Kameras gefunden:");
            for (int i = 0; i < devices.Length; i++)
            {
                Debug.Log(devices[i].name);
            }
        }
        else
        {
            Debug.LogWarning("Keine Kameras gefunden.");
        }
    }
}
