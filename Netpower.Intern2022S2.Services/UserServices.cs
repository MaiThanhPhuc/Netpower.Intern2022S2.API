using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories.Interfaces;
using Netpower.Intern2022S2.Services.Interfaces;


namespace Netpower.Intern2022S2.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<UserDTO> Get()
        {
            var result = _unitOfWork.UserRepository.GetAll().ToList<User>();
            return result.Select(x => new UserDTO(x)).ToList();
        }
        public async Task<ApiResponse> GetById(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            var userDTO = new UserDTO(user);
            if (user == null)
            {
                return new ApiResponse(400, "User not found", null!);
            }
            return new ApiResponse(200, "Success!", userDTO);
        }
        public async Task<ApiResponse> Post(UserDTO userDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userDTO.UserId);
            if (user == null)
            {
                return new ApiResponse(400, "UserId not found", null!);
            }
            _unitOfWork.UserRepository.Add(userDTO.UserDTOtoUser(user));
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
               return new ApiResponse(500, "Error while saving data", null!);
            }
            return new ApiResponse(200, "Success", userDTO);
        }
        public async Task<ApiResponse> PutUser(UserUpdateDTO userDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userDTO.UserId);
            if(user == null)
            {
                return new ApiResponse(400, "User not exist!", null!);
            }
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);


            _unitOfWork.UserRepository.Update(user);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               return new ApiResponse(500, "Error while saving data", null!);
            }
            return new ApiResponse(200, "Update success", userDTO);
        }
       


       
    }
}