using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;


namespace Netpower.Intern2022S2.Services.Interfaces
{
    public interface IProfileServices
    {
        public List<ProfileDTO> GetAll();
        public Task<ApiResponse> GetById(Guid id);
        //public Task<ApiResponse> Post(ProfileDTO profile);
        public Task<ApiResponse> PutProfile(ProfileDTO profile);
        public Task<ApiResponse> SaveImage(FileUpload fileObj);
    }
}
