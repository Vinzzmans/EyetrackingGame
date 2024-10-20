using UnityEngine;
using TMPro;  // Wenn du TextMeshPro für den Text verwendest

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;  // Ziehe dein TextMeshPro-Element hierhin
    private float deltaTime = 0.0f;

    void Update()
    {
        // FPS berechnen
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // FPS im Text-Element anzeigen
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}
