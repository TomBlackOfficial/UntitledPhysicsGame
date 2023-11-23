using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    public GameObject Pcircle, PcircleBP, currentCircle, currentCircleBP, parent;
    public GameObject[] Pcars = new GameObject[2];

    public float circleGrowthRate = 0.1f;

    private void Awake()
    {
        Pcircle = Resources.Load<GameObject>("Circle");
        PcircleBP = Resources.Load<GameObject>("Circle_Blueprint");
        parent = GameObject.Find("MainMenu");
    }

    private void Update()
    {
        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 tempScale;

        if (Input.GetMouseButtonDown(1))
        {
            currentCircle = null;
            currentCircleBP = Instantiate(PcircleBP, mousePos, Quaternion.identity, parent.transform);
        }
        if(Input.GetMouseButton(1))
        {
            currentCircleBP.transform.position = mousePos;
            tempScale = currentCircleBP.transform.localScale;
            tempScale.x = Mathf.Min(tempScale.x + (circleGrowthRate * Time.deltaTime), 1);
            tempScale.y = Mathf.Min(tempScale.y + (circleGrowthRate * Time.deltaTime), 1);
            tempScale.z = Mathf.Min(tempScale.z + (circleGrowthRate * Time.deltaTime), 1);
            currentCircleBP.transform.localScale = tempScale;
        }
        if(Input.GetMouseButtonUp(1))
        {
            currentCircle = Instantiate(Pcars[Random.value < 0.5? 0 : 1], mousePos, Quaternion.identity, parent.transform);
            currentCircle.transform.localScale = currentCircleBP.transform.localScale;
            currentCircle.GetComponent<Rigidbody2D>().mass *= currentCircle.transform.localScale.x;
            Destroy(currentCircleBP);
            tempScale = Vector3.zero;
        }
    }
}

//currentCircle.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
