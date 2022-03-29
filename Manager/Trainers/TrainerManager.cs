using Microsoft.EntityFrameworkCore;
using nat.Storage.Entity;
using nat.Storage.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Manager.Trainers
{
    public class TrainerManager :ITrainerManager
    {
        private readonly CenterDataContext _dbContext;
        /*
         * DbContext, связанный с моделью, можно использовать для:
         * Создание и выполнение запросов
         * Материализация результатов запросов в виде объектов сущностей
         * Отслеживание изменений, внесенных в эти объекты
         * Сохранить изменения объекта обратно в базе данных
         * Привязка объектов в памяти к элементам управления пользовательского интерфейса
        */
        public TrainerManager(CenterDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        //создаем запись о тренере в бд таблица Trainer
        public async Task<Trainer> AddTrainer(Guid CenterId, CreateOrUpdateTrainerRequest request)
        {
            //создаем запись тренера
            var entity = new Trainer
            {
                Id = Guid.NewGuid(),
                CenterId = CenterId,
                SurName = request.SurName,
                Name = request.Name,
                LastName = request.LastName,
                Specialization = request.Specialization
            };
            //добавляем запись тренера в бд таблица Trainer
            _dbContext.Trainer.Add(entity);
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        //обновляем запись о тренере в бд таблица Trainer
        public async Task<Trainer> UpdateTrainer(Guid id, CreateOrUpdateTrainerRequest request)
        {
            //получаем нужного нам тренера из бд таблица Trainer
            var entity = await _dbContext.Trainer.FirstOrDefaultAsync(g => g.Id == id);
            //обновляем поля записи тренера 
            entity.SurName = request.SurName;
            entity.Name = request.Name;
            entity.LastName = request.LastName;
            entity.Specialization = request.Specialization;
            //сохраняем вненесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<Trainer> DeleteTrainer(Guid id)
        {
            //получаем нужного нам тренера из бд таблица Trainer
            var entity = await _dbContext.Trainer.FirstOrDefaultAsync(g => g.Id == id);
            //удаляем запись тренера из бд таблица Trainer
            _dbContext.Trainer.Remove(entity);
            //сохраняем вненесенные изменения
            await _dbContext.SaveChangesAsync();
            return null;
        }
        public async Task<Trainer> GetById(Guid id)
        {
            //получаем нужного нам тренера из бд таблица Trainer
            var entity =  await _dbContext.Trainer.FirstOrDefaultAsync(g => g.Id == id);
            //получаем запись о центре к которому относится тренер
            entity.Center = await _dbContext.Center.AsNoTracking().FirstOrDefaultAsync(g => g.Id == entity.CenterId);
            return entity;
        }
        public async Task<Trainer> GetAllCustomers(Guid id)
        {
            //получаем нужного нам тренера из бд таблица Trainer
            var entity = await _dbContext.Trainer.FirstOrDefaultAsync(g => g.Id == id);
            //получаем список клиентов данного тренера
            entity.Customers = await _dbContext.Customer.AsNoTracking().Where(g => g.TrainerId == entity.Id).ToListAsync();
            //получаем запись о центре к которому относится тренер
            entity.Center = await _dbContext.Center.AsNoTracking().FirstOrDefaultAsync(g => g.Id == entity.CenterId);
            return entity;
        }
    }
}
