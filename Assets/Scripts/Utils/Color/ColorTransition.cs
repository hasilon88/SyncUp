
public class ColorTransition
{

    public float Red;
    public float Green;
    public float Blue;

    public ColorTransition(float red, float green, float blue)
    {
        this.Red = red;
        this.Green = green;
        this.Blue = blue;
    }

    public override string ToString()
    {
        return "Transition to next ColorNode Red: " + this.Red + "\n"
            + "Transition to next ColorNode Green: " + this.Green + "\n"
            + "Transition to next ColorNode Blue: " + this.Blue + "\n";
    }

}
