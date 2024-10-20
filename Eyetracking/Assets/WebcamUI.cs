using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamUI : MonoBehaviour
{
    public WebCamTexture webcam;
    public GameObject img;
    // Start is called before the first frame update
    void Awake()
    {
        webcam = new WebCamTexture();
        img.GetComponent<Renderer>().material.mainTexture = webcam;
        webcam.Play();
    }
}
