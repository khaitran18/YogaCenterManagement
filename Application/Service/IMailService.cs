using Domain.Model;
namespace Application.Service
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailDataModel mailData, CancellationToken ct);
    }
}
