using nat.Storage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Manager.Trainers
{
    public interface ITrainerManager
    {
        Task<Trainer> AddTrainer(Guid CenterId, CreateOrUpdateTrainerRequest request);
        Task<Trainer> UpdateTrainer(Guid id, CreateOrUpdateTrainerRequest request);
        Task<Trainer> DeleteTrainer(Guid id);
        Task<Trainer> GetById(Guid id);
        Task<Trainer> GetAllCustomers(Guid id);
    }
}
