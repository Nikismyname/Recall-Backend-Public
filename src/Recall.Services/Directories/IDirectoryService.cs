namespace Recall.Services.Directories
{
    using Recall.Services.Models.DirectoryModels;
    using Recall.Services.Models.NavigationModels;

    public interface IDirectoryService
    {
        DirectoryIndex Create(DirectoryCreate create, int userId, bool isAdmin = false);

        int Delete(int id, int userId, bool isAdmin = false);

        AllFoldersFrontEndNaming[] GetForFolderSelect(int userId);

        void Edit(DirectoryEdit data, int userId);

        AllItemsFrontEndNaming[] GetForItemSelection(int userId);
    }
}
