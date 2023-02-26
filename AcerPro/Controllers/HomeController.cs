using AcerPro.Business.Repository;
using AcerPro.Business.Services.NotificationService;
using AcerPro.Dto.Models;
using AcerPro.Dto.Statics;
using AcerPro.Dto.ViewModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ControllerBase = AcerPro.Controllers.Base.ControllerBase;

namespace AcerPro.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly EmailNotificationService _emailNotificationService;

        private readonly IRepository<Error, ErrorViewModel> _errorRepository;
        private readonly IRepository<Application, ApplicationViewModel> _applicationRepository;
        private readonly IRepository<Notification, NotificationViewModel> _notificationRepository;

        public HomeController(
            HttpClient httpClient,
            EmailNotificationService emailNotificationService,

            IRepository<Error, ErrorViewModel> errorRepository,
            IRepository<Application, ApplicationViewModel> applicationRepository,
            IRepository<Notification, NotificationViewModel> notificationRepository)
        {
            _httpClient = httpClient;
            _emailNotificationService = emailNotificationService;

            _errorRepository = errorRepository;
            _applicationRepository = applicationRepository;
            _notificationRepository = notificationRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Applications = _applicationRepository.Table.Select(q => new ApplicationViewModel
            {
                Id = q.Id,
                Url = q.Url,
                Name = q.Name,
                TraceInterval = q.TraceInterval,
            }).ToList();

            return View();
        }

        public ActionResult AddApplication(ApplicationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isApplicationExists = _applicationRepository.Table.Any(q => q.Url == model.Url);
                    if (isApplicationExists)
                    {
                        ModelState.AddModelError("ApplicationAlreadyExists", "This application is already exits");
                    }
                    else
                    {
                        _applicationRepository.Save(model);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UnexpectedError", "An unexpected error occurred");
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }

            return View(nameof(Index), model);
        }

        public ActionResult ApplicationView(int id)
        {
            var application = _applicationRepository.Take(id);
            return View(application);
        }

        public ActionResult UpdateApplication(ApplicationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isApplicationExists = _applicationRepository.Table.Any(q => q.Url == model.Url && q.Id != model.Id);
                    if (isApplicationExists)
                    {
                        ModelState.AddModelError("ApplicationAlreadyExists", $"An application with url {model.Url} is already exits");
                    }
                    else
                    {
                        _applicationRepository.Save(model);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UnexpectedError", "An unexpected error occurred");
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }

            return View(nameof(ApplicationView), model);
        }

        public ActionResult DeleteApplication(int id)
        {
            try
            {
                _applicationRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> CheckApplication(string url)
        {
            HttpResponseMessage responseMessage = null;
            int applicationId = 0;
            try
            {
                applicationId = _applicationRepository.Table.Where(q => q.Url == url).Select(q => q.Id).FirstOrDefault();

                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                responseMessage = await _httpClient.GetAsync(url);

                responseMessage = responseMessage.EnsureSuccessStatusCode();

                SendEmail($"Application with url {url} is working now", applicationId, NotificationContent.Success);
                AddNotificationToDb(applicationId, NotificationContent.Success);

                return Json(responseMessage.StatusCode);
            }
            catch (HttpRequestException)
            {
                SendEmail($"Application with url {url} is not working", applicationId, NotificationContent.Error);
                AddNotificationToDb(applicationId, NotificationContent.Error);

                return Json("error");
            }
            catch (TaskCanceledException)
            {
                SendEmail($"Request to application with url {url} is timed out", applicationId, NotificationContent.Error);
                AddNotificationToDb(applicationId, NotificationContent.Error);

                return Json("timeout");
            }
            catch (ArgumentNullException ex)
            {
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
                return Json(ex.Message);
            }
        }

        private void SendEmail(string message, int applicationId, NotificationContent nextNotificationContent)
        {
            try
            {
                var lastNotificationContent = _notificationRepository.Table.Where(q => q.UserId == CurrentSession.Id)
                                                                           .Where(q => q.ApplicationId == applicationId)
                                                                           .OrderByDescending(q => q.NotificationDate)
                                                                           .Select(q => q.NotificationContent).FirstOrDefault();

                if (lastNotificationContent == null && nextNotificationContent == NotificationContent.Success) return;

                if (lastNotificationContent == nextNotificationContent) return;

                _emailNotificationService.SendNotification(message);

            }
            catch (Exception ex)
            {
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        private void AddNotificationToDb(int applicationId, NotificationContent nextNotificationContent)
        {
            try
            {
                var lastNotificationContent = _notificationRepository.Table.Where(q => q.UserId == CurrentSession.Id)
                                                                           .Where(q => q.ApplicationId == applicationId)
                                                                           .OrderByDescending(q => q.NotificationDate)
                                                                           .Select(q => q.NotificationContent).FirstOrDefault();

                if (lastNotificationContent == null && nextNotificationContent == NotificationContent.Success) return;

                if (lastNotificationContent == nextNotificationContent) return;

                _notificationRepository.Save(new NotificationViewModel
                {
                    UserId = CurrentSession.Id,
                    ApplicationId = applicationId,
                    NotificationContent = nextNotificationContent
                });
            }
            catch (Exception ex)
            {
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}