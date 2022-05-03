using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToHide;

    public void SetActiveToggle()
    {
        objectToHide.SetActive(!objectToHide.activeInHierarchy);
    }
}
