/**
 * 2015-02-24 M.Horigome
 * Service sample
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

// add
using System.ServiceModel.Web;

namespace CsRESTWinSvc
{
    /// <summary>
    /// Service Class
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        WebServiceHost serviceHost;
       
        // initialize
        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start Service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try {
                serviceHost = new WebServiceHost(typeof(ImagingService));
                serviceHost.Open();
                EventLog.WriteEntry("! Service Start", EventLogEntryType.Information);
            } 
            catch(Exception e) 
            {
                EventLog.WriteEntry("! Service Start ERR. " + e.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Stop Service
        /// </summary>
        protected override void OnStop()
        {
            try { 
                if (serviceHost != null)
                {
                    serviceHost.Close();
                }
                EventLog.WriteEntry("! Service Stop", EventLogEntryType.Information);
            }
            catch(Exception e)
            {
                EventLog.WriteEntry("! Service Stop ERR. " + e.Message, EventLogEntryType.Error);
            }
        }
    }
}
