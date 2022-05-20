using JHExercise.API.Initialization;

namespace JHExercise.API;
public class Program
{
    static void Main(string[] args)
    {
        new Bootstrapper().Initialize(args);
    }
}