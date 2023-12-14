using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    delegate void MyDelegate();
    MyDelegate delegateFunction;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribing to the delegate
        delegateFunction += PrintToTerminal;
        delegateFunction += PrintToTerminal2;

        // Call the delegate and avoid errors
        if (delegateFunction != null)
            delegateFunction();

        delegateFunction -= PrintToTerminal;
        delegateFunction -= PrintToTerminal2;
    }

    void PrintToTerminal() {
        Debug.Log("This is my first function");
    }

    void PrintToTerminal2() {
        Debug.Log("This is my second function");
    }
}
