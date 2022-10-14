using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Quartz;
using Quartz.Impl;

/// <summary>
/// Summary description for globalAsax
/// </summary>
public class globalAsax : System.Web.HttpApplication {
	void startQuartzJob(Type JobType, string Name, int IntervalInMinutes, DateTimeOffset runTime, ref IScheduler sched, int RepeatCount = 0, List<KeyValuePair<string, object>> Options = null) {
		var job = JobBuilder.Create(JobType).WithIdentity(Name).Build();

		ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
									.WithIdentity(Name + ".trigger")
									.StartAt(runTime)
									.WithSimpleSchedule(
										x => x.WithIntervalInMinutes(IntervalInMinutes).RepeatForever()
									)
									.Build();

		if (Options != null && Options.Count > 0)
			Options.ForEach(n => job.JobDataMap.Add(n.Key, n.Value));

		if (RepeatCount > 0)
			trigger.RepeatCount = RepeatCount;

		sched.ScheduleJob(job, trigger);
	}

	void Application_Start(object sender, EventArgs e) {
		
		log4net.Config.XmlConfigurator.Configure();

		DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow).AddMinutes(1);
		DateTimeOffset midnight = DateTime.Now.Date;

		ISchedulerFactory sf = new StdSchedulerFactory();
		IScheduler sched = sf.GetScheduler();

		startQuartzJob(typeof(jobLatLong), "latlong", (60 * 24 * 7), runTime, ref sched);

		sched.Start();

		PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		object o = p.GetValue(null, null);
		FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
		object monitor = f.GetValue(o);
		MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
		m.Invoke(monitor, new object[] { });

	}

	void Application_End(object sender, EventArgs e) {
		//  Code that runs on application shutdown

	}

	void Application_Error(object sender, EventArgs e) {
		// Code that runs when an unhandled error occurs

	}

	void Session_Start(object sender, EventArgs e) {
		// Code that runs when a new session is started

	}

	void Session_End(object sender, EventArgs e) {
		// Code that runs when a session ends. 
		// Note: The Session_End event is raised only when the sessionstate mode
		// is set to InProc in the Web.config file. If session mode is set to StateServer 
		// or SQLServer, the event is not raised.

	}

    internal protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // Get objects.
        HttpContext context = base.Context;
        HttpResponse response = context.Response;
        HttpRequest request = context.Request;

        var url = request.Url.AbsoluteUri.ToLower();
        if (url.Contains(".well-known"))
        {
            return;
        }
        if (url.StartsWith("http://") && !url.StartsWith("http://localhost"))
        {
            response.Redirect(url.Replace("http://", "https://"));
        }

        // Complete.
        //base.CompleteRequest();
    }

}