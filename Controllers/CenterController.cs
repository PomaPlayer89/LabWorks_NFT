using Microsoft.AspNetCore.Mvc;
using nat.Manager.Centers;
using System;
using System.Threading.Tasks;

namespace nat.Controllers
{
    public class CenterController : Controller
    {
        private readonly ICenterManager _manager;

        public CenterController(ICenterManager manager)
        {
            _manager = manager;
        }
        //представления
        [HttpGet]
        //запускаем начальную страницу
        public ViewResult Index()
        {
            return View();
        }
        //запускаем страницу центра
        public async Task<ViewResult> Center(Guid id)
        {
            //получаем запись центра
            var entity = await _manager.GetById(id);
            return View(entity);
        }
        //запускаем страницу создание центра
        public ViewResult CreateCenter()
        {
            return View();
        }
        //запускаем страницу обновления данных центра
        public async Task<ViewResult> UpdateCenter(Guid id)
        {
            //получаем запись центра
            var entity = await _manager.GetById(id);
            return View(entity);
        }
        //запускаем страницу, где отображаются все центра
        public async Task<ViewResult> AllCenter()
        {
            //получаем все центра
            var entities = await _manager.GetAll();
            return View(entities);
        }
        //запускаем страницу, где отображаются все тренера относящиеся к одному центра
        public async Task<ViewResult> AllTrainertsCenter(Guid id)
        {
            //получаем запись центра, а в ней приходят и записи всех тренеров этого центра
            var entity = await _manager.GetAllTrainersCenter(id);
            return View(entity);
        }
        //запускаем страницу, где отображаются все клиента относящиеся к одному центру
        public async Task<ViewResult> AllCustomersCenter(Guid id)
        {
            //получаем запись центра, в ней приходят записи всех тренеров центра, а также записи всех клиентов данного центра 
            var entity = await _manager.GetAllCustomersCenter(id);
            return View(entity);
        }
        //методы, которые будут вызываться на html странице
        [HttpPost]
        //создание записи центра, метед вызывается с html страницы
        public async Task<ViewResult> Create(CreateOrUpdateCenterRequest request)
        {
            //добавляем запись о центре в бд
            await _manager.AddCenter(request);
            //получаем записи всех центорв
            var entity = await _manager.GetAll();
            //вызываем html страницу всех центров
            return View("AllCenter", entity);
        }
        //обновляем запись центра, метед вызывается с html страницы
        public async Task<ViewResult> Update(Guid id, CreateOrUpdateCenterRequest request)
        {
            //обновляем запись центра в бд
            await _manager.UpdateCenter(id, request);
            //получаем запись всех центров
            var entity = await _manager.GetAll();
            //вызываем html страницу всех центов
            return View("AllCenter", entity);
        }
        //удаляем запись центра из бд, метед вызывается с html страницы
        public async Task<ViewResult> Delete(Guid id)
        {
            //удаляем запись центра из бд
            await _manager.DeleteCenter(id);
            //получаем записи всех центров
            var entities = await _manager.GetAll();
            //вызываем html страницу всех центров
            return View("AllCenter", entities);
        }
        //осуществляем поиск тренеров удовлетворяющим критерию text в таблице Center, метед вызывается с html страницы
        public async Task<ViewResult> SearchTrainer(Guid id, string name, string text)
        {
            //осуществляем поиск нужных нам записей
            var entity = await _manager.SearchTrainer(id, name, text);
            //вызываем html страницу всех тренеров определенного центра
            return View("AllTrainertsCenter", entity);
        }
    }
}