using UnityEngine;
using UnityEngine.UI;

public class BC_MODE : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    void Start()
    {
      if(toggle == null)
        {
            toggle = GetComponent<Toggle>();
            if(toggle == null )
            {
                Debug.LogError("toggle component not found on " + gameObject.name);
                return;

            }
        }
        bool savedState = PlayerPrefs.GetInt("BCMode", 0) == 1;

        toggle.isOn = savedState;

        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        Debug.Log("Initial BC Mode State: " + savedState);



    }
    public void OnToggleValueChanged(bool isOn)
    {
        PlayerPrefs.SetInt("BCMode", isOn  ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("BCModde changed to " + isOn);

    }

    
}
