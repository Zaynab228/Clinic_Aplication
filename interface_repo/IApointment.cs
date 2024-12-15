using Castle.DynamicProxy.Generators;
using clinic_api_project.models;

namespace clinic_api_project.interface_repo
{
    public interface IApointment
    {
        ///crud operation
        List<apointment> GetAll();
        apointment GetById(int id);
        void update (int id,apointment apointment);
        void Book (apointment apointment);
        void cancell (int id);

    }
}
