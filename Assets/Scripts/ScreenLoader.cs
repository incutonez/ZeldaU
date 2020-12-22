using System.Collections;
using UnityEngine;

// Taken from https://www.youtube.com/watch?v=CE9VOZivb3I
public class ScreenLoader : MonoBehaviour
{
    public Animator transition;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel(8, 0);
        }
    }

    public void LoadNextLevel(int x, int y)
    {
        StartCoroutine(LoadLevel(x, y));
    }

    public IEnumerator LoadLevel(int x, int y)
    {
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(0.5f);

        GameHandler.sceneBuilder.BuildScene(x, y);

        yield return new WaitForSeconds(0.5f);

        transition.SetBool("Start", false);
    }
}
