using Microsoft.AspNetCore.Mvc;
using nat.Manager;
using nat.Manager.Trainers;
using System;
using System.Threading.Tasks;

namespace nat.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerManager _manager;

        public TrainerController(ITrainerManager manager)
        {
            _manager = manager;
        }
        //представления
        //запускаем html страницу создания тренера
        public ActionResult CreateTrainer(Guid CenterId)
        {
            //групперуем нужные нам элементы
            var id = new Help { CenterId = CenterId };
            return View(id);
        }
        //запускаем htnl страницу обновление записи тренера
        public async Task<ActionResult> UpdateTrainer(Guid id)
        {
            //запрашиваем нужного нам пренера в бд
            var entity = await _manager.GetById(id);
            return View(entity);
        }
        //методы, которые будут вызываться на html странице
        //создаем запись тренера в бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Create(Guid CenterId, CreateOrUpdateTrainerRequest request)
        {
            //добавляем тренера в бд
            await _manager.AddTrainer(CenterId, request);
            //осуществляем переход на метод AllTrainertsCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllTrainertsCenter", "Center", new { id = CenterId });
        }
        //обновляем запись тренера в бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Update(Guid id, Guid CenterId, CreateOrUpdateTrainerRequest request)
        {
            //обновляем запись тренера в бд
            await _manager.UpdateTrainer(id, request);
            //осуществляем переход на метод AllTrainertsCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllTrainertsCenter", "Center", new { id = CenterId });
        }
        //удаляем запись тренера из бд, метед вызывается с html страницы
        public async Task<RedirectToActionResult> Delete(Guid id, Guid CenterId)
        {
            //кудаляем запись тренера из бд
            await _manager.DeleteTrainer(id);
            //осуществляем переход на метод AllTrainertsCenter контроллера Center и передаем ему нужыые параметры для данного метода
            return RedirectToAction("AllTrainertsCenter", "Center", new { id = CenterId });
        }
    }
}