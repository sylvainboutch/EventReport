using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EventReport
{
    public static class Report
    {
        private static IEnumerable<Model.LCL_V_EVENTREPORT> _viewReport;
        public static IEnumerable<Model.LCL_V_EVENTREPORT> viewReport
        {
            get
            {
                if (_viewReport == null || _viewReport.Count() == 0)
                {
                    using (Model.EventModel db = new Model.EventModel())
                    {

                        var query = (from st in db.LCL_V_EVENTREPORT
                                     //where st.MORE_IRBAGENCY != null
                                     //&& st.IRBIDENTIFIERS != null
                                     select st);
                        _viewReport = query.ToList<Model.LCL_V_EVENTREPORT>();
                    }
                }
                return _viewReport;
            }
            set
            {
                _viewReport = value;
            }
        }


        private static IEnumerable<Model.EVENT_REPORT> _tableReport;
        public static IEnumerable<Model.EVENT_REPORT> tableReport
        {
            get
            {
                if (_tableReport == null || _tableReport.Count() == 0)
                {
                    using (Model.EventModel db = new Model.EventModel())
                    {

                        var query = (from st in db.EVENT_REPORT
                                     //where st.MORE_IRBAGENCY != null
                                     //&& st.IRBIDENTIFIERS != null
                                     select st);
                        _tableReport = query.ToList<Model.EVENT_REPORT>();
                    }
                }
                return _tableReport;
            }
            set
            {
                _tableReport = value;
            }
        }


        public static bool generateReport()
        {
            tableReport = null;
            viewReport = null;
            foreach (Model.LCL_V_EVENTREPORT vevent in viewReport)
            {
                var reportTable = (from st in tableReport
                                   where st.PATIENTEVENTPK == vevent.PATIENTEVENTPK
                                   select st);
                var inTable = reportTable.FirstOrDefault();

                if (reportTable.Count() == 0)
                {
                    Model.EVENT_REPORT newEvent = new Model.EVENT_REPORT();
                    newEvent.COMMENTS = "";
                    newEvent.CREATED_ON = vevent.CREATED_ON;
                    newEvent.EVENTNAME = vevent.EVENTNAME;
                    newEvent.EVENTNOTE = vevent.EVENTNOTE;
                    newEvent.EVENTSTAT_DATE = vevent.EVENTSTAT_DATE;
                    newEvent.EVENTSTATUS = vevent.EVENTSTATUS;
                    newEvent.LAST_MODIFIED_DATE = vevent.LAST_MODIFIED_DATE;
                    newEvent.PATIENTEVENTPK = vevent.PATIENTEVENTPK;
                    newEvent.PATIENTID = vevent.PATIENTID;
                    newEvent.PATIENTSTUDYID = vevent.PATIENTSTUDYID;
                    newEvent.STUDYNUMBER = vevent.STUDYNUMBER;
                    newEvent.VISITNAME = vevent.VISITNAME;
                    using (Model.EventModel db = new Model.EventModel())
                    {
                        db.EVENT_REPORT.AddObject(newEvent);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (inTable.EVENTNOTE != vevent.EVENTNOTE)
                    {
                        using (Model.EventModel db = new Model.EventModel())
                        {
                            var tableUpdate = from st in db.EVENT_REPORT where st.PATIENTEVENTPK == vevent.PATIENTEVENTPK select st;
                            var rowUpdate = tableUpdate.First();
                            rowUpdate.COMMENTS = rowUpdate.COMMENTS + " -- " + vevent.LAST_MODIFIED_DATE + " Notes changed, old note : " + rowUpdate.EVENTNOTE;
                            rowUpdate.EVENTNOTE = vevent.EVENTNOTE;
                            rowUpdate.LAST_MODIFIED_DATE = vevent.LAST_MODIFIED_DATE;
                            db.SaveChanges();
                        }
                    }
                    if (inTable.EVENTSTATUS != vevent.EVENTSTATUS)
                    {
                        using (Model.EventModel db = new Model.EventModel())
                        {
                            var tableUpdate = from st in db.EVENT_REPORT where st.PATIENTEVENTPK == vevent.PATIENTEVENTPK select st;
                            var rowUpdate = tableUpdate.First();
                            rowUpdate.COMMENTS = rowUpdate.COMMENTS + " -- " + vevent.LAST_MODIFIED_DATE + " Status changed, old status : " + rowUpdate.EVENTSTATUS;
                            rowUpdate.EVENTSTATUS = vevent.EVENTSTATUS;
                            rowUpdate.LAST_MODIFIED_DATE = vevent.LAST_MODIFIED_DATE;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return true;
        }
    }
}
