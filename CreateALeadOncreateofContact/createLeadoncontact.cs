using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace CreateALeadOncreateofContact
{
    public class createLeadoncontact : IPlugin

    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //tracing service essential for debugging
            ITracingService tracingservice = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingservice.Trace("Tracing Service Invoked");

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            //Provides programmatic access to the metadata and data for an organization.
            IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = organizationServiceFactory.CreateOrganizationService(context.UserId);

            try
            {
                int step = 1;

                if(context.InputParameters.Contains("Target") & context.InputParameters["Target"] is Entity)
                {

                    Guid LeadID;


                    Entity contact = (Entity)context.InputParameters["Target"];

                    if (contact.LogicalName != "account")
                        return;

                    string firstName = contact["firstname"].ToString();
                    string lastName = contact["lastname"].ToString();
                    string phone = contact["mobilephone"].ToString();
                    string email = contact["emailaddress1"].ToString();


                    step = 2;

                    Entity lead = new Entity();
                    lead["subject"] = "Lead Created from Contact";
                    lead["firstname"] = firstName;
                    lead["lastname"] = lastName;
                    lead["parentcontactid"] = new EntityReference("lead", contact.Id);
                    lead["mobilephone"] = phone;
                    lead["emailaddress1"] = email;
                    LeadID = service.Create(lead);

                    tracingservice.Trace("Lead Created with GUID" + LeadID);


                }
                else

                {
                    tracingservice.Trace("Lead was not created");

                }



            }

            catch (Exception ex)
            {
                tracingservice.Trace("{0}", ex.ToString());
                throw;
            }


        }
    }
}
 