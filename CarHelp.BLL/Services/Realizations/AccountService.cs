using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITestRepository testRepository;
        private readonly IRepository<User> userRepository;

        public AccountService(ITestRepository testRepository, IRepository<User> userRepository)
        {
            this.testRepository = testRepository;
            this.userRepository = userRepository;
        }

        public void GenerateSmsCode()
        {
            testRepository.TestGenericMethod();

            testRepository.SomeComplicatedMethod();

            userRepository.TestGenericMethod();
        }
    }
}
