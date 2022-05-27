namespace MyShuttle.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyShuttleContext context;

        public CustomerRepository(MyShuttleContext dbcontext)
        {
            context = dbcontext;
        }
    }
}