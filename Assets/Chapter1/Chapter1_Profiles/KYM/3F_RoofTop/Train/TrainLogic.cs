using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLogic : MonoBehaviour
{

    [SerializeField, Range(0f, 2f)] private float speed = 1f;
    [SerializeField, Range(0f, 90f)] private float turnRate = 3f;
    [SerializeField] private Transform pivot_1 = null;
    [SerializeField] private Transform pivot_2 = null;
    [SerializeField] private Transform startPos = null;
    private bool move = false;
    private bool turnLeft = false;
    private bool turnRight = false;
    private void Start()
    {
        move = true;
    }
    private void Update()
    {
        if(move)
        {
            transform.position += speed * Time.deltaTime * transform.forward;
        }


        if(turnLeft)
        {
            transform.RotateAround(pivot_1.position, pivot_1.up, - Time.deltaTime * turnRate);
        }
        else if(turnRight)
        {
            transform.RotateAround(pivot_2.position, pivot_2.up, Time.deltaTime * turnRate);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "1") // Turn left trigger
        {
            turnLeft = true;
            move = false;
        }
        else if(other.gameObject.name == "2") // Stop turn trigger
        {
            if(turnLeft)
            {
                turnLeft = false;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.y);
            }
            move = true;
        }
        else if(other.gameObject.name == "3") // Turn right trigger
        {
            turnRight = true;
            move = false;
        }
        else if (other.gameObject.name == "4")
        {
            move = true;
            if (turnRight)
            {
                turnRight = false;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90f, transform.rotation.z);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.position.z);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
    }
    private IEnumerator Reset()
    {
        while(true)
        {
            transform.position = Vector3.Lerp(transform.position, startPos.position, Time.deltaTime * 2f);
            if(transform.position == startPos.position)
            {
                yield break;
            }
            yield return null;
        }
    }
}
