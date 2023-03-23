using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{

    [SerializeField] private Text _fpsText;
    [SerializeField] private float _hudRefreshPerSec = 1f;

    private float _timer;

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 60;
        }
    }
    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "" + fps;
            _timer = Time.unscaledTime + _hudRefreshPerSec;
        }
    }
}
