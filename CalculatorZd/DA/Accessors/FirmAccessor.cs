using BLToolkit.DataAccess;
using BO;
using System;

namespace DA.Accessors
{
    public abstract class FirmAccessor : DataAccessor<Firm>
    {
        // Here we explicitly specify a stored procedure name.
        //
        [SprocName("p_Firm_Insert")]
        
        public abstract Firm CreateFirm(Firm firm);

        [SprocName("p_Firm_Update")]

        public abstract Firm FirmUpdate(Firm firm);

        [SprocName("p_Firm_InsertActivationCode")]
        public abstract Firm CreateActivationCode(Guid @activationCode, Guid @firmId, int @activationType, string @email, DateTime @expiredDate);
        
        [SprocName("p_Firm_GetByID")]
        public abstract Firm GetFirmByID(Guid @id);


        [SprocName("p_Firm_GetByEmail")]
        public abstract Firm GetFirmByEmail(string @email);


        [SprocName("p_Firm_GetFirmIDByActivationCode")]
        public abstract Firm GetFirmIDByActivationCode(Guid @activationCode, int @activationType);

        [SprocName("p_Firm_ActivateActivationCode")]
        public abstract int ActivateActivationCode(Guid @activationCode, int @activationType);
 
    }
}
