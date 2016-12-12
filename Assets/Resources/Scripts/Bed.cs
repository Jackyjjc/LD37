using System;
using UnityEngine;

public class Bed : ProgressObjective
{
    public override string GetDescription()
    {
        return "Make the bed";
    }

    public override float GetWeight()
    {
        return 1;
    }

    internal override string GetCompletedText()
    {
        return "Completed!";
    }

    internal override string GetHint()
    {
        return "Press M to make bed";
    }

    internal override KeyCode GetKeyCode()
    {
        return KeyCode.M;
    }

    internal override int GetProgressStep()
    {
        return 10;
    }
}
