using clinic_api_project.models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace clinic_api_project.interface_repo
{
    public class ApointmentRepo: IApointment
    {
        private readonly Context context;

        public ApointmentRepo(Context context)
        {
            this.context = context;
        }

        public void cancell(int id)
        {
           apointment apoint = context.apointments.Find(id);
            if (apoint!= null)
            {
                context.apointments.Remove(apoint);
                context.SaveChanges();
            }
        }

        public List<apointment> GetAll()
        {
            List<apointment> list=context.apointments.ToList();
            return list;
        }

        public apointment  GetById(int id)
        {
            apointment apoint = context.apointments.Find(id);
            return apoint;
        }

      public void update(int id, apointment apointment)
        {
            apointment old = GetById(id);
            if (old != null)
            {
                old.Id=apointment.Id;
                old.DateApointment= apointment.DateApointment;
                old.Status=apointment.Status;
                old.Notes = apointment.Notes;
                context.SaveChanges(true);
            }
        }

       
        public void Book(apointment apointment)
        {
            
            apointment ap = new apointment();
            ap.Id=apointment.Id;
            ap.DateApointment = apointment.DateApointment;
            ap.Status="pending";
            ap.Notes=apointment.Notes;
            context.apointments.Add(ap);
            context.SaveChanges(true);
        }


    }
}
