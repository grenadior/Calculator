using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DA.ServerProperties;

namespace BO.Implementation
{
    public class ServerProperties
    {
        private static volatile ServerProperties instance;
        private static readonly object syncRoot = new Object();
        public float SearchResultMainCoeff = default(float);
        public string EmailSMTPClient = "";
        public string FromMail_Password = "";
        public string FromMail = "";
        public string DBGruzhon = "";
        public string DBPorozhn = "";
        public string ReportStoragePath = "";
        public int EmailPort = 80;
        public int ReturnMainSiteHttpPort = 80;
        public string SiteName = "site.ru";

        #region ServerProperties
         private string SearchResultMainCoeff_Key = "SearchResultMainCoeff";
         private string EmailSMTPClient_Key = "EmailSMTPClient";
         private string FromMail_Password_Key = "fromMail_Password";
         private string FromMail_Key = "FromMail";
         private string DBGruzhon_Key = "DBGruzhon";
         private string DBPorozh_Key = "DBPorozhn";
         private string ReportStoragePath_KEY = "ReportStoragePath";
         private string EmailPort_Key = "EmailPort";
         private string ReturnMainSiteHttpPort_Key = "ReturnMainSiteHttpPort";
         private string SiteName_Key = "SiteName";
        
        #endregion

        private ServerProperties()
        {
            
        }

        public void Init()
        {
            Settings = ServerPropertiesAdapter.GetServerProperties();
            SearchResultMainCoeff = GetFloatValue(SearchResultMainCoeff_Key);
            FromMail = GetStringValue(FromMail_Key);
            EmailSMTPClient = GetStringValue(EmailSMTPClient_Key);
            FromMail_Password = GetStringValue(FromMail_Password_Key);
            DBGruzhon = GetStringValue(DBGruzhon_Key);
            DBPorozhn = GetStringValue(DBPorozh_Key);
            ReportStoragePath = GetStringValue(ReportStoragePath_KEY);
            EmailPort = GetIntValue(EmailPort_Key);
            ReturnMainSiteHttpPort = GetIntValue(ReturnMainSiteHttpPort_Key);
            SiteName = GetStringValue(SiteName_Key);
        }
        public static ServerProperties Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ServerProperties();
                    }
                }

                return instance;
            }
        }

        private List<ServerProperty> Settings { get; set; }

        //TODO: переделать под произвольные типы
        public string GetStringValue(string name)
        {
            ServerProperty property = Settings.FirstOrDefault(rs => rs.Name == name);
            if (property != null)
                return  property.StringValue;
            else
            {
                return "";
            }
        }

        public int GetIntValue(string name)
        {
            ServerProperty property = Settings.FirstOrDefault(rs => rs.Name == name);
            if (property != null)
                return property.IntValue;
            else
            {
                return EmailPort;
            }
        }

        public float GetFloatValue(string name)
        {
            ServerProperty property = Settings.FirstOrDefault(rs => rs.Name.ToLower() == name.ToLower());
            if (property != null)
                return property.FloatValue;
            else
            {
                return 0;
            }
        }
    }
}