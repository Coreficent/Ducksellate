namespace Coreficent.Display
{
    public class Obstacle : Piece
    {
        void Start()
        {
            RandomizeDelta();
            Reposition();
        }
    }
}