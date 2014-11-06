using System;
using System.Data;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BO;
using Common.Api;
using Configuration;
using DA.Accessors;

namespace DA
{
    public class FirmAdapter
    {
        private static string p_Firm_Insert = "p_Firm_Insert";
        private static string p_Firm_Get = "p_Firm_Get";
        private static string p_Firm_ActivateEmail = "p_Firm_ActivateEmail";
        private static string p_Firm_GetFirmIDByActivationCode = "p_Firm_GetFirmIDByActivationCode";
        private static string p_Firm_InsertActivationCode = "p_Firm_InsertActivationCode";


        public static Firm CreateFirm(Firm firm)
        {
            Firm createdFirm = null;

            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    createdFirm = fa.CreateFirm(firm);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return createdFirm;
        }


        public Firm CreateUser(int id)
        {
            Firm user = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                try
                {
                    db.SetCommand(p_Firm_Get);
                    db.Parameter("ID", id, DbType.Int32);

                    user = db.ExecuteObject<Firm>();
                }
                catch (Exception e)
                {
                    throw Trace.Log<ExceptionHolder>(e);
                }
            }
            return user;
        }


        public static OperationStatus ActivateActivationCode(Guid activationCode, int activationType)
        {
            var status = OperationStatus.Failure;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    int result = fa.ActivateActivationCode(activationCode, activationType);
                    status = result > 0 ? OperationStatus.Success : OperationStatus.Failure;
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }
            return status;
        }
        
        public static OperationStatus InsertActivationCode(Guid activationCode, Guid firmID, string email,
            DateTime expiredDate, int activationType)
        {
            var status = OperationStatus.Failure;
            using (var db = new DbManager(DB.LocalDb))
            {
                try
                {
                    db.SetSpCommand(p_Firm_InsertActivationCode,
                        db.Parameter("ActivationCode", activationCode, DbType.Guid),
                        db.Parameter("ActivationType", activationType, DbType.Int32),
                        db.Parameter("FirmID", firmID, DbType.Guid),
                        db.Parameter("Email", email, DbType.String),
                        db.Parameter("ExpiredDate", expiredDate, DbType.DateTime));

                    status = db.ExecuteNonQuery() > 0 ? OperationStatus.Success : OperationStatus.Failure;
                }
                catch (Exception e)
                {
                    throw Trace.Log<ExceptionHolder>(e);
                }
            }
            return status;
        }

        public static Firm GetFirmIDByActivationCode(Guid activationCode, int activationType)
        {
            Firm firm = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    firm = fa.GetFirmIDByActivationCode(activationCode, activationType);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }
            return firm;
        }
       

        public static bool CreateRestorePasswordActivationInfo(Guid firmid, Guid activationCode, int activationType)
        {
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    fa.CreateActivationCode(activationCode, firmid, activationType, "", DateTime.Now.AddDays(2));
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);

                    return false;
                }
            }
            return true;
        }

        public static Firm GetFirmByID(Guid id)
        {
            Firm firm = null;

            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    firm = fa.GetFirmByID(id);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return firm;
        }

        public static Firm GetFirmByEmail(string email)
        {
            Firm firm = null;

            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    firm = fa.GetFirmByEmail(email);
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return firm;
        }

        public static Firm UpdateFirm(Firm firm)
        {
            Firm updatedFirm = null;

            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<FirmAccessor>(db);

                try
                {
                    updatedFirm = fa.FirmUpdate(firm);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return updatedFirm;
        }
    }
}