using Microsoft.AspNetCore.Mvc;
using nat.Manager;
using nat.Manager.Customers;
using System;
using System.Threading.Tasks;

namespace nat.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManager _manager;

        public CustomerController(ICustomerManager manager)
        { 
            _manager = manager;
        }
        //представления
        [HttpGet]
        //запускаем страницу создания клиента
        public ActionResult CreateCustomer(Guid CenterId, Guid TrainerId)
        {
            //так как в представление можно передать только один параметр групперуем нужные нам параметры в один класс и отправляем в представление объект данного класса
            var id = new Help { CenterId = CenterId, TrainerId = TrainerId };
            return View(id);
        }
        //запускаем страницу обновленя клиента
        public async Task<ActionResult> UpdateCustomer(Guid id, Guid CenterId)
        {
            //находим нужного клиента в бд
            var entity = await _manager.GetById(id);
            //групперуем нужные нам данные и отправляем объект даннух в представление
            var p = new Help { CenterId = CenterId, Customer = entity };
            return View(p);
        }
        //методы, которые будут вызываться на html странице
        [HttpPost]
        //создание записи клиента в бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Create(Guid CenterId, Guid TrainerId, CreateOrUpdateCustomerRequest request)
        {
            //добавляем запись клента в бд
            await _manager.AddCustomer(CenterId, TrainerId, request);
            //осуществляем переход на метод AllCustomersCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllCustomersCenter", "Center", new { id = CenterId});
        }
        //обновление записи клиента в бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Update(Guid id, Guid CenterId, CreateOrUpdateCustomerRequest request)
        {
            //обновление записи клиента в бд
            await _manager.UpdateCustomer(id, request);
            //осуществляем переход на метод AllCustomersCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllCustomersCenter", "Center", new { id = CenterId });
        }
        //удаление записи клиента из бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Delete(Guid id, Guid CenterId)
        {
            //удаление записи клиента из бд
            await _manager.DeleteCustomer(id);
            //осуществляем переход на метод AllCustomersCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllCustomersCenter", "Center", new { id = CenterId });
        }
    }
}