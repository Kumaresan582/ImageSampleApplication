using Entity.Model;
using Repo.MapperModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Interface
{
    public interface ICustomerService
    {
        Task<List<CustomerMapper>> getall();
        Task<CustomerMapper> getById(int cusId); 
        Task<ApiResponse> Remove(int cusId);
        Task<ApiResponse> Create(CustomerMapper data);

        Task<ApiResponse> Update(CustomerMapper data, int cusId);
    }
}
