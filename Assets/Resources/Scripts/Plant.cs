using System;
using UnityEngine;

public class Plant : ProgressObjective
{
    public override string GetDescription()
    {
        return "Water the plant";
    }

    public override float GetWeight()
    {
        return 0.5f;
    }

    internal override string GetCompletedText()
    {
        return "Completed!";
    }

    internal override string GetHint()
    {
        return "Press W to water plant";
    }

    internal override KeyCode GetKeyCode()
    {
        return KeyCode.W;
    }

    internal override int GetProgressStep()
    {
        return 10;
    }
}