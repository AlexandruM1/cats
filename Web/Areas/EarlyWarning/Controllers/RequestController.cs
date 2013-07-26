﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.EarlyWarning;
using Cats.Helpers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Workflow = Cats.Models.Constant.WORKFLOW;


namespace Cats.Areas.EarlyWarning.Controllers
{
    public class RequestController : Controller
    {
        //
        // GET: /EarlyWarning/RegionalRequest/

        private IRegionalRequestService _regionalRequestService;
        private IFDPService _fdpService;
        //private IRoundService _roundService;
        private IAdminUnitService _adminUnitService;
        private IProgramService _programService;
        private ICommodityService _commodityService;
        private IRegionalRequestDetailService _reliefRequisitionDetailService;
        private IWorkflowStatusService _workflowStatusService;
        private IRationService _rationService;

        public RequestController(IRegionalRequestService reliefRequistionService
                                 , IFDPService fdpService
                                 , IAdminUnitService adminUnitService,
                                 IProgramService programService,
                                 ICommodityService commodityService,
                                 IRegionalRequestDetailService reliefRequisitionDetailService,
                                 IWorkflowStatusService workflowStatusService,
            IRationService rationService)
        {
            this._regionalRequestService = reliefRequistionService;
            this._adminUnitService = adminUnitService;
            this._commodityService = commodityService;
            this._fdpService = fdpService;
            this._programService = programService;
            this._reliefRequisitionDetailService = reliefRequisitionDetailService;
            this._workflowStatusService = workflowStatusService;
            this._rationService = rationService;
        }

     

        private IEnumerable<RegionalRequestViewModel> BuildRegionalRequestViewModel(
            IEnumerable<RegionalRequest> requests)
        {
            //TODO:While Displaying date make sure displayed based on user language preference
            var result = (from request in requests
                          select new RegionalRequestViewModel()
                                     {
                                         Program = request.Program.Name,
                                         ProgramId = request.ProgramId,
                                         ReferenceNumber = request.ReferenceNumber,
                                         Region = request.AdminUnit.Name,
                                         RegionID = request.RegionID,
                                         RegionalRequestID = request.RegionalRequestID,
                                         Year = request.Year,
                                         Remark = request.Remark,
                                         StatusID = request.Status,
                                         Month = request.Month,
                                         RequistionDate = request.RequistionDate,
                                         Status =
                                             _workflowStatusService.GetStatusName(Workflow.REGIONAL_REQUEST,
                                                                                  request.Status),
                                         RequestDateEt = EthiopianDate.GregorianToEthiopian(request.RequistionDate)


                                     });
            return result;
        }

        [HttpPost]
        public ActionResult Index(int year, int month)
        {
            // TODO: Filter the collection using incoming parameters
            ViewBag.Months = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");


            var reliefrequistions =
                _regionalRequestService.Get(r => r.RequistionDate.Year == year && r.RequistionDate.Month == month, null,
                                            "AdminUnit,Program");

            return View(reliefrequistions.ToList());
        }

        public ActionResult RequestsFromRegion()
        {
            ViewBag.Months = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.RegionID = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID",
                                              "Name");
            ViewBag.Status = new SelectList(_workflowStatusService.GetStatus(Workflow.REGIONAL_REQUEST), "StatusID",
                                            "Description");


            return View();
        }

