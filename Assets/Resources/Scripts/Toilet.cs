using System;
using UnityEngine;

public class Toilet : ProgressObjective
{
    internal override string GetHint()
    {
        return "Press C to clean";
    }

    internal override string GetCompletedText()
    {
        return "Cleaned!";
    }

    internal override int GetProgressStep()
    {
        return 15;
    }

    internal override KeyCode GetKeyCode()
    {
        return KeyCode.C;
    }

    public override string GetDescription()
    {
        return "Clean the toilet";
    }

    public override float GetWeight()
    {
        return 1;
    }
}
