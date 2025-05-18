using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<DepartmentDto>> GetByIdAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Department with ID {id} not found.");
        }

        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return BaseResponse<DepartmentDto>.SuccessResult(departmentDto);
    }

    public async Task<BaseResponse<IEnumerable<DepartmentDto>>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return BaseResponse<IEnumerable<DepartmentDto>>.SuccessResult(departmentDtos);
    }

    public async Task<BaseResponse<DepartmentDto>> CreateAsync(CreateDepartmentDto createDto, Guid userId)
    {
        try
        {
            // Validate head user exists
            var head = await _unitOfWork.Users.GetByIdAsync(createDto.HeadId);
            if (head == null)
            {
                return BaseResponse<DepartmentDto>.FailResult($"Head user with ID {createDto.HeadId} not found.");
            }

            // Validate head department if provided
            if (createDto.HeadDepartmentId.HasValue)
            {
                var headDepartment = await _unitOfWork.Departments.GetByIdAsync(createDto.HeadDepartmentId.Value);
                if (headDepartment == null)
                {
                    return BaseResponse<DepartmentDto>.FailResult($"Head department with ID {createDto.HeadDepartmentId} not found.");
                }
            }

            // Create department
            var department = _mapper.Map<Department>(createDto);
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.CompleteAsync();

            // Get created department with relationships
            var createdDepartment = await _unitOfWork.Departments.GetByIdAsync(department.Id);
            var departmentDto = _mapper.Map<DepartmentDto>(createdDepartment);
            
            return BaseResponse<DepartmentDto>.SuccessResult(departmentDto, "Department created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Error creating department: {ex.Message}");
        }
    }

    public async Task<BaseResponse<DepartmentDto>> UpdateAsync(Guid id, UpdateDepartmentDto updateDto, Guid userId)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Department with ID {id} not found.");
        }

        try
        {
            // Validate head user if provided
            if (updateDto.HeadId.HasValue)
            {
                var head = await _unitOfWork.Users.GetByIdAsync(updateDto.HeadId.Value);
                if (head == null)
                {
                    return BaseResponse<DepartmentDto>.FailResult($"Head user with ID {updateDto.HeadId} not found.");
                }
            }

            // Validate head department if provided
            if (updateDto.HeadDepartmentId.HasValue)
            {
                // Check for circular reference
                if (updateDto.HeadDepartmentId.Value == id)
                {
                    return BaseResponse<DepartmentDto>.FailResult("Department cannot be its own head department.");
                }
                
                var headDepartment = await _unitOfWork.Departments.GetByIdAsync(updateDto.HeadDepartmentId.Value);
                if (headDepartment == null)
                {
                    return BaseResponse<DepartmentDto>.FailResult($"Head department with ID {updateDto.HeadDepartmentId} not found.");
                }
                
                // Check if this would create a circular reference in the department hierarchy
                var currentDept = headDepartment;
                while (currentDept != null && currentDept.HeadDepartmentId.HasValue)
                {
                    if (currentDept.HeadDepartmentId.Value == id)
                    {
                        return BaseResponse<DepartmentDto>.FailResult("Creating circular reference in department hierarchy is not allowed.");
                    }
                    currentDept = await _unitOfWork.Departments.GetByIdAsync(currentDept.HeadDepartmentId.Value);
                }
            }

            // Update department
            _mapper.Map(updateDto, department);
            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.CompleteAsync();

            // Get updated department
            var updatedDepartment = await _unitOfWork.Departments.GetByIdAsync(id);
            var departmentDto = _mapper.Map<DepartmentDto>(updatedDepartment);
            
            return BaseResponse<DepartmentDto>.SuccessResult(departmentDto, "Department updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Error updating department: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            return BaseResponse<bool>.FailResult($"Department with ID {id} not found.");
        }

        try
        {
            // Check for associated entities
            var subDepartments = await _unitOfWork.Departments.GetWithSubDepartmentsAsync(id);
            if (subDepartments?.SubDepartments.Any() == true)
            {
                return BaseResponse<bool>.FailResult("Cannot delete department with sub-departments. Please reassign or delete sub-departments first.");
            }
            
            var departmentWithUsers = await _unitOfWork.Departments.GetWithUsersAsync(id);
            if (departmentWithUsers != null && departmentWithUsers.UserDepartments.Any())
            {
                return BaseResponse<bool>.FailResult("Cannot delete department with assigned users. Please reassign users first.");
            }

            // Delete department
            await _unitOfWork.Departments.RemoveAsync(department);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Department deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting department: {ex.Message}");
        }
    }

    public async Task<BaseResponse<DepartmentDto>> GetWithSubDepartmentsAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetWithSubDepartmentsAsync(id);
        if (department == null)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Department with ID {id} not found.");
        }

        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return BaseResponse<DepartmentDto>.SuccessResult(departmentDto);
    }

    public async Task<BaseResponse<DepartmentDto>> GetWithUsersAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetWithUsersAsync(id);
        if (department == null)
        {
            return BaseResponse<DepartmentDto>.FailResult($"Department with ID {id} not found.");
        }

        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return BaseResponse<DepartmentDto>.SuccessResult(departmentDto);
    }

    public async Task<BaseResponse<IEnumerable<DepartmentDto>>> GetByHeadIdAsync(Guid headId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(headId);
        if (user == null)
        {
            return BaseResponse<IEnumerable<DepartmentDto>>.FailResult($"User with ID {headId} not found.");
        }
        
        var departments = await _unitOfWork.Departments.GetByHeadIdAsync(headId);
        var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        
        return BaseResponse<IEnumerable<DepartmentDto>>.SuccessResult(departmentDtos);
    }
}
