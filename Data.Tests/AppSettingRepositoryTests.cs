using Data.Connect;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data.Tests
{
    public class AppSettingRepositoryTests
    {
        private AppSettingRepository _testAppSettingRepository;
        private ApplicationSettingContext _context;

        private DbContextOptions<ApplicationSettingContext> _contextOptions;


        [SetUp]
        public void Setup()
        {

            _contextOptions = new DbContextOptionsBuilder<ApplicationSettingContext>()
            .UseInMemoryDatabase("AppSettingRepositoryTest-Memory")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            _context = new ApplicationSettingContext(_contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _testAppSettingRepository = new AppSettingRepository(_context);
        }

        [Test]
        public void AddNewHostTest()
        {
            //given
            string testHostname = "google.ru";
            HistoryHost testItem = new HistoryHost() { Hostname = testHostname };
            _testAppSettingRepository.AddNewHost(testItem);

            //when
            var act = _context.History.ToList();

            //then
            Assert.NotNull(act);
            Assert.That(act.Count, Is.EqualTo(1));
            Assert.That(act.First().Hostname, Is.EqualTo(testHostname));
        }

        [Test]
        public void GetLastFiveHostnameTest()
        {
            //given
            string[] testMassHostname = new string[] { "google.ru", "ya.ru", "github.com", "overcoder.net", "youtube.com" };

            foreach (var test in testMassHostname)
            {
                HistoryHost testItem = new HistoryHost() { Hostname = test };
                _testAppSettingRepository.AddNewHost(testItem);
            }

            //when
            var act = _testAppSettingRepository.GetLastFiveHostname();
            Array.Reverse(testMassHostname);

            //then
            Assert.NotNull(act);
            Assert.That(act.Count, Is.EqualTo(5));
            for (var i = 0; i < act.Count; i++)
            {
                Assert.That(act[i].Hostname, Is.EqualTo(testMassHostname[i]));
            }
        }

        [Test]
        public void ClearAllTableTest()
        {
            //given
            string[] testMassHostname = new string[] { "google.ru", "ya.ru", "github.com", "overcoder.net", "youtube.com" };

            foreach (var test in testMassHostname)
            {
                HistoryHost testItem = new HistoryHost() { Hostname = test };
                _testAppSettingRepository.AddNewHost(testItem);
            }

            //when
            var resultAct = _testAppSettingRepository.ClearAllTable();
            var act = _context.History.ToList();

            //then
            Assert.NotNull(resultAct);
            Assert.IsTrue(resultAct);
            Assert.That(act.Count, Is.EqualTo(0));
        }
    }
}
