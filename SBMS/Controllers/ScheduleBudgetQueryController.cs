﻿using SBMS.Models;
using System.Linq;
using System.Web.Mvc;

namespace SBMS.Controllers
{
    public class ScheduleBudgetQueryController : Controller
    {
        private readonly SBMSDbContext _dbContext = new SBMSDbContext();

        [HttpGet]
        public ActionResult Query()
        {
            return View("~/Views/ContractProposal/Query.cshtml", new ContractProposal());
        }

        [HttpPost]
        public ActionResult Result(ContractProposal contract)
        {
            ContractProposal originalContract = (from thisContract in _dbContext.Contracts
                                                 where thisContract.Username == contract.Username
                                                 select thisContract).FirstOrDefault();

            ViewBag.Message = originalContract == null
                ? "This customer does not have a contract with us!"
                : string.Empty;

            return View("~/Views/ContractProposal/Result.cshtml", originalContract);
        }
    }
}