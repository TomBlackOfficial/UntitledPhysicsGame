using System;
using UnityEngine;
using UnityEngine.UI;

public class TextMeshSharpener : MonoBehaviour
{
    public static event Action<string> buttonPressed;
    public static event Action<string, TextMesh> buttonHover;

    private void OnMouseDown()
    {
        buttonPressed?.Invoke(text);
    }
    /*
	Makes TextMesh look sharp regardless of camera size/resolution
	Do NOT change character size or font size; use scale only
	*/

    // Properties
    private float lastPixelHeight = -1;
    private TextMesh textMesh;
    private string text;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        text = textMesh.text;    
    }

    void Start()
    {
        resize();
    }

    void Update()
    {
        // Always resize in the editor, or when playing the game, only when the resolution changes
        if (Camera.main.pixelHeight != lastPixelHeight || (Application.isEditor && !Application.isPlaying)) resize();
    }

    private void OnMouseOver()
    {
        buttonHover?.Invoke(text, textMesh);
    }

    private void resize()
    {
        float ph = Camera.main.pixelHeight;
        float ch = Camera.main.orthographicSize;
        float pixelRatio = (ch) / ph;
        float targetRes = 64f;
        textMesh.characterSize = pixelRatio * Camera.main.orthographicSize / Math.Max(transform.localScale.x, transform.localScale.y);
        textMesh.fontSize = (int)Math.Round(targetRes / textMesh.characterSize);
        lastPixelHeight = ph;
    }
}