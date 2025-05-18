using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return BaseResponse<UserDto>.FailResult($"User with ID {id} not found.");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return BaseResponse<UserDto>.SuccessResult(userDto);
    }

    public async Task<BaseResponse<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return BaseResponse<IEnumerable<UserDto>>.SuccessResult(userDtos);
    }

    public async Task<BaseResponse<UserDto>> CreateAsync(CreateUserDto createDto, Guid userId)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(createDto.Email);
            if (existingUser != null)
            {
                return BaseResponse<UserDto>.FailResult($"User with email {createDto.Email} already exists.");
            }

            // Map and add the user
            var user = _mapper.Map<User>(createDto);
            await _unitOfWork.Users.AddAsync(user);

            // Add user to departments if specified
            if (createDto.DepartmentIds.Any())
            {
                foreach (var departmentId in createDto.DepartmentIds)
                {
                    var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                    if (department != null)
                    {
                        var userDepartment = new UserDepartment
                        {
                            UserId = user.Id,
                            DepartmentId = departmentId
                        };
                        user.UserDepartments.Add(userDepartment);
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
            
            // Get the full user with departments
            var createdUser = await _unitOfWork.Users.GetUserWithDepartmentsAsync(user.Id);
            var userDto = _mapper.Map<UserDto>(createdUser);
            
            return BaseResponse<UserDto>.SuccessResult(userDto, "User created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UserDto>.FailResult($"Error creating user: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UserDto>> UpdateAsync(Guid id, UpdateUserDto updateDto, Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return BaseResponse<UserDto>.FailResult($"User with ID {id} not found.");
        }

        try
        {
            // Check email uniqueness if changing email
            if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != user.Email)
            {
                var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(updateDto.Email);
                if (existingUser != null)
                {
                    return BaseResponse<UserDto>.FailResult($"User with email {updateDto.Email} already exists.");
                }
            }

            // Update user properties
            _mapper.Map(updateDto, user);
            await _unitOfWork.Users.UpdateAsync(user);

            // Update department relationships if provided
            if (updateDto.DepartmentIds != null)
            {
                // Get current user departments
                var userWithDepts = await _unitOfWork.Users.GetUserWithDepartmentsAsync(id);
                var currentDeptIds = userWithDepts.UserDepartments.Select(ud => ud.DepartmentId).ToList();
                
                // Departments to remove
                var deptIdsToRemove = currentDeptIds.Except(updateDto.DepartmentIds).ToList();
                foreach (var deptId in deptIdsToRemove)
                {
                    var userDept = userWithDepts.UserDepartments.FirstOrDefault(ud => ud.DepartmentId == deptId);
                    if (userDept != null)
                    {
                        user.UserDepartments.Remove(userDept);
                    }
                }
                
                // Departments to add
                var deptIdsToAdd = updateDto.DepartmentIds.Except(currentDeptIds).ToList();
                foreach (var deptId in deptIdsToAdd)
                {
                    var dept = await _unitOfWork.Departments.GetByIdAsync(deptId);
                    if (dept != null)
                    {
                        user.UserDepartments.Add(new UserDepartment
                        {
                            UserId = id,
                            DepartmentId = deptId
                        });
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
            
            // Get updated user with departments
            var updatedUser = await _unitOfWork.Users.GetUserWithDepartmentsAsync(id);
            var userDto = _mapper.Map<UserDto>(updatedUser);
            
            return BaseResponse<UserDto>.SuccessResult(userDto, "User updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UserDto>.FailResult($"Error updating user: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return BaseResponse<bool>.FailResult($"User with ID {id} not found.");
        }

        try
        {
            await _unitOfWork.Users.RemoveAsync(user);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "User deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UserDto>> GetUserWithDepartmentsAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetUserWithDepartmentsAsync(id);
        if (user == null)
        {
            return BaseResponse<UserDto>.FailResult($"User with ID {id} not found.");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return BaseResponse<UserDto>.SuccessResult(userDto);
    }

    public async Task<BaseResponse<IEnumerable<UserDto>>> GetUsersByDepartmentAsync(Guid departmentId)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
        if (department == null)
        {
            return BaseResponse<IEnumerable<UserDto>>.FailResult($"Department with ID {departmentId} not found.");
        }
        
        var users = await _unitOfWork.Users.GetUsersByDepartmentAsync(departmentId);
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        
        return BaseResponse<IEnumerable<UserDto>>.SuccessResult(userDtos);
    }

    public async Task<BaseResponse<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
        if (user == null)
        {
            return BaseResponse<UserDto>.FailResult($"User with email {email} not found.");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return BaseResponse<UserDto>.SuccessResult(userDto);
    }
}