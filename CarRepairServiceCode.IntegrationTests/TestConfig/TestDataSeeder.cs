using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Models;
using System;
using CarRepairServiceCode.Helper;

namespace CarRepairServiceCode.IntegrationTests.TestConfig
{
    public class TestDataSeeder
    {
        private readonly CarRepairServiceDB_Context _context;
        private const int ExistingEmpIdManager = 1;
        private const int ExistingEmpIdMachinist = 2;

        private const string Manager = "Manager";
        private const string ManagerFirstName = "John";
        private const string ManagerLastName = "Devidson";
        private const string Mechanic = "Mechanic";
        private const string Maid = "Maid";
        private const string Accountant = "Accountant";
        private const string EmpPositionEntityName = "emp_position";
        private const string ManagerMail = "johndevidson@gmail.com";
        private const string MachinistMail = "bobdownson@gmail.com";
        private const string ManagerPassword = "john12345";
        private const string MachinistPassword = "bob12345";

        private static readonly EmpPosition _empPositionManager = new EmpPosition { PositionId = ExistingEmpIdManager, PositionName = Manager, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionMachinist = new EmpPosition { PositionId = ExistingEmpIdMachinist, PositionName = Mechanic, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionMaid = new EmpPosition { PositionId = 3, PositionName = Maid, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionAccountant = new EmpPosition { PositionId = 4, PositionName = Accountant, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };

        private static readonly Employee _employeeManager = new Employee
        {
            EmployeeId = ExistingEmpIdManager,
            FirstName = ManagerFirstName,
            LastName = ManagerLastName,
            PositionId = ExistingEmpIdManager,
            IsActive = true,
            EmpLogin = ManagerMail,
            EmpPassword = SecurePasswordHasherHelper.Hash(ManagerPassword),
            CreatedById = ExistingEmpIdManager,
            CreatedDate = DateTime.UtcNow
        };
        private static readonly Employee _employeeMachinist = new Employee
        {
            EmployeeId = ExistingEmpIdMachinist,
            FirstName = ManagerFirstName,
            LastName = ManagerLastName,
            PositionId = ExistingEmpIdMachinist,
            IsActive = true,
            EmpLogin = MachinistMail,
            EmpPassword = SecurePasswordHasherHelper.Hash(MachinistPassword),
            CreatedById = ExistingEmpIdMachinist,
            CreatedDate = DateTime.UtcNow
        };

        private static readonly Permissions _permissionsManager = new Permissions
        {
            PermissionId = 1,
            PositionId = 1,
            EntityForAction = EmpPositionEntityName,
            CreateEntry = true,
            ReadEntry = true,
            UpdateEntry = true,
            DeleteEntry = true,
            CreatedById = 1,
            CreatedDate = DateTime.UtcNow
        };
        private static readonly Permissions _permissionsMachinist = new Permissions
        {
            PermissionId = 2,
            PositionId = 2,
            EntityForAction = EmpPositionEntityName,
            CreateEntry = false,
            ReadEntry = false,
            UpdateEntry = false,
            DeleteEntry = false,
            CreatedById = 1,
            CreatedDate = DateTime.UtcNow
        };

        public TestDataSeeder(CarRepairServiceDB_Context context)
        {
            _context = context;
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void SeedToDoItems()
        {
            _context.EmpPositions.AddRangeAsync(_empPositionManager, _empPositionMachinist, _empPositionMaid, _empPositionAccountant);
            _context.Permissions.AddRangeAsync(_permissionsManager, _permissionsMachinist);
            _context.Employees.AddRangeAsync(_employeeManager, _employeeMachinist);
            _context.SaveChanges();
        }
    }
}
