using Netpower.Intern2022S2.Entities.Models;

namespace Netpower.Intern2022S2.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        TrainingContext DbContext { get; }
        IRepository<Profile> ProfileRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<VerificationToken> VerificationRepository { get; }
        void Dispose();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}