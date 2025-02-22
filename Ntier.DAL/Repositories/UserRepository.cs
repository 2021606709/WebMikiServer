﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ntier.DAL.Context;
using Ntier.DAL.Entities;
using Ntier.DAL.Interfaces;
using Ntier.DTO.DTO;
using Ntier.DTO.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;
        public UserRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User?> AddUserAsync(UserRegisterDTO userDTO)
        {
            string sql = "EXEC dbo.REGISTER_USER @UserId , @Email , @Password , @Name";
            var result = await _context.Users.FromSqlRaw
            (sql,
            new SqlParameter("@UserId", userDTO.Id),
            new SqlParameter("@Email", userDTO.Email),
            new SqlParameter("@Password", userDTO.Password),
            new SqlParameter("@Name", userDTO.Name)
        ).ToListAsync();

            User? user = result.FirstOrDefault();

            return user;
        }

        public async Task<User?> CheckUserAsync(UserLoginDTO userLoginDTO)
        {
            string sql = "EXEC [dbo].[LOGIN_USER] @Email , @Password ;";
            var result = await _context.Users.FromSqlRaw
            (sql,
            new SqlParameter("@Email", userLoginDTO.Email),
            new SqlParameter("@Password", userLoginDTO.Password)
            ).ToListAsync();

            User? user = result.FirstOrDefault();

            return user;
        }

        public async Task UpdateUserInformation ( UserDTO user)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await _context.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RemoveRefreshTokenAsync(string userId)
        {
            try
            {
                var refreshTk = await _context.RefreshTokens.SingleOrDefaultAsync( r => r.Userid == userId );
                _context.Remove(refreshTk);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<RefreshToken?>GetRefreshTokenAsync(string userId)
        {
           var refreshTk = await _context.RefreshTokens.FirstOrDefaultAsync( r => r.Userid == userId );
           return refreshTk;
        }
    }
}

