using System.Threading.Tasks;
using MailTest.Models;

namespace MailTest.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}