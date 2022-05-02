using UnityEngine;

public class TitorialButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToHide;

    public void SetActiveToggle()
    {
        objectToHide.SetActive(!objectToHide.activeInHierarchy);
    }
}
