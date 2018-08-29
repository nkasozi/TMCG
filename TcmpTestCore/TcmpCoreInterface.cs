using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    public interface ITcmpCoreInterface
    {
        List<Item> GetAllAvailableItems();

        Result RegisterSaleItems(SaleItem[] saleItems);

        Result RegisterSale(Sale sale);

        Result RegisterItem(Item sale);

        Result RegisterPaymentType(PaymentType sale);

        Result RegisterPaymentSystem(PaymentSystem sale);

        SystemUser Login(string Username, string Password);

        Result PayForTransaction(Payment payment);

        Result RegisterCustomer(Customer user);

        object[] GetByID(string Type, string ID);

        object[] GetAll(string Type);

        Result RegisterSystemUser(SystemUser user);

        Result RegisterUserRole(UserRole user);

        Result LogError(string Message, string Identifier, string UserId);

        Result SetUpDatabase();
    }

    public class TcmpCore : ITcmpCoreInterface
    {
        public List<Item> GetAllAvailableItems()
        {
            List<Item> all = new List<Item>();
            try
            {
                //pick all items that are in stock ie. count>0
                Item[] items = Item.QueryWithStoredProc("GetAllItems").ToArray();

                //populate response
                all.AddRange(items);
            }
            catch (Exception)
            {
            }
            return all;
        }

        public object[] GetByID(string Type, string ID)
        {
            try
            {
                switch (Type)
                {
                    case "USERROLE":
                        return UserRole.FindAll().Where(i => i.RoleCode == ID).ToArray();
                    case "SALE":
                        return GetSalesById(ID);
                }
            }
            catch (Exception)
            {

            }

            return new object[] { };
        }

        private static object[] GetSalesById(string ID)
        {
            Sale[] sales = Sale.QueryWithStoredProc("GetSaleByID",ID).ToArray();
            foreach (var sale in sales)
            {
                SaleItem[] saleItems = SaleItem.QueryWithStoredProc("GetSaleItemsBySaleID", sale.Id).ToArray();
                foreach (var saleItem in saleItems)
                {
                    Item item = Item.QueryWithStoredProc("GetItemByID", saleItem.ItemId).FirstOrDefault();
                    sale.TotalCost += item != null ? item.ItemPrice : 0;
                }
            }
            return sales;
        }

        private static object[] GetSales()
        {
            Sale[] sales = Sale.QueryWithStoredProc("GetAllSales").ToArray();
            foreach (var sale in sales)
            {
                SaleItem[] saleItems = SaleItem.QueryWithStoredProc("GetSaleItemsBySaleID",sale.Id).ToArray();
                foreach (var saleItem in saleItems)
                {
                    Item item = Item.QueryWithStoredProc("GetItemByID",saleItem.ItemId).FirstOrDefault();
                    sale.TotalCost += item != null ? item.ItemPrice : 0;
                }
            }
            return sales;
        }

        public object[] GetAll(string Type)
        {
            try
            {
                switch (Type)
                {
                    case "USERROLE":
                        return UserRole.FindAll().ToArray();
                    case "SALE":
                        return GetSales();
                    case "ITEM":
                        return Item.FindAll().ToArray();
                }
            }
            catch (Exception)
            {

            }

            return new object[] { };
        }

        public Result LogError(string message, string location, string userId)
        {
            Result result = new Result();
            try
            {
                //generate an error log
                Errorlog errorlog = new Errorlog()
                {
                    Message = message,
                    LocationID = location,
                    UserID = userId
                };

                //Log the error async
                Task.Factory.StartNew(() => { try { errorlog.Save(); } catch (Exception) { } });

                //success
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public SystemUser Login(string Username, string Password)
        {
            SystemUser result = new SystemUser();
            try
            {
                //quick validations
                if (string.IsNullOrEmpty(Username))
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Please Supply a {nameof(Username)}";
                    return result;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Please Supply a {nameof(Password)}";
                    return result;
                }

                //find the first user whose username is the one supplied
                SystemUser user = SystemUser.QueryWithStoredProc("GetSystemUserByID",Username).FirstOrDefault();

                //oops no user found..stop
                if (user == null)
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Invalid Username or Password";
                    return result;
                }

                //hash the password supplied
                string hashedPassword = SharedCommons.GenerateMD5Hash(Password);

                //compare hashes
                if (hashedPassword != user.Password)
                {
                    //no match..stop
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Invalid Username or Password";
                    return result;
                }

                //user is authentic
                result = user;
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result PayForTransaction(Payment payment)
        {
            Result result = new Result();
            try
            {
                if (!payment.IsValid())
                {
                    result.StatusCode = payment.StatusCode;
                    result.StatusDesc = $"{payment.StatusDesc}";
                    return result;
                }

                payment.Save();

                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterCustomer(Customer cust)
        {
            Result result = new Result();
            try
            {
                if (!cust.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{cust.StatusDesc}";
                    return result;
                }

                Customer old = Customer.QueryWithStoredProc("GetCustomerByID",cust.CustomerID).FirstOrDefault();
                cust.Id = old != null ? old.Id : cust.Id;

                cust.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterItem(Item item)
        {
            Result result = new Result();
            try
            {
                if (!item.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{item.StatusDesc}";
                    return result;
                }

                Item old = Item.QueryWithStoredProc("GetItemByID",item.ItemCode).FirstOrDefault();
                item.Id = old != null ? old.Id : item.Id;

                item.SaveWithStoredProc("SaveItem",item.ItemCode,item.ItemName,item.ItemPrice,item.ItemImage,item.ItemCount,item.ModifiedBy);

                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterPaymentSystem(PaymentSystem system)
        {
            Result result = new Result();
            try
            {
                if (!system.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{system.StatusDesc}";
                    return result;
                }

                PaymentSystem old = PaymentSystem.FindAll().Where(i => i.PaymentSystemCode == system.StatusCode).FirstOrDefault();
                system.Id = old != null ? old.Id : system.Id;

                system.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterPaymentType(PaymentType type)
        {
            Result result = new Result();
            try
            {
                if (!type.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{type.StatusDesc}";
                    return result;
                }

                PaymentType old = PaymentType.FindAll().Where(i => i.TypeCode == type.TypeCode).FirstOrDefault();

                type.Id = old != null ? old.Id : type.Id;

                type.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterSale(Sale sale)
        {
            Result result = new Result();
            try
            {
                if (!sale.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{sale.StatusDesc}";
                    return result;
                }

                Sale old = Sale.QueryWithStoredProc("GetSaleID",sale.SaleID).FirstOrDefault();

                sale.Id = old != null ? old.Id : sale.Id;

                sale.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterSaleItems(params SaleItem[] saleItems)
        {
            Result result = new Result();
            try
            {
                foreach (var item in saleItems)
                {
                    if (!item.IsValid())
                    {
                        result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                        result.StatusDesc = $"{item.StatusDesc}";
                        return result;
                    }
                }

                foreach (var item in saleItems)
                {
                    SaleItem old = SaleItem.QueryWithStoredProc("GetSaleItemByID",item.SaleId,item.ItemId).FirstOrDefault();
                    item.Id = old != null ? old.Id : item.Id;

                    item.Save();
                }

                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterSystemUser(SystemUser user)
        {
            Result result = new Result();
            try
            {

                if (!user.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{user.StatusDesc}";
                    return result;
                }

                //check among the existing users for someone with the same username
                SystemUser old = SystemUser.QueryWithStoredProc("GetSystemUserByID",user.Username).FirstOrDefault();

                //a current user has been found with the same username
                if (old != null)
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Username Already in Use. Please try another Username";
                    return result;
                }

                //hash user password
                user.Password = SharedCommons.GenerateMD5Hash(user.Password);

                //save the user
                user.Save();

                //success
                result.ResponseId = user.Username;
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result RegisterUserRole(UserRole role)
        {
            Result result = new Result();
            try
            {
                if (!role.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"{role.StatusDesc}";
                    return result;
                }

                UserRole old = UserRole.FindAll().Where(i => i.RoleCode == role.RoleCode).FirstOrDefault();
                role.Id = old != null ? old.Id : role.Id;

                role.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        public Result SetUpDatabase()
        {
            Result result = new Result();
            try
            {
                DbInitializer.TypesToKeepTrackOf = new List<Type>() {
                                                                        typeof(Item),
                                                                        typeof(Payment),
                                                                        typeof(PaymentSystem),
                                                                        typeof(PaymentType),
                                                                        typeof(Result),
                                                                        typeof(SaleItem),
                                                                        typeof(SystemUser),
                                                                        typeof(UserRole),
                                                                        typeof(Sale),
                                                                        typeof(Errorlog),
                                                                        typeof(Customer)
                                                                     };

                DbResult dbresult = DbInitializer.Initialize();

                //db setup failed
                if (dbresult.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"ERROR: {dbresult.StatusDesc}";
                    return result;
                }

                return result = SeedData();
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            
            return result;
        }

        private Result SeedData()
        {
            Result result = new Result();

            
            UserRole role = new UserRole
            {
                RoleCode = "SUPER_ADMIN",
                RoleName = "Super Administrator"
            };

            result = RegisterUserRole(role);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

            role = new UserRole
            {
                RoleCode = "ADMIN",
                RoleName = "Administrator"
            };

            result = RegisterUserRole(role);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

            role = new UserRole
            {
                RoleCode = "STORE_KEEPER",
                RoleName = "Store Keeper"
            };

            result = RegisterUserRole(role);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

            PaymentType type = new PaymentType
            {
                TypeCode = "CASH",
                TypeName = "Cash"
            };

            result = RegisterPaymentType(type);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

            type = new PaymentType
            {
                TypeCode = "ONLINE",
                TypeName = "Online"
            };

            result = RegisterPaymentType(type);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

            //Random random = new Random(1000);

            //for (int i = 0; i <= 10; i++)
            //{
            //    Item item = new Item
            //    {
            //        CreatedBy = "admin",
            //        ItemCode = SharedCommons.GenerateUniqueId("ITEM-"),
            //        ItemCount = 20,
            //        ItemImage = "data:image/png;base64,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",
            //        ItemName = SharedCommons.GenerateRandomString(),
            //        ItemPrice = random.Next(50000) + 500,
            //        ModifiedBy = "admin"
            //    };

            //    result = RegisterItem(item);

            //    //failed to save
            //    if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            //    {
            //        return result;
            //    }

            //}

            return result;
        }
    }
}
