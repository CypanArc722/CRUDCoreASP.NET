using System.Diagnostics;
using CRUDCoreASP.NET.Models;
using CRUDCoreASP.NET.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDCoreASP.NET.Controllers
{
    public class HomeController : Controller
    {
        #region Properties
        private readonly ILogger<HomeController> _logger;
        private readonly IUserServices _userServices;
        #endregion

        #region Constructor
        public HomeController(ILogger<HomeController> logger, IUserServices _userServices)
        {
            _logger = logger;
            this._userServices = _userServices;
        }
        #endregion

        #region Methods
        public async Task<IActionResult> Index()
        {
            try
            {
                Status status = new Status();
                status.User = await _userServices.GetAllUsers();
                return View(status);
            }
            catch (Exception ex)
            {
                Status stat = new Status();
                stat.StatusName = ex.Message.ToString();
                return View(stat);
            }
            
        }

        [HttpPost]
        public async Task<Status> AddUser(Users user)
        {
            try
            {
                Status status = new Status();
                status.User = await _userServices.CreateUser(user);
                return status;
            }
            catch (Exception ex)
            {
                Status stat = new Status();
                stat.StatusName = ex.Message.ToString();
                return stat;
            }
        }

        [HttpPost]
        public async Task<Status> EditUser(Users user)
        {
            try
            {
                Status status = new Status();
                status.User = await _userServices.UpdateUser(user);
                return status;
            }
            catch (Exception ex)
            {
                Status stat = new Status();
                stat.StatusName = ex.Message.ToString();
                return stat;
            }
        }

        [HttpPost]
        public async Task<Status> DeleteUser(int id)
        {
            try
            {
                Status status = new Status();
                status.StatusName = await _userServices.DeleteUser(id) == true? "Successfully deleted":"Failed to delete";
                return status;
            }
            catch (Exception ex)
            {
                Status stat = new Status();
                stat.StatusName = ex.Message.ToString();
                return stat;
            }
        }

        public IActionResult ErrorViewModel()
        {
            return View();
        }
        #endregion
    }
}
