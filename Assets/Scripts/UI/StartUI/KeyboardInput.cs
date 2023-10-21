using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput
{
    public readonly KeyboardInputType keyboardInputType;
    public readonly string keyboardInputCharacter;

    public KeyboardInput(KeyboardInputType keyboardInputType, string keyboardInputCharacter = "")
    {
        this.keyboardInputType = keyboardInputType;
        this.keyboardInputCharacter = keyboardInputCharacter;
    }
}
