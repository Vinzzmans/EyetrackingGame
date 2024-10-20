using OpenCvSharp;
using UnityEngine;
using System.Collections.Generic;

public class EyeTrackingWithOpenCV : MonoBehaviour
{
    public WebcamUI webcamScript; // Drag and drop the WebcamUI object in the Inspector
    private WebCamTexture webcamTexture;
    private CascadeClassifier faceCascade;  // Für Gesichtserkennung
    private CascadeClassifier eyeCascade;   // Für Augenerkennung
    public List<OpenCvSharp.Rect> detectedEyes = new List<OpenCvSharp.Rect>(); // Liste für erkannte Augen
    public List<OpenCvSharp.Rect> detectedFaces = new List<OpenCvSharp.Rect>(); // Liste für erkannte Gesichter

    void Start()
    {
        webcamTexture = webcamScript.webcam; // Verwende die bereits initialisierte Webcam aus WebcamUI
        webcamTexture.Play();

        // Debugging-Informationen
        Debug.Log("Webcam initialized: " + webcamScript.webcam.deviceName);
        Debug.Log("Webcam is playing: " + webcamScript.webcam.isPlaying);

        // Lade Haarcascades für Gesichts- und Augenerkennung
        faceCascade = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_frontalface_default.xml");
        eyeCascade = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_eye.xml");

        if (faceCascade.Empty() || eyeCascade.Empty())
        {
            Debug.LogError("Failed to load Haar Cascade for face or eye detection!");
        }
    }

    void Update()
    {
        if (webcamTexture == null)
        {
            Debug.LogError("WebCamTexture is not initialized!");
            return;
        }

        if (!webcamTexture.isPlaying)
        {
            Debug.LogError("WebCamTexture is not playing!");
            return;
        }

        Mat frame = OpenCvSharp.Unity.TextureToMat(webcamTexture);

        if (frame == null || frame.Empty())
        {
            Debug.LogWarning("Failed to convert webcam texture to Mat.");
            return;
        }

        Debug.Log("Frame captured successfully. Size: " + frame.Size());

        // Gesichter erkennen
        OpenCvSharp.Rect[] faces = faceCascade.DetectMultiScale(frame, scaleFactor: 1.1, minNeighbors: 5);
        detectedFaces.Clear(); // Zurücksetzen der Liste der erkannten Gesichter

        // Durch alle erkannten Gesichter gehen
        foreach (var face in faces)
        {
            detectedFaces.Add(face);  // Gesicht zur Liste hinzufügen
            Debug.Log($"Face detected at: {face.X}, {face.Y}");

            // Suche in jedem Gesicht nach Augen
            Mat faceRegion = new Mat(frame, face); // Teilbereich des Bildes für das Gesicht
            OpenCvSharp.Rect[] eyes = eyeCascade.DetectMultiScale(faceRegion, scaleFactor: 1.1, minNeighbors: 5);

            detectedEyes.Clear(); // Zurücksetzen der Liste der erkannten Augen
            foreach (var eye in eyes)
            {
                // Die Position der Augen relativ zum gesamten Bild berechnen
                OpenCvSharp.Rect eyeInFullImage = new OpenCvSharp.Rect(
                    face.X + eye.X, face.Y + eye.Y, eye.Width, eye.Height
                );
                detectedEyes.Add(eyeInFullImage);
                Debug.Log($"Eye detected at: {eyeInFullImage.X}, {eyeInFullImage.Y}");
            }
        }
    }

    void OnDrawGizmos()
    {
        // Setze Gizmo-Farbe für Gesichter
        Gizmos.color = Color.blue;
        foreach (var face in detectedFaces)
        {
            Vector3 facePosition = new Vector3(face.X + face.Width / 2, face.Y + face.Height / 2, 0);
            facePosition = Camera.main.ScreenToWorldPoint(new Vector3(facePosition.x, facePosition.y, 10)); // z=10 für Sichtbarkeit

            // Zeichne blaues Rechteck um erkannten Kopf
            Gizmos.DrawWireCube(facePosition, new Vector3(0.3f, 0.3f, 1)); // Größe anpassen
        }

        // Setze Gizmo-Farbe für Augen
        Gizmos.color = Color.red;
        foreach (var eye in detectedEyes)
        {
            Vector3 eyePosition = new Vector3(eye.X + eye.Width / 2, eye.Y + eye.Height / 2, 0);
            eyePosition = Camera.main.ScreenToWorldPoint(new Vector3(eyePosition.x, eyePosition.y, 10)); // z=10 für Sichtbarkeit

            // Zeichne rotes Rechteck um erkannten Augenbereich
            Gizmos.DrawWireCube(eyePosition, new Vector3(0.1f, 0.05f, 1)); // Größe anpassen
        }
    }
}
