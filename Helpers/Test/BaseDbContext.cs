using Microsoft.EntityFrameworkCore;

namespace Test;

public class BaseDbContext(DbContextOptions options) : DbContext(options)
{

}