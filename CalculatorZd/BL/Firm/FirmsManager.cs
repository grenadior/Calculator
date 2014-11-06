using System;
using BO;
using BO.Implementation;
using Common.Api;
using Common.Api.Cryptography;
using DA;

namespace BL
{
    public static class FirmsManager
    {
        public static string GeneratePwdSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=";
            var rand = new Random((int) DateTime.Now.Ticks);
            var c = new char[32];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = chars[rand.Next(0, 50)];
            }
            return new string(c);
        }

        public static Firm CreateFirm(Firm firm, string password)
        {
            string PwdSalt = GeneratePwdSalt();

            if (!string.IsNullOrEmpty(PwdSalt))
                password = password + PwdSalt;

            string mD5 = Hash.ReadableHash(Hash.MD5, password);
            DateTime passwordExpDate = FirmHelper.GetPasswordExpirationDate();
            firm.MD5 = mD5;
            firm.PasswordExpDate = passwordExpDate;
            return FirmAdapter.CreateFirm(firm);
        }

        public static Firm GetFirmByID(Guid id)
        {
            return FirmAdapter.GetFirmByID(id);
        }

        public static Firm UpdateFirm(Firm firm)
        {
            return FirmAdapter.UpdateFirm(firm);
        }

        public static OperationStatus ActivateEmail(Guid activationCode, Guid firmID)
        {
            return FirmAdapter.ActivateEmail(activationCode, firmID);
        }

        public static OperationStatus UpdateActivateEmail(Guid activationCode, string email, Guid firmID)
        {
            return FirmAdapter.UpdateActivateEmail(activationCode, email, firmID,
                DateTime.Now.AddDays(Constants.ExpiredDateActivationEmail));
        }

        public static OperationStatus InsertActivationEmail(Guid activationCode, string email, Guid firmID)
        {
            return FirmAdapter.InsertActivationEmail(activationCode, firmID, email,
                DateTime.Now.AddDays(Constants.ExpiredDateActivationEmail));
        }

        public static Guid GetFirmIDByActivationCode(Guid activationCode)
        {
            return FirmAdapter.GetFirmIDByActivationCode(activationCode);
        }
    }
}