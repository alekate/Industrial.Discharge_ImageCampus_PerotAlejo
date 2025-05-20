using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player_CamaraChanger : MonoBehaviour
{
    [SerializeField] private GameObject firstPerCam;
    [SerializeField] private GameObject thirdPerCam;

    public bool isFirstPerson = true;

    [SerializeField] private TMP_Text crosshairText;

    [SerializeField] private Player_Shoot player_shoot;

    [SerializeField] private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        thirdPerCam.SetActive(false);
        lineRenderer.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
            {
                firstPerCam.SetActive(true);
                thirdPerCam.SetActive(false);

                crosshairText.gameObject.SetActive(true);

                lineRenderer.enabled = false;


            }

            if (!isFirstPerson)
            {

                firstPerCam.SetActive(false);
                thirdPerCam.SetActive(true);

                crosshairText.gameObject.SetActive(false);

                lineRenderer.enabled = true;

            }

        }

    }
}
