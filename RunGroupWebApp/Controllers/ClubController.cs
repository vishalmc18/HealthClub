using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModel;
using System.Runtime.CompilerServices;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //this stores images with link
        //public ClubController(IClubRepository clubRepository)
        //{
        //    _clubRepository = clubRepository;
        //}
        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);

        }

      


        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if(clubDetails==null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost,ActionName("DeleteClub")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubdelete = await _clubRepository.GetByIdAsync(id);
            if(clubdelete == null) return View("Error");
            _clubRepository.Delete(clubdelete);
            return RedirectToAction("Index");
        }

        //this stores image with link 
        //[HttpPost]
        //public async Task<IActionResult> Create(Club club)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View(club);
        //    }
        //    _clubRepository.Add(club);

        //    return RedirectToAction("Index");
        //}

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId,
            };
            return View(createClubViewModel);
        }
        //this store images from cloud not in SQL Table
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubview)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubview.Image);

                var club = new Club
                {
                    Title = clubview.Title,
                    Description = clubview.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubview.AppUserId,
                    Address = new Address
                    {
                        street = clubview.Address.street,
                        city = clubview.Address.city,
                        state = clubview.Address.state
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(clubview);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }
            var clubview = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                //Image = club.Image,
                AdressId = club.AdressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };
            return View(clubview);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel editclubview)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", editclubview);
            }
            var edituserclub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (edituserclub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(edituserclub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "failed to delete previous photo");
                    return View(editclubview);
                }

                var photoresult = await _photoService.AddPhotoAsync(editclubview.Image);
                var club = new Club
                {
                    Id = id,
                    Title = editclubview.Title,
                    Description = editclubview.Description,
                    Image = photoresult.Url.ToString(),
                    AdressId = editclubview.AdressId,
                    Address = editclubview.Address
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(editclubview);
            }
        }


        ////This is without using Irepository
        //private readonly ApplicationDbContext _context;
        //public ClubController(ApplicationDbContext context)
        //{
        //    _context  = context;
        //}
        //public IActionResult Index()
        //{   
        //    List<Club> clubs = _context.Clubs.ToList();
        //    return View(clubs);
        //}

        //public IActionResult Detail(int id)
        //{
        //    Club club = _context.Clubs.Include(a=>a.Address).FirstOrDefault(c => c.Id == id);
        //    return View(club);
        //}
    }
}
