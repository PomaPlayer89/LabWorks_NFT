using Microsoft.EntityFrameworkCore;
using nat.Storage.Entity;
using nat.Storage.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Manager.Customers
{
    public class CustomersManager :ICustomerManager
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
        public CustomersManager(CenterDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        //добавляем запись клиента в бд таблица Customer
        public async Task<Customer> AddCustomer(Guid CenterId, Guid TrainerId, CreateOrUpdateCustomerRequest request)
        {
            //создаем запись клиента
            var entity = new Customer
            {
                Id = Guid.NewGuid(),
                TrainerId = TrainerId, 
                SurName = request.SurName,
                Name = request.Name,
                LastName = request.LastName,
                Birthday = request.MyDay
                
            };
            //добавляем запись клиента в бд таблица Customer
            _dbContext.Customer.Add(entity);
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        //обновляем запись клиента в бд
        public async Task<Customer> UpdateCustomer(Guid id, CreateOrUpdateCustomerRequest request)
        {
            //запрашиваем определенного клиента в бд таблица Customer
            var entity = await _dbContext.Customer.FirstOrDefaultAsync(g => g.Id == id);
            //обновляем записи
            entity.SurName = request.SurName;
            entity.Name = request.Name;
            entity.LastName = request.LastName;
            entity.Birthday = request.MyDay;
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        //удаляем запись клиента из бд таблица Customer
        public async Task<Customer> DeleteCustomer(Guid id)
        {
            //получаем нужную запись из бд таблица Customer
            var entity = await _dbContext.Customer.FirstOrDefaultAsync(g => g.Id == id);
            //удаляем запись из таблицы Customer
            _dbContext.Customer.Remove(entity);
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return null;
        }
        //получаем запись клиента из бд таблица Customer
        public async Task<Customer> GetById(Guid id)
        {
            //получаем запись клиента из бд таблица Customer
            return await _dbContext.Customer.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
