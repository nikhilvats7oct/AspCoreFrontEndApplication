using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IDistributedCache DistributedCache;

        protected BaseController(ILogger<BaseController> logger,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            DistributedCache = distributedCache;
            ApplicationSessionState = sessionState;
        }

        protected string LoggedInUserId => GetCaseflowUserId();

        protected IUserIdentity LoggedInUser
        {
            get
            {
                var userId = LoggedInUserId;

                //
                // Logged in - construct object with user properties
                //
                if (!string.IsNullOrEmpty(userId) &&
                    User.Identity.IsAuthenticated)
                {
                    var email = User.FindFirst("email")?.Value;

                    return new UserIdentityContainer
                    {
                        IsLoggedInUser = true,
                        UserId = userId,
                        EmailAddress = email
                    };
                }

                // Anonymous, therefore return null user id (IsLoggedInUser will be false)
                return new UserIdentityContainer
                {
                    UserId = null
                };
            }
        }

        protected IApplicationSessionState ApplicationSessionState { get; }
        protected ILogger<BaseController> Logger { get; }
        protected IConfiguration Configuration { get; }

        protected virtual string GetCaseflowUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var caseflowUerIdClaim = User.FindFirst("caseflow_userid");

            return caseflowUerIdClaim?.Value;
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        protected void AddErrors(string error)
        {
            ModelState.AddModelError("", error);
        }

        protected string SerialiseModel<T>(T model)
        {
            var clearText = JsonConvert.SerializeObject(model);
            var aesKey = Configuration["Encryption:AesKey"];
            var aesIv = Configuration["Encryption:AesInitializationVector"];
            return AesHelper.Encrypt(clearText, aesKey, aesIv);
        }

        protected T DeserialiseModel<T>(string modelString)
        {
            Debug.Assert(!string.IsNullOrEmpty(modelString));
            var aesKey = Configuration["Encryption:AesKey"];
            var aesIv = Configuration["Encryption:AesInitializationVector"];
            modelString = AesHelper.Decrypt(modelString, aesKey, aesIv);
            return JsonConvert.DeserializeObject<T>(modelString);
        }

        protected virtual string GetEmail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var emailClaim = User.FindFirst("email");

            return emailClaim?.Value;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var accessToken = context.HttpContext.GetTokenAsync("access_token").Result;

            ViewData.Add("access_token", accessToken);
        }


        protected virtual async Task<T> GetFromCache<T>(string key)
        {
            var item = await DistributedCache.GetAsync(key);

            if (item == null)
            {
                return default;
            }

            var itemString = Encoding.UTF8.GetString(item);

            if (typeof(T) == typeof(string))
            {
                return (T) Convert.ChangeType(itemString, typeof(T));
            }

            return JsonConvert.DeserializeObject<T>(itemString, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
        }

        protected virtual async Task<T> GetFromCacheForUser<T>(string key)
        {
            return await GetFromCache<T>($"{GetSubjectId()}-{key}");
        }

        protected virtual async Task ClearCache(string key)
        {
            await DistributedCache.RemoveAsync(key);
        }

        protected virtual async Task<string> CacheWithDefaultTtl<T>(T item, string key = null)
        {
            return await CacheFor(item, TimeSpan.FromMinutes(20), key);
        }

        protected virtual async Task<string> CacheFor<T>(T item, TimeSpan timeSpan, string key = null)
        {
            key = key ?? Guid.NewGuid().ToString("N");
            var serialised = JsonConvert.SerializeObject(item);
            var serialisedBytes = Encoding.UTF8.GetBytes(serialised);

            await DistributedCache.SetAsync(key, serialisedBytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.Add(timeSpan)
            });

            return key;
        }

        protected virtual async Task CacheItemForUser(ICacheableViewModel vm, TimeSpan? timeSpan = null)
        {
            vm.Key = vm.Key ?? Guid.NewGuid().ToString("N");

            await CacheFor(vm, timeSpan ?? TimeSpan.FromMinutes(20), $"{GetSubjectId()}-{vm.Key}");
        }

        protected virtual async Task CacheItem(ICacheableViewModel vm, TimeSpan? timeSpan = null)
        {
            vm.Key = vm.Key ?? Guid.NewGuid().ToString("N");

            await CacheFor(vm, timeSpan ?? TimeSpan.FromMinutes(20), $"{vm.Key}");
        }

        protected virtual async Task CacheForUser(ICacheableViewModel vm, TimeSpan? timeSpan = null)
        {
            vm.Key = vm.Key ?? Guid.NewGuid().ToString("N");

            var serialised = JsonConvert.SerializeObject(vm);
            var serialisedBytes = Encoding.UTF8.GetBytes(serialised);

            var span = timeSpan ?? TimeSpan.FromMinutes(20);

            await DistributedCache.SetAsync($"{GetSubjectId()}-{vm.Key}", serialisedBytes,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(span)
                });
        }

        protected virtual string GetSubjectId()
        {
            return !User.Identity.IsAuthenticated ? null : User.FindFirst("sub").Value;
        }

        private class UserIdentityContainer : IUserIdentity
        {
            public bool IsLoggedInUser { get; set; }
            public string UserId { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}