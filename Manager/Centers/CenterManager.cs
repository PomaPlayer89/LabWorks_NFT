using Microsoft.EntityFrameworkCore;
using nat.Storage.Entity;
using nat.Storage.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nat.Manager.Centers
{
    public class CenterManager : ICenterManager
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

        public CenterManager(CenterDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        //получаем сущность центра
        public async Task<Center> GetById(Guid id)
        {
            //создаем запрос к базе данных к таблицe Center, возвращаем первую найденную удовлетворяюший ограничению сущность (если нет совпадений вернет null)
            return await _dbContext.Center.FirstOrDefaultAsync(g => g.Id == id);
        }
        //добавляем сущность в бд таблицу Center
        public async Task<Center> AddCenter(CreateOrUpdateCenterRequest request)
        {
            //создаем сущность
            var entity = new Center
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                AdressCity = request.AdressCity,
                AdressStreet = request.AdressStreet,
                AdressNumberHouse = request.AdressNumberHouse
            };
            //добавляем сущность в таблицу Center
            _dbContext.Center.Add(entity);
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        //обновляем запись в бд таблице Center
        public async Task<Center> UpdateCenter(Guid id, CreateOrUpdateCenterRequest request)
        {
            //получаем сущность из бд таблицы Center
            var entity = await _dbContext.Center.FirstOrDefaultAsync(g => g.Id == id);
            //обновляем значенияе полей сущности
            entity.Name = request.Name;
            entity.AdressCity = request.AdressCity;
            entity.AdressStreet = request.AdressStreet;
            entity.AdressNumberHouse = request.AdressNumberHouse;
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        //удаляем сущность из бд таблица Center
        public async Task<Center> DeleteCenter(Guid id)
        {
            //получаем сущность из бд таблицы Center
            var entity = await _dbContext.Center.FirstOrDefaultAsync(g => g.Id == id);
            //производим удаление из бд таблицы Center
            _dbContext.Center.Remove(entity);
            //сохраняем внесенные изменения
            await _dbContext.SaveChangesAsync();
            return null;
        }
        //получаем все сущности таблицы Center
        public async Task<List<Center>> GetAll()
        {
            //делаем запрос к бд, AsNoTracking говорит, что полученные данные не надо помещать в кеш, то есть нам не нужно отслеживать изменения в полученных данных и преобразуем полученные данные в список
            return await _dbContext.Center.AsNoTracking().ToListAsync();
        }
        //получаем всех клиентов центра
        public async Task<Center> GetAllCustomersCenter(Guid id)
        {
            //находим нужным нам центр
            var entity = await _dbContext.Center.FirstOrDefaultAsync(g => g.Id == id);
            //получаем записи всех тренеров, относящихся к нужному центру
            entity.Trainers = await _dbContext.Trainer.AsNoTracking().Where(g => g.CenterId == id).ToListAsync();
            List<Customer> list = new List<Customer>();
            entity.Customers = new List<Customer>();
            //организуем цикл, чтоб получить записи всех клиентов данного центра
            for (int i = 0; i < entity.Trainers.Count; i++)
            {
                //получаем всех клиентов тренера
                list = await _dbContext.Customer.AsNoTracking().Where(g => g.TrainerId == entity.Trainers[i].Id).ToListAsync();
                //добавляем их в список клиентов
                entity.Customers = await Task.Run(() => AddListCustomer(entity.Customers, list));
            }
            return entity;
        }
        //получаем всех тренеров центра
        public async Task<Center> GetAllTrainersCenter(Guid id)
        {
            //получаем запись о данном центре
            var entity = await _dbContext.Center.FirstOrDefaultAsync(g => g.Id == id);
            //получаем всех тренеров данного центра
            entity.Trainers = await _dbContext.Trainer.AsNoTracking().Where(g => g.CenterId == id).ToListAsync();
            return entity;
        }
        //метод, который объединяет два списка, причем повторяющиеся сущности добавдляет лишь один раз
        public List<Customer> AddListCustomer(List<Customer> List1, List<Customer> List2)
        {
            //проверяем первый список на пустоту
            if (List1.Count != 0)
            {
                //проверяем второй список на пустоту
                if (List2.Count != 0)
                {
                    //организуем цмкл, нам надо пройтись по всем записям второго списка
                    foreach (var entity in List2)
                    {
                        //проверяем наличие записи второго списка в первом, если записи нет, то в record = null
                        var record = List1.FirstOrDefault(g => g.Id == entity.Id);
                        if (record == null)
                        {
                            //добавляем запись в первый список
                            List1.Add(entity);
                        }
                    }
                }
                return List1;
            }
            else if (List2.Count != 0)
            {
                return List2;
            }
            return new List<Customer>();
        }
        //поиск в бд таблицы Center
        public async Task<Center> SearchTrainer(Guid id, string name, string text)
        {
            //получаем запись центра
            var entity = await _dbContext.Center.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            //удаляем вначале и в конце все пробелы в строке
            text = text.Trim(' ');
            //заменяем все повторяющиеся пробелы одним пробелом
            text = Regex.Replace(text, @"\s+", " ");
            //переводим в нижний регистр указательное слово (как осуществлять поиск: по всем столбцам или по конкретному столбцу)
            name = name.ToLower();
            List<Trainer> list = new List<Trainer>();
            //получаем список тренеров относящихся к нашему центру
            entity.Trainers = await _dbContext.Trainer.AsNoTracking().Where(g => g.CenterId == id).ToListAsync();
            switch (name)
            {
                case "all":
                    //осуществляем поиск по всем столбцам таблицы center в бд
                    list = entity.Trainers.Where(g => g.Specialization.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                                                                                         (g.SurName + " " + g.Name + " " +g.LastName).Contains(text, StringComparison.OrdinalIgnoreCase))
                                                                                         .ToList();
                    //если ничего найти не удалось, пытаемся разбить строку text на несколько слов и осуществить поиск заново.
                    if(list.Count == 0)
                    {
                        //Разбиваем сроку text на подстроки с помощью метода Split и осуществляем цикл по полученному массиву
                        foreach(var word in text.Split(' '))
                        {
                            //получаем список тренеров удовлетворяющих критерию, а именно строке word
                            //метод Contain - Возвращает значение, указывающее, встречается ли указанная строка внутри этой строки, используя указанные правила сравнения.
                            //StringComparison.OrdinalIgnoreCase - Сравнивать строки, используя правила обычной (двоичной) сортировки без учета регистра сравниваемых строк.
                            list = entity.Trainers.Where(g => g.Specialization.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                                                                                         (g.SurName + " " + g.Name + " " + g.LastName).Contains(word, StringComparison.OrdinalIgnoreCase))
                                                                                         .ToList();
                            //объединяем списки
                            entity.Trainers = await Task.Run(() => AddListTrainer(entity.Trainers, list));
                        }
                    }
                    else
                    {
                        entity.Trainers = list;
                    }
                    break;
                case "specialization":
                    //осуществляем поиск в столбце specialization
                    entity.Trainers = entity.Trainers.Where(g => g.Specialization.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "name":
                    //осуществляем поиск в столбцах SurName Name LastName 
                    entity.Trainers = entity.Trainers.Where(g => (g.SurName + " " + g.Name + " " + g.LastName).Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
            }
            return entity;
        }
        //метод, который объединяет два списка, причем повторяющиеся сущности добавдляет лишь один раз
        public List<Trainer> AddListTrainer(List<Trainer> List1, List<Trainer> List2)
        {
            //проверяем первый список на пустоту
            if (List1.Count != 0)
            {
                //проверяем второй список на пустоту
                if (List2.Count != 0)
                {
                    //организуем цмкл, нам надо пройтись по всем записям второго списка
                    foreach (var entity in List2)
                    {
                        //проверяем наличие записи второго списка в первом, если записи нет, то в record = null
                        var record = List1.FirstOrDefault(g => g.Id == entity.Id);
                        if (record == null)
                        {
                            //добавляем запись в первый список
                            List1.Add(entity);
                        }
                    }
                }
                return List1;
            }
            else if (List2.Count != 0)
            {
                return List2;
            }
            return new List<Trainer>();
        }
    }
}
