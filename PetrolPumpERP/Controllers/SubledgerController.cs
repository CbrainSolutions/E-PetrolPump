﻿using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class SubledgerController : Controller
    {
        SubledgerBL objSubledger = SubledgerBL.Instance;
        // GET: Subledger
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            //ViewBag.MainledgerList = objSubledger.GetMainledgers().MainledgerList;
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetAllSubledgers()
        {
            //ViewBag.MainledgerList = objSubledger.GetMainledgers().MainledgerList;
            return Json(objSubledger.GetAllSubledgers(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(SubledgerModel model)
        {
            model.SubLedgerCreationDate = DateTime.Now.Date;
            model.IsDelete = false;
            return Json(objSubledger.SaveSubledger(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(SubledgerModel model)
        {
            return Json(objSubledger.UpdateSubledger(model));
        }
    }
}