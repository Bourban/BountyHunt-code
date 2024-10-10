using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    [SerializeField]
    private Image CrosshairImage;

    private void Start()
    {
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetActive(bool isActive) 
    {
        Cursor.visible = !isActive;
        CrosshairImage.enabled = isActive;
    }
}
