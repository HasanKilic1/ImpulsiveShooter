using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static void RunAfterDelay(MonoBehaviour monoBehaviour , float delay , Action task , out Coroutine coroutine)
    {
        coroutine = monoBehaviour.StartCoroutine(RunAfterDelay(delay , task));
    }

    private static IEnumerator RunAfterDelay(float delay , Action task )
    {
        yield return new WaitForSeconds(delay);
        task.Invoke();
    }
}
