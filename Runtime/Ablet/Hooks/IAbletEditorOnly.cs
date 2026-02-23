namespace Ablet.Hooks
{
    public interface IAbletEditorOnly
#if ABLET_VRCSDK3_AVATARS
    // FIXME: remove VRCSDK dll reference from inheritors 
: VRC.SDKBase.IEditorOnly
#endif
    {
    }
}
