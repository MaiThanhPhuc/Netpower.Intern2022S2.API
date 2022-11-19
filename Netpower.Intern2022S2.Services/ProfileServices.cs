using Microsoft.EntityFrameworkCore;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories.Interfaces;
using Netpower.Intern2022S2.Services.Interfaces;


namespace Netpower.Intern2022S2.Services
{
    public class ProfileServices : IProfileServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ProfileDTO> GetAll()
        {
            var result = _unitOfWork.ProfileRepository.GetAll().ToList<Profile>();
            return result.Select(x => new ProfileDTO(x)).ToList();
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var user = await (_unitOfWork.UserRepository.GetByIdAsync(id));
            if (user == null) return new ApiResponse(400, "User not exist", null!);

            var profile = await (_unitOfWork.ProfileRepository.GetByIdAsync(id));
            if (profile == null)return new ApiResponse(400, "Profile not exist", null!);
            
                
            
            var profileDTO = new ProfileDTO(profile);
            if (profileDTO.Image != null)
            {
                profileDTO.Image = GetImage(Convert.ToBase64String(profileDTO.Image));
            }
            return new ApiResponse(200, "Success", profileDTO);
        }
        //public async Task<ApiResponse> Post(ProfileDTO profileDTO)
        //{
        //    var user = await (_unitOfWork.UserRepository.GetByIdAsync(profileDTO.UserId));
        //    if (user == null) return new ApiResponse(false, "User not exist", null!);

        //    Profile profile = new Profile();
        //    profile.Birthday = profileDTO.Birthday;
        //    profile.PhoneNumber = profileDTO.PhoneNumber;
        //    profile.Address = profileDTO.Address;
        //    profile.Image = profileDTO.Image;
        //    profile.Sex = profileDTO.Sex;
        //    profile.UserId = profileDTO.UserId;
        //    profile.Nationality = profileDTO.Nationality;
        //    _unitOfWork.ProfileRepository.Add(profileDTO.ProfileDTOToProfile(profile));
        //    try
        //    {
        //        await _unitOfWork.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return new ApiResponse(false, "Error while saving data", null!);
        //    }
        //    return new ApiResponse(true, "Success", profileDTO);
        //}

        public async Task<ApiResponse> SaveImage(FileUpload fileObj)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(fileObj.UserId);
            if (user == null)
            {
                return new ApiResponse(400, "User not exist!", null!);
            }
            var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(fileObj.UserId);
            if (profile == null)return new ApiResponse(400, "Profile not exist", null!);

            if (fileObj.file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    fileObj.file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    profile.Image = fileBytes;

                    _unitOfWork.ProfileRepository.Update(profile);
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        return new ApiResponse(500, "Error while saving data", null!);
                    }
                }
                return new ApiResponse(200, "Upload Success", null!);
            }
            return new ApiResponse(400, "Upload Failed", null!);
        }


        public async Task<ApiResponse> PutProfile(ProfileDTO profileDTO)
        {
            var user = await (_unitOfWork.UserRepository.GetByIdAsync(profileDTO.UserId));
            if (user == null) return new ApiResponse(400, "User not exist", null!);

            var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(profileDTO.UserId);
            if (profile == null)return new ApiResponse(400, "Profile not exist", null!);

            profile.Birthday = profileDTO.Birthday;
            profile.PhoneNumber = profileDTO.PhoneNumber;
            profile.Address = profileDTO.Address;
            profile.Image = profileDTO.Image;
            profile.Sex = profileDTO.Sex;
            profile.UserId = profileDTO.UserId;
            profile.Nationality = profileDTO.Nationality;

            _unitOfWork.ProfileRepository.Update(profileDTO.ProfileDTOToProfile(profile));
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return new ApiResponse(500, "Error while saving data", null!);

            }
            return new ApiResponse(200, "Success", profileDTO);
        }
        
        private byte[] GetImage(string sBase64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(sBase64String))
            {
                bytes = Convert.FromBase64String(sBase64String);
            }
            return bytes!;
        }

    }
}
