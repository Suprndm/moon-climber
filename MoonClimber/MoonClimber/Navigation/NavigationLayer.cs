using MoonClimber.Game.Pages.Game;
using Odin.Core;
using Odin.Navigation;

namespace MoonClimber.Navigation
{
    public class NavigationLayer:OView
    {
        public NavigationLayer()
        {
         
            SetupNavigation();
        }

        public void SetupNavigation()
        {
            var gamePage = new GamePage();
            AddChild(gamePage);

            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
        }
    }
}
