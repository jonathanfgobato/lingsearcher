using Lingseacher.Entity;
using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace Lingsearcher.Controllers
{
    public class UserProfileController : Controller
    {
        private UserManager<UserApplication> _userManager;

        public UserManager<UserApplication> UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    var contextOwin = HttpContext.GetOwinContext();
                    _userManager = contextOwin.GetUserManager<UserManager<UserApplication>>();
                }
                return _userManager;
            }
            set
            {
                _userManager = value;
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UpdateUserProfile()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            UpdateUserProfileViewModel model = new UpdateUserProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName,
            };


            UserSystemDao userSystemDao = new UserSystemDao();
            UserSystem userSystem = new UserSystem();
            userSystem.UserApplication = user;
            userSystem = userSystemDao.GetByUserApplicationId(user.Id);

            if(userSystem != null)
            {
                BaseDAO<Address> baseDAO = new BaseDAO<Address>();
                Address address = baseDAO.GetById(userSystem.AddressId);

                if (address != null)
                {
                    model.City = address.City;
                    model.Country = address.Country;
                    model.Neighbourhood = address.Neighbourhood;
                    model.State = address.State;
                    model.Street = address.Street;
                    model.PostalCode = address.PostalCode;
                }
            }



            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdateUserProfile(UpdateUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                var newAddress = new Address
                {
                    Street = model.Street,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    City = model.City,
                    Neighbourhood = model.Neighbourhood
                };

                newAddress = new BaseDAO<Address>().Insert(newAddress);

                var userApplication = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var newUserSystem = new UserSystem
                {
                    UserApplication = userApplication,
                    Address = newAddress
                };

                userApplication.FullName = model.FullName;
                userApplication.UserName = model.UserName;

                // Alteração das informações de Login do usuário

                var result = UserManager.Update(userApplication);
                if (!result.Succeeded)
                {
                    // Retornar para uma página para confirmar
                    return View(model);
                }
                else
                    AddErrors(result);

                BaseDAO<UserSystem> baseDAOUserSystem = new BaseDAO<UserSystem>();
                BaseDAO<Address> baseDaoAddress = new BaseDAO<Address>();

                newUserSystem.FgActive = 1;

                // Verifica se o usuário já tem as informações adicionais cadastradas

                UserSystem userSystem = new UserSystemDao().GetByUserApplicationId(user.Id);
                if (userSystem == null)
                {
                    baseDAOUserSystem.Insert(newUserSystem);
                    return View(model);
                }
                newUserSystem.Id = userSystem.Id;

                // Caso o usuário tenha a informação é atualizado com o novo valor
                //baseDaoAddress.Update(newAddress);
                
                baseDAOUserSystem.Update(newUserSystem);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.NewPasswordConfirmation)
                {
                    ModelState.AddModelError("", "Confirmação de senha não coincide");
                    return View(model);
                }

                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);


                if (!result.Succeeded)
                {
                    AddErrors(result);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DisableUser()
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.SetLockoutEndDateAsync(User.Identity.GetUserId(), DateTime.Today.AddYears(10));
                BaseDAO<UserSystem> baseDAO = new BaseDAO<UserSystem>();

                UserSystem userSystem = new UserSystemDao().GetByUserApplicationId(User.Identity.GetUserId());

                userSystem.FgActive = 0;

                baseDAO.Update(userSystem);

                if (!result.Succeeded)
                {
                    AddErrors(result);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


        private void AddErrors(IdentityResult result)
        {
            // Retorna todos os erros através da model state
            foreach (var erro in result.Errors)
                ModelState.AddModelError("", erro);
        }

    }
}