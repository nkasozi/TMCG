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
        Result RegisterSaleItems(SaleItem[] saleItems);

        Result RegisterSale(Sale sale);

        Result RegisterItem(Item sale);

        Result RegisterPaymentType(PaymentType sale);

        Result RegisterPaymentSystem(PaymentSystem sale);

        SystemUser Login(string Username, string Password);

        Result PayForTransaction(Payment payment);

        Result RegisterSystemUser(SystemUser user);

        Result RegisterUserRole(UserRole user);

        Result SetUpDatabase();
    }

    public class TcmpCore : ITcmpCoreInterface
    {

        public SystemUser Login(string Username, string Password)
        {
            SystemUser result = new SystemUser();
            try
            {
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

                SystemUser user = SystemUser.FindAll().Where(i => i.Username == Username).FirstOrDefault();

                if (user == null)
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Invalid Username or Password";
                    return result;
                }

                string hashedPassword = SharedCommons.GenerateMD5Hash(Password);

                if (hashedPassword != user.Password)
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                    result.StatusDesc = $"Invalid Username or Password";
                    return result;
                }

                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            result.Save();
            return result;
        }

        public Result PayForTransaction(Payment payment)
        {
            Result result = new Result();
            try
            {
                if (!payment.IsValid())
                {
                    result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
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
            result.Save();
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
                
                item.Save();
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            result.Save();
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

                PaymentSystem old = PaymentSystem.FindAll().Where(i=>i.PaymentSystemCode==system.StatusCode).FirstOrDefault();
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
            result.Save();
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
            result.Save();
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

                Sale old = Sale.FindAll().Where(i => i.SaleID == sale.SaleID).FirstOrDefault();

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
            result.Save();
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
                    SaleItem old = SaleItem.FindAll().Where(i => (i.SaleId == item.SaleId&&i.ItemId==item.ItemId)).FirstOrDefault();
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
            result.Save();
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

                SystemUser old = SystemUser.FindAll().Where(i => i.Username == user.Username).FirstOrDefault();
                user.Id = old != null ? old.Id : user.Id;

                //hash user password
                user.Password = SharedCommons.GenerateMD5Hash(user.Password);
                user.Save();

                result.ResponseId = user.Username;
                result.StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                result.StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = $"ERROR: {ex.Message}";
            }
            result.Save();
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
            result.Save();
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
                                                                        typeof(Sale)
                                                                     };

                DbResult dbresult = DbInitializer.CreateDbIfNotExistsAndUpdateSchema();

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
            result.Save();
            return result;
        }

        private Result SeedData()
        {
            Result result = new Result();

            SystemUser systemUser = new SystemUser
            {
                Username = "Nsubugak",
                Password = "T3rr1613",
                RoleCode = "SUPER_ADMIN",
                FullName = "Nsubuga Kasozi",
                ModifiedBy = "admin"
            };

            result = RegisterSystemUser(systemUser);

            //failed to save
            if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
            {
                return result;
            }

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

            return result;
        }
    }
}
