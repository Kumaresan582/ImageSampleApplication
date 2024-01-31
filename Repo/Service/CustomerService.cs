using AutoMapper;
using Entity.DatabaseConn;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repo.Interface;
using Repo.MapperModel;

namespace Repo.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ApplicationDbContext context, IMapper mapper,ILogger<CustomerService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse> Create(CustomerMapper data)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                _logger.LogInformation("Create Begins");
                Customer customer = _mapper.Map<CustomerMapper, Customer>(data);
                await _context.customer.AddAsync(customer);
                await _context.SaveChangesAsync();
                response.ResponseCode = 201;
                response.Result = data.CustomerId;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Errormessage = ex.Message;
            }

            return response;
        }

        public async Task<List<CustomerMapper>> getall()
        {
            List<CustomerMapper> _response = new List<CustomerMapper>();
            var data = await _context.customer.ToListAsync();
            if (data != null)
            {
                _response = _mapper.Map<List<Customer>, List<CustomerMapper>>(data);
            }
            return _response;
        }

        public async Task<CustomerMapper> getById(int cusId)
        {
            CustomerMapper customer = new CustomerMapper();
            var data = await _context.customer.FindAsync(cusId);
            if (data != null)
            {
                customer = _mapper.Map<Customer, CustomerMapper>(data);
            }
            return customer;
        }

        public async Task<ApiResponse> Remove(int cusId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _context.customer.FindAsync(cusId);
                if (data != null)
                {
                    _context.customer.Remove(data);
                    await _context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = cusId;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Errormessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Errormessage = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse> Update(CustomerMapper data, int cusId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var customer = await _context.customer.FindAsync(cusId);
                if (customer != null)
                {
                    customer.FirstName = data.FirstName;
                    customer.LastName = data.LastName;
                    customer.PhoneNumber = data.PhoneNumber;
                    customer.Email = data.Email;
                    customer.Birthdate = data.Birthdate;
                    customer.IsActive = data.IsActive;
                    _context.customer.Update(customer);
                    await _context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = cusId;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Errormessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Errormessage = ex.Message;
            }

            return response;
        }
    }
}