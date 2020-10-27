namespace Coreficent.Animation
{
    public interface ITransitionable
    {
        void TransitionIn();
        void TransitionOut();
        bool TransitionInComplete();
        bool TransitionOutComplete();
    }
}