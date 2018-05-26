using System.Threading.Tasks;
using Odin.Core;
using Odin.Paint;
using Odin.Shapes;
using SkiaSharp;

namespace MoonClimber
{
    public class Game : ORoot
    {
        private PaintStorage _paintStorage;
        public override Task LoadAssets()
        {
            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            _paintStorage = new PaintStorage();
            _paintStorage.DeclarePaint("red", new SKPaint() { Color = new SKColor(255, 0, 0) });

            AddChild(new Rectangle(100, 100, 100, 100, _paintStorage.GetPaint("red")));
        }
    }
}
