using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.ViewModel;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository= dashboardRepository;
        }
        public async  Task<IActionResult> Index()
        {
            var userClub = await _dashboardRepository.GetAllUserClubs();
            var userRace = await _dashboardRepository.GetAllUserRaces();
            var dashboardviewmodel = new DashboardViewModel()
            {
                races = userRace,
                clubs = userClub
            };
            return View(dashboardviewmodel);
        }
    }
}
