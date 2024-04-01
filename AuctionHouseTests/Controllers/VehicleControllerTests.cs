using AuctionHouse.Controllers;
using AuctionHouse.Data;
using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AuctionHouseTests.Controllers
{
    public class VehicleControllerTests
    {
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly VehicleController _sut;
        private readonly IFixture _fixture;

        public VehicleControllerTests()
        {
            CreateInMemoryDbContext();
            _mapper = Substitute.For<IMapper>();
            _fixture = new Fixture();
            _sut = new VehicleController(_dbContext, _mapper);
        }

        private void CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                   .UseInMemoryDatabase("ApplicationDbContext")
                                   .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
    }
}
