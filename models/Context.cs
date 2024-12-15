using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace clinic_api_project.models
{
    public class Context:IdentityDbContext<UserApplication>

    {

        public Context() { }
        public Context(DbContextOptions<Context> options): base(options) 
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
            .UseLazyLoadingProxies(true);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //relation between doc and medicalrecord
            builder.Entity<MedicalRecord>().HasOne(mr => mr.Doctor).WithMany(d => d.DOCRecords).HasForeignKey(Mr => Mr.DoctorID).OnDelete(DeleteBehavior.NoAction);
            //relation between patient and medicalrecord
            builder.Entity<MedicalRecord>().HasOne(mr => mr.patient).WithOne(p => p.PatientRecord).HasForeignKey<MedicalRecord>(pr => pr.patientID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<apointment>().HasOne(mr => mr.patient).WithMany(p => p.Apointments).HasForeignKey(pr => pr.patientID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<apointment>().HasOne(mr => mr.Doctor).WithMany(d => d.PationApointments).HasForeignKey(d => d.DoctorID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserApplication>()
                      .HasOne(u => u.address)
                      .WithOne(a => a.UserApplication)
                      .HasForeignKey<Address>(a => a.UserApplicationID);
        }
        public DbSet<apointment> apointments { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<MedicalRecord> medicicals { get; set;}
        public DbSet<UserApplication> patientAndDoctors { get; set; }

    }
}
