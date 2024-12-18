using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveExample : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefab, mainCamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10, Quaternion.identity);
        }

        Vector3 moveVector = Vector3.up * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal");

        transform.position += moveVector * Time.deltaTime;
    }
}
