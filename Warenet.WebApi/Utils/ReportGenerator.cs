using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warenet.WebApi.Utils
{
    public class ReportGenerator
    {
        ReportDocument rptDoc;

        public ReportDocument GetReportDocument(string ReportName, string ReportPath, Dictionary<string, object> Params = null)
        {
            string path = ReportPath + @"\" + ReportName;

            try
            {
                rptDoc = new ReportDocument();
                rptDoc.Load(path);
            }
            catch (Exception ex)
            {
                string msg = "The report file " + path +
                " can not be loaded, ensure that the report exists or the path is correct." +
                "\nException:\n" + ex.Message +
                "\nSource:" + ex.Source +
                "\nStacktrace:" + ex.StackTrace;
                throw new Exception(msg);
            }

            // set login
            rptDoc.SetDatabaseLogon("sa","sysfreight");

            // set parameters
            if (Params != null)
            {
                int paramCnt = Params.Count();
                int rptParamCnt = rptDoc.DataDefinition.ParameterFields.Count;

                if (paramCnt < rptParamCnt) throw new InvalidCastException();

                foreach (ParameterFieldDefinition rptParam in rptDoc.DataDefinition.ParameterFields)
                {
                    string paramName = rptParam.ParameterFieldName;
                    object paramValue = Params[paramName];

                    if (paramValue != null)
                    {
                        PassParameter(paramName, paramValue);
                    }
                    else
                    {
                        throw new InvalidCastException();
                    }
                }
            }

            //setDbInfo(rptDoc,"Warenet","UNILYGN","sa","sysfreight");

            return rptDoc;
        }

        ///
        /// Set parameter value in crystal report on corresponding index
        ///
        private void PassParameter(string Name, object Value)
        {
            // '
            // ' Declare the parameter related objects.
            // '
            ParameterDiscreteValue crParameterDiscreteValue;
            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldLocation;
            ParameterValues crParameterValues;

            crParameterFieldDefinitions = rptDoc.DataDefinition.ParameterFields;
            crParameterFieldLocation = (ParameterFieldDefinition)crParameterFieldDefinitions[Name];
            crParameterValues = crParameterFieldLocation.CurrentValues;

            crParameterDiscreteValue = new CrystalDecisions.Shared.ParameterDiscreteValue();
            crParameterDiscreteValue.Value = Value;
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldLocation.ApplyCurrentValues(crParameterValues);
        }


        public void setDbInfo(ReportDocument rpt, string ServerName, string DatabaseName, string UserName, string Password)
        {
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            foreach (CrystalDecisions.CrystalReports.Engine.Table t in rpt.Database.Tables)
            {
                logoninfo = t.LogOnInfo;
                logoninfo.ReportName = rpt.Name;
                logoninfo.ConnectionInfo.ServerName = ServerName;
                logoninfo.ConnectionInfo.DatabaseName = DatabaseName;
                logoninfo.ConnectionInfo.UserID = UserName;
                logoninfo.ConnectionInfo.Password = Password;
                logoninfo.TableName = t.Name;
                t.ApplyLogOnInfo(logoninfo);
                //t.Location = t.Name;
            }
        }

        //protected void DisplayReport()
        //{
        //    try
        //    {
        //        ReportDocument rpt = new ReportDocument();
        //        rpt.Load(Server.MapPath("~/CRList.rpt"));
        //        string ServerName = ConfigurationManager.AppSettings["Server"].ToString();
        //        string DatabaseName = ConfigurationManager.AppSettings["DBName"].ToString();
        //        string UserName = ConfigurationManager.AppSettings["UserName"].ToString();
        //        string Password = ConfigurationManager.AppSettings["Password"].ToString();
        //        setDbInfo(rpt, ServerName, DatabaseName, UserName, Password);
        //        CRPTViewer.ReportSource = rpt;
        //        CRPTViewer.RefreshReport();
        //    }
        //    catch (Exception exc)
        //    {
        //        throw exc;
        //    }
        //}
    }
}