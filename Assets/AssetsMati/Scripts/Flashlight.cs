using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight;
    private bool flashlightActive = false;

    void Start()
    {
        flashlightLight.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightActive = !flashlightActive;
            flashlightLight.SetActive(flashlightActive);
        }
    }
}