namespace Lex.Module.ImportExport.Core.Domain;

public interface IImportExportRepository
{
    void AddExportJob(ExportJob job);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
