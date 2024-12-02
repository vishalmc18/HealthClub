using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModel;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {

        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;



        //this is for image link stores in the SQL Table
        //public RaceController(IRaceRepository raceRepository)
        //{
        //    _raceRepository = raceRepository;
        //}
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> race = await _raceRepository.GetAll();
            return View(race);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {

            var curuserid = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createviewmodel = new CreateRaceViewModel
            {
                AppUserId = curuserid,
            };
            return View(createviewmodel);
             
            

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceview)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceview.Image);

                var race = new Race
                {
                    Title = raceview.Title,
                    Description = raceview.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceview.AppUserId,
                    Address = new Address
                    {
                        street = raceview.Address.street,
                        city = raceview.Address.city,
                        state = raceview.Address.state
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(raceview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null)
            {
                return View("Error");
            }
            var raceview = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                URL = race.Image,
                AddressId = race.AddressId,
                Address = race.Address,

                RaceCategory = race.RaceCategory,


            };
            return View(raceview);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel editraceview)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fail to Edit Race ");
                return View("Edit", editraceview);
            }
            var edituserrace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (edituserrace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(edituserrace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(editraceview);
                }

                var photoresult = await _photoService.AddPhotoAsync(editraceview.Image);
                var race = new Race
                {
                    Id = id,
                    Title = editraceview.Title,
                    Description = editraceview.Description,
                    Image = photoresult.Url.ToString(),
                    AddressId = editraceview.AddressId,
                    Address = editraceview.Address

                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(editraceview);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var delfound =await _raceRepository.GetByIdAsync(id);
            if (delfound == null) { return View("Error"); }

            return View(delfound);
           
        }

        [HttpPost, ActionName("DeleteRace")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var delrace = await _raceRepository.GetByIdAsync(id);
            if (delrace == null)
            {
                return View("Error");
            }
            _raceRepository.Delete(delrace);
            return RedirectToAction("Index");

        }
            //this is for image link stores in the SQL Table

            //[HttpPost]
            //public async Task<IActionResult> Create(Race race)
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return View(race);
            //    }
            //    _raceRepository.Add(race);

            //    return RedirectToAction("Index");
            //}
            ////this is without using Irepository
            //private readonly ApplicationDbContext _context;

            //public RaceController(ApplicationDbContext context)
            //{
            //    _context= context;
            //}
            //public IActionResult Index()
            //{
            //    List<Race> races = _context.Races.ToList();
            //    return View(races);
            //}

            //public IActionResult Detail(int id)
            //{
            //    Race race= _context.Races.Include(a=>a.Address).FirstOrDefault(r => r.Id==id);
            //    return View(race);
            //}
        }
    }
