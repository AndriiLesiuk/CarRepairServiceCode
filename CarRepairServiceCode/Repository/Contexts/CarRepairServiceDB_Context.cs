using CarRepairServiceCode.Repository.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CarRepairServiceCode.Repository.Contexts
{
    public partial class CarRepairServiceDB_Context : DbContext
    {
        public CarRepairServiceDB_Context()
        {
        }

        public CarRepairServiceDB_Context(DbContextOptions<CarRepairServiceDB_Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarOrder> CarOrders { get; set; }
        public virtual DbSet<CarOrderDetail> CarOrderDetails { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<EmpPosition> EmpPositions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<TaskCatalog> TaskCatalogs { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United Kingdom.1252");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("car");

                entity.Property(e => e.CarId)
                    .HasColumnName("car_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CarName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("car_name");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.Colour)
                    .HasMaxLength(30)
                    .HasColumnName("colour");

                entity.Property(e => e.VinNumber)
                    .HasMaxLength(20)
                    .HasColumnName("vin_number");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_client_car");
            });

            modelBuilder.Entity<CarOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("carorder_pkey");

                entity.ToTable("car_order");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.OrderAmount)
                    .HasPrecision(18, 2)
                    .HasColumnName("order_amount")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderComments)
                    .HasMaxLength(500)
                    .HasColumnName("order_comments");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarOrders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_carorder_carid");
            });

            modelBuilder.Entity<CarOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsId)
                    .HasName("carorderdetails_pkey");

                entity.ToTable("car_order_details");

                entity.Property(e => e.OrderDetailsId)
                    .HasColumnName("order_details_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.TaskId).HasColumnName("task_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.CarOrderDetails)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_carorderdetails_employee");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.CarOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_carorderdetails_carorder");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.CarOrderDetails)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_carorderdetails_taskcatalog");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("last_name");

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(20)
                    .HasColumnName("mobile_number");

                entity.Property(e => e.Wallet)
                    .HasPrecision(18, 2)
                    .HasColumnName("wallet")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            });

            modelBuilder.Entity<EmpPosition>(entity =>
            {
                entity.HasKey(e => e.PositionId)
                    .HasName("empposition_pkey");

                entity.ToTable("emp_position");

                entity.Property(e => e.PositionId)
                    .HasColumnName("position_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("position_name");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.EmpLogin)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("emp_login");

                entity.Property(e => e.EmpPassword)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("emp_password");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("last_name");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_position");
            });

            modelBuilder.Entity<TaskCatalog>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("taskcatalog_pkey");

                entity.ToTable("task_catalog");

                entity.Property(e => e.TaskId)
                    .HasColumnName("task_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.TaskDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("task_description");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("task_name");

                entity.Property(e => e.TaskPrice)
                    .HasPrecision(18, 2)
                    .HasColumnName("task_price")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            });

            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(e => new { e.PositionId, e.EntityForAction })
                    .HasName("permission_pkey");

                entity.ToTable("permissions");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.EntityForAction)
                    .HasMaxLength(40)
                    .HasColumnName("entity_for_action");

                entity.Property(e => e.CreateEntry)
                    .HasColumnName("create_entry")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeleteEntry)
                    .HasColumnName("delete_entry")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PermissionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("permission_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ReadEntry)
                    .HasColumnName("read_entry")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.UpdateEntry)
                    .HasColumnName("update_entry")
                    .HasDefaultValueSql("false");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("fk_permissions_emp_position");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
