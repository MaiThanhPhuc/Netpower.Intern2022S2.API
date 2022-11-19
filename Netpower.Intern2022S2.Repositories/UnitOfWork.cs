using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        public UnitOfWork(TrainingContext context, IRepository<User> userRepository, IRepository<Profile> profileRepository, IRepository<VerificationToken> verificationRepository)
        {

            DbContext = context;
            UserRepository = userRepository;
            UserRepository.DbContext = DbContext;
            ProfileRepository = profileRepository;
            ProfileRepository.DbContext = DbContext;
            VerificationRepository = verificationRepository;
            VerificationRepository.DbContext = DbContext;

        }
        public TrainingContext DbContext { get; }

        public IRepository<User> UserRepository { get; }
        public IRepository<Profile> ProfileRepository { get; }
        public IRepository<VerificationToken> VerificationRepository { get; }

        public int SaveChanges()
        {
            var iResult = DbContext.SaveChanges();
            return iResult;
        }

        public async Task<int> SaveChangesAsync()
        {
            var iResult = await DbContext.SaveChangesAsync();
            return iResult;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }


    }
}
