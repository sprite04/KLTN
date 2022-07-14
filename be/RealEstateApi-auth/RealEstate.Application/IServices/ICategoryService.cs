using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> GetCategories();
    }
}
