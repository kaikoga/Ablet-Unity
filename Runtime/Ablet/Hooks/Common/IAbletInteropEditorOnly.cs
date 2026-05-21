namespace Ablet.Hooks.Common
{
    // WARNING: avoid using this interface unless when really intentional,
    // because assembly references to every possible platform SDKs may be required...
    // 
    // note: CVR.CCKEditor.ContentBuilder.ICCKEditorOnly is not included because Assembly-CSharp
    public interface IAbletInteropEditorOnly
#if ABLET_VRCSDK3_AVATARS
        : VRC.SDKBase.IEditorOnly
#endif
    {
    }
}
