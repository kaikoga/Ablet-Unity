namespace Ablet.Hooks
{
    public interface IAbletEditorOnly
#if ABLET_VRCSDK3_AVATARS
: VRC.SDKBase.IEditorOnly
#endif
    {
    }
}
