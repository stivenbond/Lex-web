namespace Lex.Module.AssessmentDelivery.Core.Domain;

public interface IDeliveryRepository
{
    void AddSession(AssessmentSession session);
    Task SaveChangesAsync(CancellationToken ct = default);
}
