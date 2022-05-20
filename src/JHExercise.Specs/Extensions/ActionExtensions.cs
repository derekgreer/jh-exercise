using System;

namespace JHExercise.Specs.Extensions;

public static class ActionExtensions
{
    public static void Repeat(this Action<int> action, int count)
    {
        for (int i = 0; i < count; i++)
        {
            action(i);
        }
    }
}