using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Diagnostics;
using TransactionApp.Models;
using TransactionApp.ViewModels;

namespace TransactionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification)
        {
            _logger = logger;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            return View();
        }
        private static Task<GenericResponse<FeeClass>> ValidateJsonFile(IFormFile formFile)
        {
            var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot",
                    formFile.FileName);


            if (!Path.GetExtension(formFile.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                return Task.FromResult(new GenericResponse<FeeClass>
                {
                    Success = false,
                    Message = "Error: File not supported",
                    Data = null,
                });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }


            string? fileContent = null;
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            var result = JsonConvert.DeserializeObject<FeeClass>(fileContent);
            if (result != null)
            {
                return Task.FromResult(new GenericResponse<FeeClass>
                {
                    Success = true,
                    Message = null,
                    Data = result,
                });
            }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return Task.FromResult(new GenericResponse<FeeClass>
            {
                Success = false,
                Message = "No data",
                Data = null,
            });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        public class GenericResponse<T>
        {
            public bool Success { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string? Message { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public T Data { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> TransactionCharge(TransactionVM transactionVM)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                if (transactionVM.Document == null || transactionVM.Document.Length == 0)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Error: No file selected",
                    });
                }

                var res = await ValidateJsonFile(transactionVM.Document);
                if (res.Success)
                {
                    foreach (var item in res.Data.Fees)
                    {
                        if (transactionVM.Amount >= item.minAmount && transactionVM.Amount <= item.maxAmount)
                        {
                            // return item.feeAmount;
                            return Json(new
                            {
                                success = true,
                                msg = "No Data",
                                charge = item.feeAmount
                            });
                        }
                    }
                    return Json(new
                    {
                        success = false,
                        msg = "Amount does to fall within configuration range",

                    });
                }
                return Json(new
                {
                    success = res.Success,
                    msg = res.Message,

                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    msg = "An error occured",
                    error = e.Message,
                });
            }
        }

        public async Task<IActionResult> TransactionSurcharge(TransactionVM transactionVM)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                if (transactionVM.Document == null || transactionVM.Document.Length == 0)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Error: No file selected",
                    });
                }

                var res = await ValidateJsonFile(transactionVM.Document);
                if (res.Success)
                {
                    foreach (var item in res.Data.Fees)
                    {
                        if (transactionVM.Amount >= item.minAmount && transactionVM.Amount <= item.maxAmount)
                        {
                            var transferAmount = transactionVM.Amount - item.feeAmount;
                            if(transferAmount <= 0)
                            {
                                return Json(new
                                {
                                    success = false,
                                    msg = "Insufficient funds",

                                });
                            }
                            // return item.feeAmount;
                            return Json(new
                            {
                                success = true,
                                msg = "No Data",
                                charge = item.feeAmount,
                                transferAmount 
                            });
                        }
                    }
                    return Json(new
                    {
                        success = false,
                        msg = "Amount does to fall within configuration range",

                    });
                }
                return Json(new
                {
                    success = res.Success,
                    msg = res.Message,

                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    msg = "An error occured",
                    error = e.Message,
                });
            }
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}