/// <summary>
/// Any script that contains death instructions for a character (or object, in come cases) 
/// will be of this interface so it can be used by their "health" script
/// </summary>
public interface IDeathInstructions {
    void executeDeath(int healthLeftBeforeDeath);
}
