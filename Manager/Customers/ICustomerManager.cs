using nat.Storage.Entity;
using System;
using System.Threading.Tasks;

namespace nat.Manager.Customers
{
    public interface ICustomerManager
    {
        Task<Customer> AddCustomer(Guid CenterId, Guid TrainerId, CreateOrUpdateCustomerRequest request);
        Task<Customer> UpdateCustomer(Guid id, CreateOrUpdateCustomerRequest request);
        Task<Customer> DeleteCustomer(Guid id);
        Task<Customer> GetById(Guid id);
    }
}
