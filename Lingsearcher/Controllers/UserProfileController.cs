using Lingseacher.Entity;
using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using log4net;

namespace Lingsearcher.Controllers
{
    public class UserProfileController : Controller
    {
        private UserManager<UserApplication> _userManager;
        private static readonly ILog _logger = LogManager.GetLogger(Environment.MachineName);

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

        public IAuthenticationManager _authenticationManager
        {
            get
            {
                var contextoOwin = Request.GetOwinContext();
                return contextoOwin.Authentication;
                
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UpdateUserProfile()
        {
            UpdateUserProfileViewModel model = null;
            try
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                model = new UpdateUserProfileViewModel
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    UserName = user.UserName,
                };


                UserSystemDao userSystemDao = new UserSystemDao();
                UserSystem userSystem = new UserSystem();
                userSystem.UserApplication = user;
                userSystem = userSystemDao.GetByUserApplicationId(user.Id);

                if (userSystem != null)
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
                        model.AddressId = userSystem.AddressId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdateUserProfile(UpdateUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());

                    var newAddress = new Address
                    {
                        Id = model.AddressId,
                        Street = model.Street,
                        State = model.State,
                        PostalCode = model.PostalCode,
                        Country = model.Country,
                        City = model.City,
                        Neighbourhood = model.Neighbourhood
                    };

                    if(newAddress.Id == 0 )
                    {
                        newAddress = new BaseDAO<Address>().Insert(newAddress);
                    }
                    else
                    {
                        new BaseDAO<Address>().Update(newAddress);
                    }
                    

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
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error");
                }
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
                try
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
                        ViewBag.MessageUpdatePassword = "Senha alterada com sucesso!!!";
                        //return RedirectToAction("Index", "Home");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DisableUser()
        {
            if (ModelState.IsValid)
            {
                try
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
                        //_authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        //return RedirectToAction("Index", "Home");
                        ViewBag.MessageDisableUser = "Usuario desativado com sucesso!!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error");
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