        public JsonResult GetRation()
        {
           
           
            var ration = _rationService.GetAllRation();
            var rationViewModel = (from item in ration
                                   select new
                                              {
                                                  _commodityService.FindById(item.CommodityID).Name,
                                                  Value = item.Amount
                                              });
            return Json(rationViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Submitted()
        {
            var reliefrequistions = _regionalRequestService.Get(null, null, "AdminUnit,Program");
            var resutl = reliefrequistions.ToList().Select(item => new ReceivedRequisitionsDto()
                                                                       {
                                                                           Program = item.Program.Name,
                                                                           ReferenceNumber = item.ReferenceNumber,
                                                                           Region = item.AdminUnit.Name,
                                                                           RequistionDate = item.RequistionDate.Date,
                                                                           Remark = item.Remark,
                                                                           Year = item.Year,
                                                                           Status =
                                                                               _workflowStatusService.GetStatusName(
                                                                                   Workflow.REGIONAL_REQUEST,
                                                                                   item.Status),
                                                                           Round = item.Month,
                                                                           Create =
                                                                               _workflowStatusService.GetStatusName(
                                                                                   Workflow.REGIONAL_REQUEST,
                                                                                   item.Status)
                                                                       }).ToList();


            return Json(resutl.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ViewResult SubmittedRequest()
        {
            ViewBag.Months = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.RegionID = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID",
                                              "Name");
            ViewBag.Status = new SelectList(_workflowStatusService.GetStatus(Workflow.REGIONAL_REQUEST), "StatusID",
                                            "Description");

            var requests = _regionalRequestService.Get(null, null, "AdminUnit,Program");

            return View(BuildRegionalRequestViewModel(requests));
        }

        [HttpPost]
        public ViewResult SubmittedRequest(int? RegionID, int month, int? Status)
        {


            // TODO: Filter the collection using incoming parameters
            ViewBag.Months = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.RegionID = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID",
                                              "Name");
            ViewBag.Status = new SelectList(_workflowStatusService.GetStatus(Workflow.REGIONAL_REQUEST), "StatusID",
                                            "Description");
            var reliefrequistions = _regionalRequestService.GetSubmittedRequest(RegionID.HasValue ? RegionID.Value : 0,
                                                                                month,
                                                                                Status.HasValue ? Status.Value : 1);


            return View(reliefrequistions);
        }



        [HttpGet]
        public ActionResult New()
        {
            PopulateLookup();

            var relifRequisition = new RegionalRequest();
            var fdpList = _fdpService.GetAllFDP();
            var releifDetails = (from fdp in fdpList
                                 select new RegionalRequestDetail()
                                            {
                                                Beneficiaries = 0,
                                                Fdpid = fdp.FDPID,

                                            }).ToList();
            relifRequisition.RegionalRequestDetails = releifDetails;

            return View(relifRequisition);
        }

        private void PopulateLookup()
        {
            ViewBag.RegionID = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID",
                                              "Name");
            ViewBag.ProgramID = new SelectList(_programService.GetAllProgram(), "ProgramID", "Name");
            ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "ID", "Name");
            ViewBag.CommodityID = new SelectList(_commodityService.GetAllCommodity(), "CommodityID", "Name");
            ViewBag.FDPID = new SelectList(_fdpService.GetAllFDP(), "FDPID", "Name");

        }

        //
        // GET: /ReliefRequisitoin/Details/5

