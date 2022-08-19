namespace Data.Context
{
    public class DbContextFactory
    {
        private AppDbContext _context;

        public AppDbContext Get()
        {
            if (_context == null)
            {
                _context = new AppDbContext();
            }

            return _context;
        }
    }
}
