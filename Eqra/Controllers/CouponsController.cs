using Eqra.Data;
using Eqra.Models;
using Eqra.Services;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class CouponsController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly CodeGeneratorService _codeGenerator;
        public CouponsController(UserManager<User> userManager, ApplicationDbContext context, CodeGeneratorService codeGenerator)
        {
            _userManager = userManager;
            _context = context;
            _codeGenerator = codeGenerator;
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Coupon model)
        {

                model.Code = _codeGenerator.RandomString(6);
                _context.Coupons.Add(model);
                _context.SaveChanges();


            return RedirectToAction("Index", "Shop", _context.Coupons.ToList());
        }

        
        public IActionResult Delete(Guid id)
        {

            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == id);
            _context.Coupons.Remove(coupon);
            _context.SaveChanges();

            return RedirectToAction("Index", "Shop", _context.Coupons.ToList());
        }

        public async Task<JsonResult> UseCoupon([FromBody] CheckPointsViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == model.Id);


            userLogged.Points -= coupon.Cost;
            await _userManager.UpdateAsync(userLogged);

            coupon.Used = true;
            _context.Coupons.Update(coupon);
            _context.SaveChanges();

            return Json(new {code = coupon.Code});
        }

        [HttpPost]
        public async Task<JsonResult> CheckPoints([FromBody] CheckPointsViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var coupon = _context.Coupons.Where(o => o.Id == model.Id).FirstOrDefault();


            if(coupon.Cost > userLogged.Points)
            {
                return Json(new { correct = false });
            }
            else
            {
                return Json(new { correct = true });
            }

        }




    }
}