        public ActionResult Details(int id = 0)
        {
            RegionalRequest reliefrequistion =
                _regionalRequestService.Get(t => t.RegionalRequestID == id, null, "AdminUnit,Program").FirstOrDefault();
            if (reliefrequistion == null)
            {
                return HttpNotFound();
            }
            return View(reliefrequistion);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            PopulateLookup();
            var regionalRequest =
                _regionalRequestService.Get(t => t.RegionalRequestID == id, null,
                                            "RegionalRequestDetails,RegionalRequestDetails.Fdp," +
                                            "RegionalRequestDetails.Fdp.AdminUnit,RegionalRequestDetails.Fdp.AdminUnit.AdminUnit2")
                    .
                    FirstOrDefault();
            return View(regionalRequest);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(RegionalRequest regionalRequest)
        {
            var requId = 0;
            if(regionalRequest!=null && ModelState.IsValid )
            {
                var target = _regionalRequestService.FindById(regionalRequest.RegionalRequestID);
                target.ProgramId = regionalRequest.ProgramId;
                target.ReferenceNumber = regionalRequest.ReferenceNumber;
                target.RegionID = regionalRequest.RegionID;
                target.Year = regionalRequest.Year;
                target.RequistionDate = regionalRequest.RequistionDate;
                target.Remark = regionalRequest.Remark;
                target.Month = regionalRequest.Month;

                _regionalRequestService.EditRegionalRequest(target);
                return RedirectToAction("Allocation", "Request", new { id = regionalRequest.RegionalRequestID });
            }
            //foreach (var reliefRequisitionDetailEditInput in input)
            //{

            //    var tempReliefRequistionDetail = _reliefRequisitionDetailService.FindById(reliefRequisitionDetailEditInput.Number);
            //    requId = tempReliefRequistionDetail.RegionalRequestID;
            //    tempReliefRequistionDetail.Beneficiaries = reliefRequisitionDetailEditInput.Beneficiaries;
            //    tempReliefRequistionDetail.CSB = reliefRequisitionDetailEditInput.CSB;
            //    tempReliefRequistionDetail.Oil = reliefRequisitionDetailEditInput.Oil;
            //    tempReliefRequistionDetail.Grain = reliefRequisitionDetailEditInput.Grain;
            //    tempReliefRequistionDetail.Pulse = reliefRequisitionDetailEditInput.Pulse;

            //}

            PopulateLookup();
            return View(regionalRequest);
        }

        public ActionResult ApproveRequest(int id)
        {
            _regionalRequestService.ApproveRequest(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult New(RegionalRequest regionalRequest, string requestDate)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(requestDate);
            }
            catch (Exception)
            {
                var strEth = new getGregorianDate();
                date = strEth.ReturnGregorianDate(requestDate);
                //throw;
            }
            regionalRequest.RequistionDate = date;

            if (ModelState.IsValid)
            {
                //TODO:Filter with selected region
                var fdpList = _fdpService.FindBy(t => t.AdminUnit.AdminUnit2.ParentID == regionalRequest.RegionID);
                var releifDetails = (from fdp in fdpList
                                     select new RegionalRequestDetail()
                                                {
                                                    Beneficiaries = 0,
                                                    Fdpid = fdp.FDPID,
                                                    Grain = 0,
                                                    Pulse = 0,
                                                    Oil = 0,
                                                    CSB = 0

                                                }).ToList();
                regionalRequest.RegionalRequestDetails = releifDetails;
                regionalRequest.Status = (int)RegionalRequestStatus.Draft;
                _regionalRequestService.AddRegionalRequest(regionalRequest);
                return RedirectToAction("Index", "Request");
            }

            PopulateLookup();
            return View(regionalRequest);
        }


        #region Regional Request Detail


        public ActionResult Allocation(int id)
        {
            ViewBag.RequestID = id;
            var request =
                _regionalRequestService.Get(t => t.RegionalRequestID == id, null, "AdminUnit,Program").FirstOrDefault();
            var requestModelView = BindRegionalRequestViewModel(request);
            return View(requestModelView);
        }

        public ActionResult Allocation_Read([DataSourceRequest] DataSourceRequest request, int id)
        {

            var requestDetails = _reliefRequisitionDetailService.FindBy(t => t.RegionalRequestID == id);
            var requestDetailViewModels = (from dtl in requestDetails select BindRegionalRequestDetailViewModel(dtl));
            return Json(requestDetailViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private RegionalRequestDetailViewModel BindRegionalRequestDetailViewModel(RegionalRequestDetail regionalRequestDetail)
        {
            return new RegionalRequestDetailViewModel()
                       {
                           Beneficiaries = regionalRequestDetail.Beneficiaries,
                           CSB = regionalRequestDetail.CSB,
                           FDP = regionalRequestDetail.Fdp.Name,
                           Fdpid = regionalRequestDetail.Fdpid,
                           Grain = regionalRequestDetail.Grain,
                           Oil = regionalRequestDetail.Oil,
                           Pulse = regionalRequestDetail.Pulse,
                           RegionalRequestID = regionalRequestDetail.RegionalRequestID,
                           RegionalRequestDetailID = regionalRequestDetail.RegionalRequestDetailID,
                           Woreda= regionalRequestDetail.Fdp.AdminUnit.Name,
                           Zone=regionalRequestDetail.Fdp.AdminUnit.AdminUnit2.Name 
                       };

        }
        private RegionalRequestDetail BindRegionalRequestDetail(RegionalRequestDetailViewModel regionalRequestDetailViewModel)
        {
            return new RegionalRequestDetail()
                               {
                                   Beneficiaries = regionalRequestDetailViewModel.Beneficiaries,
                                   CSB = regionalRequestDetailViewModel.CSB,
                                   Fdpid = regionalRequestDetailViewModel.Fdpid,
                                   Grain = regionalRequestDetailViewModel.Grain,
                                   Oil = regionalRequestDetailViewModel.Oil,
                                   Pulse = regionalRequestDetailViewModel.Pulse,
                                   RegionalRequestID = regionalRequestDetailViewModel.RegionalRequestID,
                                   RegionalRequestDetailID = regionalRequestDetailViewModel.RegionalRequestDetailID
                               };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Create([DataSourceRequest] DataSourceRequest request, RegionalRequestDetailViewModel regionalRequestDetailViewModel)
        {
            if (regionalRequestDetailViewModel != null && ModelState.IsValid)
            {
                _reliefRequisitionDetailService.AddRegionalRequestDetail(BindRegionalRequestDetail(regionalRequestDetailViewModel));
            }

            return Json(new[] { regionalRequestDetailViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Update([DataSourceRequest] DataSourceRequest request, RegionalRequestDetailViewModel regionalRequestDetail)
        {
            if (regionalRequestDetail != null && ModelState.IsValid)
            {
                var target = _reliefRequisitionDetailService.FindById(regionalRequestDetail.RegionalRequestDetailID);
                if (target != null)
                {
                    target.Grain = regionalRequestDetail.Grain;
                    target.Pulse = regionalRequestDetail.Pulse;
                    target.CSB = regionalRequestDetail.CSB;
                    target.Oil = regionalRequestDetail.Oil;
                    target.Beneficiaries = regionalRequestDetail.Beneficiaries;
                    _reliefRequisitionDetailService.EditRegionalRequestDetail(target);
                }
            }

            return Json(new[] { regionalRequestDetail }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  RegionalRequestDetail regionalRequestDetail)
        {
            if (regionalRequestDetail != null)
            {
                _reliefRequisitionDetailService.DeleteById(regionalRequestDetail.RegionalRequestDetailID);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        #endregion


        #region Reguest

        public ActionResult Index()
        {
           
         
            var regions = (from region in _adminUnitService.GetRegions()
                           select new
                                      {
                                          region.AdminUnitID,
                                          region.Name
                                      });
            ViewData["adminunits"] = regions;
            var programs = (from program in _programService.GetAllProgram()
                           select new
                           {
                               program.ProgramID,
                               program.Name
                           });
            ViewData["programs"] = programs;
                 
                
           


            return View();
        }

        public ActionResult Request_Read([DataSourceRequest] DataSourceRequest request)
        {

            var requests = _regionalRequestService.GetAllRegionalRequest();
            var requestViewModels = (from dtl in requests select BindRegionalRequestViewModel(dtl));
            return Json(requestViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private RegionalRequestViewModel BindRegionalRequestViewModel(RegionalRequest regionalRequest)
        {
            return new RegionalRequestViewModel()
            {
               
                ProgramId = regionalRequest.ProgramId,
                Program=regionalRequest.Program.Name ,
                Region=regionalRequest.AdminUnit.Name ,
                ReferenceNumber = regionalRequest.ReferenceNumber,
                RegionID = regionalRequest.RegionID,
                RegionalRequestID = regionalRequest.RegionalRequestID,
                Remark = regionalRequest.Remark,
                RequestDateEt = EthiopianDate.GregorianToEthiopian(regionalRequest.RequistionDate
                ),
                Month = regionalRequest.Month,
                Status = _workflowStatusService.GetStatusName(WORKFLOW.REGIONAL_REQUEST, regionalRequest.Status),
                RequistionDate = regionalRequest.RequistionDate,
                StatusID = regionalRequest.Status,
                Year = regionalRequest.Year,
                
                
                
            };

        }
        private RegionalRequest BindRegionalRequest(RegionalRequestViewModel regionalRequestViewModel)
        {
            return new RegionalRequest()
            {
                ProgramId = regionalRequestViewModel.ProgramId,
                ReferenceNumber = regionalRequestViewModel.ReferenceNumber,
                RegionID = regionalRequestViewModel.RegionID,
                RegionalRequestID = regionalRequestViewModel.RegionalRequestID,
                Remark = regionalRequestViewModel.Remark,
                Month = regionalRequestViewModel.Month,
                RequistionDate = regionalRequestViewModel.RequistionDate,
                Status = regionalRequestViewModel.StatusID,
                Year = regionalRequestViewModel.Year
            };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Request_Create([DataSourceRequest] DataSourceRequest request, RegionalRequestViewModel regionalRequestViewModel)
        {
            if (regionalRequestViewModel != null && ModelState.IsValid)
            {
                    var regionalRequest = BindRegionalRequest(regionalRequestViewModel);
                    _regionalRequestService.AddRegionalRequest(regionalRequest);
            }

            return Json(new[] { regionalRequestViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Request_Update([DataSourceRequest] DataSourceRequest request, RegionalRequestViewModel regionalRequest)
        {
            if (regionalRequest != null && ModelState.IsValid)
            {
                var target = _regionalRequestService.FindById(regionalRequest.RegionalRequestID);
                if (target != null)
                {
                    target.ProgramId = regionalRequest.ProgramId;
                    target.ReferenceNumber = regionalRequest.ReferenceNumber;
                    target.RegionID = regionalRequest.RegionID;
                    target.Remark = regionalRequest.Remark;
                    target.Month = regionalRequest.Month;
                    target.RequistionDate = regionalRequest.RequistionDate;
                    target.Status = regionalRequest.StatusID;
                    target.Year = regionalRequest.Year;
                    _regionalRequestService.EditRegionalRequest(target);
                }
            }

            return Json(new[] { regionalRequest }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Request_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  RegionalRequest regionalRequest)
        {
            if (regionalRequest != null)
            {
                _reliefRequisitionDetailService.DeleteById(regionalRequest.RegionalRequestID);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        #endregion

    }








}