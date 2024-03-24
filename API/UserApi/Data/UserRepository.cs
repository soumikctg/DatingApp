﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username);
    }

    public async Task AddUserAsync(AppUser user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users.Include(p => p.Photos).ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUserNameAsync(string username)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }



    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await _context.Users.Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = _context.Users.AsQueryable();

        query = query.Where(u => u.UserName != userParams.CurrentUserName);
        query = query.Where(u => u.Gender == userParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive)
        };
        var members = await query.ToListAsync();

        return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
            userParams.PageNumber, userParams.PageSize);
    }
}