using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror3FAdd : MonoBehaviour
{
    private Transform head;
    private Transform playerHead;
    private Player player;
    private void Awake()
    {
        head = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
        player = GameObject.FindObjectOfType<Player>();
        playerHead = player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
    }
    private void LateUpdate()
    {
        head.transform.LookAt(playerHead.transform);
    }
    private void OnEnable()
    {
        player.GetLight().SetActive(false);
        transform.position = player.transform.position + player.transform.forward;
        transform.LookAt(player.transform);
    }
    private void OnDisable()
    {
        player.GetLight().SetActive(true);
    }
}
