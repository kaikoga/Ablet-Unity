namespace Ablet.API
{
    public static class AbletSymbols
    {
#if PREFER_ABLET
        public static bool PreferAblet => true;
#else
        public static bool PreferAblet => false;
#endif        
    }
}
