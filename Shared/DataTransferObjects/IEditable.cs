namespace OpenGitSync.Shared.DataTransferObjects
{
    public interface IEditable
    {
        void BeginEdit();
        void CancelEdit();
        void EndEdit();
    }
}
