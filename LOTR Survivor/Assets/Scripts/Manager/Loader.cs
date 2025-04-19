using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MenuScene,
        HubScene,
        TestMobile,
        Loading,
        SkillTree
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
        };

        UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.Loading.ToString());
        Time.timeScale = 1.0f;

        Debug.Log("LOADING");
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
