using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewScene : MonoBehaviour
{
    public void moveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }


    public void quitMM()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif    


    }
}
