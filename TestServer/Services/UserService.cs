using System;
using Microsoft.EntityFrameworkCore;
using TestServer.Attributes;


namespace TestServer.Services
{
    public interface IUserService
    {
        Task<IBaseResponse<User>> GetByIdAsync(int id);
        Task CreateAsync(User user);
    }
    [DITransient]
    public class UserService(MobileContext _dbContext) : IUserService
    {

        public async Task CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IBaseResponse<User>> GetByIdAsync(int id)
        {
            var baseResponse = new BaseResponse<User>() { Description = "" };
            try
            {
                baseResponse.Description = "Hello from User.GetById";
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = await _dbContext.Users.FirstAsync(u => u.Id == id);
                return baseResponse;
            }
            catch (Exception)
            {
                baseResponse.Description = "Fuck u no data";
                baseResponse.StatusCode = StatusCode.InternalServerError;
                return baseResponse;
            }
        }
    }


}