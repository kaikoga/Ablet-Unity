using JetBrains.Annotations;

namespace Ablet.Locales
{
    static class Translations
    {
        [UsedImplicitly]
        static void Tr(string _) { }

        [UsedImplicitly]
        static void Tr()
        {
            Tr("AbletConsoleWindowView::sceneEntrypointsPopup");
            Tr("AbletConsoleWindowView::entrypointObjectField");
        }
    }
}