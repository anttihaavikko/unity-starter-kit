using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class SceneChangerObject : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        SceneChanger.Instance.ChangeScene(scene);
    }

    public void Quit()
    {
        SceneChanger.Instance.blinders.Close();
        this.StartCoroutine(() => Application.Quit(), 1f);
    }
}
