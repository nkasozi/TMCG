using Microsoft.VisualStudio.TestTools.UnitTesting;
using TcmpTestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore.Tests
{
    [TestClass()]
    public class TcmpCoreTests
    {
        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            TcmpCore core = new TcmpCore();
            Result result = core.SetUpDatabase();
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void LoginTest_ValidInput_True()
        {
            TcmpCore core = new TcmpCore();
            SystemUser user = core.Login("Nsubugak", "T3rr1613");
            Assert.AreEqual(user.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void LoginTest_InvalidInput_False()
        {
            TcmpCore core = new TcmpCore();
            SystemUser user = core.Login("", "");
            Assert.AreNotEqual(user.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterSystemUserTest()
        {
            TcmpCore core = new TcmpCore();
            SystemUser systemUser = new SystemUser
            {
                Username = "Nsubugak",
                Password = "T3rr1613",
                RoleCode = "SuperAdmin",
                FullName = "Nsubuga Kasozi",
                ModifiedBy = "admin"
            };

            Result result = core.RegisterSystemUser(systemUser);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void PayForTransactionTest()
        {
            Payment payment = new Payment
            {
                DigitalSignature = SharedCommons.GenerateRandomString(),
                Password = "T3rr1613",
                PayerContact = "0794132389",
                PaymentChannel = "BANK",
                PaymentId = "1325621",
                PaymentNarration = "Test Payment",
                PaymentSystemCode = "SBU",
                PaymentType = "CASH",
                PaymerName = "Nsubuga Kasozi"
            };

            TcmpCore core = new TcmpCore();
            Result result = core.PayForTransaction(payment);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterItemTest()
        {
            Item item = new Item
            {
                ItemCode = SharedCommons.GenerateUniqueId("ITEM-"),
                CreatedBy = "admin",
                ItemCount = 10,
                ItemName = "Shoes",
                ItemPrice = 2000,
                ModifiedBy= "admin"
            };

            TcmpCore core = new TcmpCore();
            Result result = core.RegisterItem(item);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterPaymentSystemTest()
        {
            PaymentSystem system = new PaymentSystem
            {
                Password = "T3rr1613",
                PaymentSystemCode = "SBU",
                SecretKey = "T3rr16132016"
            };
            TcmpCore core = new TcmpCore();
            Result result = core.RegisterPaymentSystem(system);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterPaymentTypeTest()
        {
            PaymentType type = new PaymentType
            {
                TypeCode = "CASH",
                TypeName = "Cash"
            };
            TcmpCore core = new TcmpCore();
            Result result = core.RegisterPaymentType(type);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterSaleTest()
        {
            Sale sale = new Sale
            {
                SaleID = SharedCommons.GenerateUniqueId("SALE-"),
                CustomerId = "Nsubugak",
                TotalCost = 0,
                Tax = 0
            };
            TcmpCore core = new TcmpCore();
            Result result = core.RegisterSale(sale);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void RegisterSaleItemsTest()
        {
            SaleItem item = new SaleItem
            {
                ItemId = SharedCommons.GenerateUniqueId("ITEM-"),
                SaleId = SharedCommons.GenerateUniqueId("SALE-")
            };
            TcmpCore core = new TcmpCore();
            Result result = core.RegisterSaleItems(item);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }



        [TestMethod()]
        public void RegisterUserRoleTest()
        {
            UserRole role = new UserRole
            {
                RoleCode = "SUPER_ADMIN",
                RoleName = "Super Administrator"
            };
            TcmpCore core = new TcmpCore();
            Result result = core.RegisterUserRole(role);
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }

        [TestMethod()]
        public void SetUpDatabaseTest()
        {
            TcmpCore core = new TcmpCore();
            Result result = core.SetUpDatabase();
            Assert.AreEqual(result.StatusDesc, SharedCommonsGlobals.SUCCESS_STATUS_TEXT);
        }
    }
}