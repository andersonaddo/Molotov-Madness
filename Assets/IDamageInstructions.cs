/// <summary>
/// Any script that contains instructions on how a character (or object, in come cases) 
/// should behave when damaged will be of this interface so it can be used by their "health" script
/// </summary>
public interface IDamageInstructions{
    void executeDamageInstructions(int damage);
}
