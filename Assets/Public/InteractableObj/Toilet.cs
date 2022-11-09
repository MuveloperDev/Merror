using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    [SerializeField] private GameObject Flush_Water = null;
    [SerializeField] private float Up_Speed = 0.1f;
    [SerializeField] private float Down_Speed = 1f;
    public IEnumerator Flush()
    {
        // X Delta = 0.5f
        // Y Delta = 0.64f
        // Z scale delta  = 0.9f
        // X : Y = 1 : 1.28
        // Y * 0.781 = X
        // Y * 1.40 = Z scale
        float end_X = 0.2f;
        float end_Y = 0.5f;
        float end_Z_Scale = 0.55f;
        while (true)
        {
            if (Flush_Water.transform.localPosition.y <= end_Y)
            {
                Flush_Water.transform.localPosition = new Vector3(end_X,
                    end_Y, Flush_Water.transform.localPosition.z);
                Flush_Water.transform.localScale = new Vector3(Flush_Water.transform.localScale.x,
                    Flush_Water.transform.localScale.y, end_Z_Scale);
                Debug.Log("Finish Down : " + Flush_Water.transform.localPosition.y);
                break;
            }
            Flush_Water.transform.localPosition -= new Vector3(-Time.deltaTime * Down_Speed * 0.781f, Time.deltaTime * Down_Speed, 0f);
            Flush_Water.transform.localScale -= new Vector3(0f, 0f, Time.deltaTime * Down_Speed * 1.40f);
            yield return null;
        }
        end_X = -0.3f;
        end_Y = 1.14f;
        end_Z_Scale = 1.45f;
        while (true)
        {
            if (Flush_Water.transform.localPosition.y >= end_Y)
            {
                Flush_Water.transform.localPosition = new Vector3(end_X,
                    end_Y, Flush_Water.transform.localPosition.z);
                Flush_Water.transform.localScale = new Vector3(Flush_Water.transform.localScale.x,
                    Flush_Water.transform.localScale.y, end_Z_Scale);
                Debug.Log("Finish Up : " + Flush_Water.transform.localPosition.y);
                yield break;
            }
            Flush_Water.transform.localPosition += new Vector3(-Time.deltaTime * Up_Speed * 0.781f, Time.deltaTime * Up_Speed, 0f);
            Flush_Water.transform.localScale += new Vector3(0f, 0f, Time.deltaTime * Up_Speed * 1.40f);
            yield return null;
        }
    }
}
