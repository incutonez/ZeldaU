using System;
using System.Collections;
using UnityEngine;

public class SetIntervalHook : MonoBehaviour
{
    public Action OnUpdate { get; set; }
    public float Timeout { get; set; }

    private void Start()
    {
        StartCoroutine(SetInterval());
    }

    private IEnumerator SetInterval()
    {
        while (true)
        {
            OnUpdate?.Invoke();
            yield return new WaitForSeconds(Timeout);
        }
    }

    public static SetIntervalHook Create(Action onUpdate, float timeout)
    {
        SetIntervalHook setTimeoutHook = new GameObject("SetTimeout").AddComponent<SetIntervalHook>();
        setTimeoutHook.Timeout = timeout;
        setTimeoutHook.OnUpdate = onUpdate;
        return setTimeoutHook;
    }
}