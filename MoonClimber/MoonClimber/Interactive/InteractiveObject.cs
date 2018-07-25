using Odin.Core;

namespace MoonClimber.Interactive
{
    public abstract class InteractiveObject : OView
    {
        protected InteractiveObject(int absoluteX, int absoluteY)
        {
            AbsoluteX = absoluteX;
            AbsoluteY = absoluteY;
        }

        public int AbsoluteX { get;   }
        public int AbsoluteY { get;   }

        public virtual void Load(float x, float y)
        {

        }

        public virtual void Unload()
        {

        }
    }
}
