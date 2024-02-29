using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemInitializer
{
    //Credit: Tarodev
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("[ SYSTEM ]")));
}
