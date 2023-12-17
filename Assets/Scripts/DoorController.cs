using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform doorTransform;
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openTime;
    [SerializeField] float closeTime;
    [SerializeField] int keysRequired;
    [SerializeField] NavMeshAgent navMeshAgent;

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isOpening;
    private bool isClosing;
    private bool hasOpened;
    private float startTime;

    Key key;

    void Start()
    {
        initialRotation = doorTransform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(0f, openAngle, 0f);

        key = FindObjectOfType<Key>();
    }

    void Update()
    {
        if (isOpening)
        {
            float t = (Time.time - startTime) / openTime;
            doorTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            if (t >= 1f)
            {
                isOpening = false;
                navMeshAgent.enabled = true;
            }
        }
        else if (isClosing)
        {
            float t = (Time.time - startTime) / closeTime;
            doorTransform.rotation = Quaternion.Lerp(targetRotation, initialRotation, t);

            if (t >= 1f)
            {
                isClosing = false;
            }
        }
    }

    public void OpenDoor()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;
            startTime = Time.time;
            FindObjectOfType<PlayerMovement>().StopMoving();
            hasOpened = true;
        }
    }

    public void CloseDoor()
    {
        if (!isClosing && !isOpening && hasOpened)
        {
            isClosing = true;
            startTime = Time.time;
            hasOpened = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (key.keyCount >= keysRequired)
            {
                OpenDoor();
                key.ReduceCurrentKey(keysRequired);
            }
            else
            {
                FindObjectOfType<PlayerMovement>().StopMoving();
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            CloseDoor();
        }
    }
}
