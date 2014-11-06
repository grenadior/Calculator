using System;
using Common.Api;
using Common.Api.Cryptography;
using Configuration;
using DA;
using Constants = Common.Api.Constants;

namespace BO.Implementation
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
       
        public static BO.Firm GetFirmByID(Guid id)
        {
            return FirmAdapter.GetFirmByID(id);
        }
        public static Firm GetLoginByEmail(string email)
        {
            return FirmAdapter.GetFirmByEmail(email);
        }
        public static BO.Firm UpdateFirm(BO.Firm firm)
        {
            return FirmAdapter.UpdateFirm(firm);
        }

        public static OperationStatus ActivateEmail(Guid activationCode)
        {
            return FirmAdapter.ActivateActivationCode(activationCode, (int)ActivationType.ActivationEmail);
        }

        public static Firm GetFirmIDByActivationCode(Guid activationCode, int activationType)
        {
            return FirmAdapter.GetFirmIDByActivationCode(activationCode, activationType);
        }
    }
}