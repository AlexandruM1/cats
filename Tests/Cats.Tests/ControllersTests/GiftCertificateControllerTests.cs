using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Controllers;
using Cats.Areas.GiftCertificate.Models;
using Cats.Models;
using Cats.Services.EarlyWarning;
using Moq;
using NUnit.Framework;

namespace Cats.Tests.ControllersTests
{
    [TestFixture]
    public class GiftCertificateControllerTests
    {
        #region SetUp / TearDown

        private GiftCertificateController _giftCertificateController;
        [SetUp]
        public void Init()
        {
           
            var giftDetails = new List<GiftCertificateDetail>()
                                  {
                                      new GiftCertificateDetail
                                          {
                                              GiftCertificateID = 1,
                                              AccountNumber = 1,
                                              BillOfLoading = "1",
                                              Detail = new Detail {DetailID = 1, Name = "WFP"},
                                              CommodityID = 1,
                                              Commodity = new Commodity() {CommodityID = 1, Name = "CSB"}
                                          }
                                  };
            ;
            var gifts = new List<GiftCertificate>()
                            {
                                new GiftCertificate()
                                    {GiftCertificateID = 1, ProgramID = 1, ReferenceNo = "1", SINumber = "SI-001",DonorID=1,Donor=new Donor(){DonorID=1,Name="WFP"},GiftCertificateDetails=giftDetails},
                                new GiftCertificate()
                                    {GiftCertificateID = 1, ProgramID = 1, ReferenceNo = "1", SINumber = "SI-001",DonorID=1,Donor=new Donor(){DonorID=1,Name="WFP"},GiftCertificateDetails=giftDetails},
                                new GiftCertificate()
                                    {GiftCertificateID = 1, ProgramID = 1, ReferenceNo = "1", SINumber = "SI-001",DonorID=1,Donor=new Donor(){DonorID=1,Name="WFP"},GiftCertificateDetails=giftDetails}
                            };



            var giftCertificateService = new Mock<IGiftCertificateService>();
            giftCertificateService.Setup(t => t.IsSINumberNewOrEdit(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            giftCertificateService.Setup(t => t.Get(It.IsAny<Expression<Func<GiftCertificate,bool>>>(),It.IsAny<Func<IQueryable<GiftCertificate>,IOrderedQueryable<GiftCertificate>>>(),It.IsAny<string>())).Returns(gifts);
            var giftCertificateDetailService = new Mock<IGiftCertificateDetailService>();
            _giftCertificateController = new GiftCertificateController(giftCertificateService.Object, giftCertificateDetailService.Object);
        }

        [TearDown]
        public void Dispose()
        { _giftCertificateController.Dispose();}

        #endregion

        #region Tests

        [Test]
        public void ShouldReturnTrueIsSINumberNewOrEdit()
        {
            //ACT
            var result = _giftCertificateController.NotUnique("SI-001", 1);

            //Assert

            Assert.IsInstanceOf<JsonResult>(result);
            Assert.IsTrue((bool)((JsonResult)result).Data);
        }
        [Test]
        public void ShouldDisplayGiftCertificates()
        {
            //Act
            var result = _giftCertificateController.Index();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<List<GiftCertificateViewModel>>(((ViewResult)result).Model);
        }

        #endregion
    }
}
