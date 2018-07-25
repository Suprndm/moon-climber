using MoonClimber.Game.Sprites;

namespace MoonClimber.Interactive
{
    public class TreeObject : DecorativeObject
    {
        public TreeObject(int absoluteX, int absoluteY) : base(SpriteConst.Tree, 33, 35,0,-12, absoluteX, absoluteY)
        {
        }
    }
}
