﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.PostProcessing;

public class TestCode3 : MonoBehaviour
{
    public GameObject sampleTown;
    public PostProcessingBehaviour postProcessing;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Fog() 
    {
        RenderSettings.fog = !RenderSettings.fog;
    }

    private void Post()
    {
        postProcessing.enabled = !postProcessing.enabled;
    }

    /*void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 150), "포그"))
        {
            Fog();
        }

        if (GUI.Button(new Rect(0, 150, 200, 150), "sample town 1"))
        {
            sampleTown.SetActive(!sampleTown.activeSelf);
        }

        if (GUI.Button(new Rect(0, 300, 200, 150), "포스트 프로세싱"))
        {
            Post();
        }
    }*/
}
