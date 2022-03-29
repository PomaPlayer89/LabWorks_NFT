using nat.Storage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Manager.Centers
{
    public interface ICenterManager
    {
        Task<Center> AddCenter(CreateOrUpdateCenterRequest request);
        Task<Center> UpdateCenter(Guid id, CreateOrUpdateCenterRequest request);
        Task<Center> DeleteCenter(Guid id);
        Task<List<Center>> GetAll();
        Task<Center> GetAllCustomersCenter(Guid id);
        Task<Center> GetAllTrainersCenter(Guid id);
        Task<Center> GetById(Guid id);
        Task<Center> SearchTrainer(Guid id, string name, string text);
    }
}
