using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityCRUD
{
    public partial class Form1 : Form
    {
        MyContext  _context = new MyContext();
        public Form1()
        {
            InitializeComponent();
            gridView1.DataController.AllowIEnumerableDetails = true;
            FillData();
            _context.Persons.Load();
            gridControl1.DataSource = _context.Persons.Local.ToBindingList();
        }

        private static void FillData()
        {
            using (var _cnt = new MyContext())
            {
                _cnt.Configuration.AutoDetectChangesEnabled = false;
                if (_cnt.Persons.Any())
                {
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    Person newPerson = new Person { FirstName = "First Name" + i, LastName = "Last Name" + i };
                    Adress adr = new Adress();
                    adr.City = "City" + i;
                    newPerson.Adresses.Add(adr);
                    _cnt.Persons.Add(newPerson);
                }
                _cnt.SaveChanges();
            }
        }
        private void previewChanges()
        {
            var changeSet = _context.ChangeTracker.Entries();
            int deletedCount = 0;
            int changedCount = 0;
            int addedCount = 0;
            if (changeSet != null)
                foreach (var dbEntityEntry in changeSet)
                {
                        switch (dbEntityEntry.State)
                        {
                            case EntityState.Deleted:
                                deletedCount++;
                            // process
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Added:
                                addedCount++;
                                // process
                                break;
                            case EntityState.Modified:
                                changedCount++;
                                break;
                        }

                }
            string outp = string.Format("{0} added; {1} deleted; {2} modified",addedCount,deletedCount,changedCount);
            MessageBox.Show(outp);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            previewChanges();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            _context.SaveChanges();
        }
    }
    public class MyContext : DbContext
    {
        public MyContext()
            : base(@"Data Source=(localdb)\v11.0;Initial Catalog=TestContext;Integrated Security=True;MultipleActiveResultSets=True;Application Name=TestOfMasterDetail3")

        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Adress> Adresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Adress>().Property(a => a.PersonID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Adress>().HasRequired(a => a.Person).WithMany(p => p.Adresses).HasForeignKey(a => a.PersonID);
        }
    }
    public class Person
    {
        public Person()
        {
            Adresses = new List<Adress>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
    }
    public class Adress
    {
        public string City { get; set; }
        public int ID { get; set; }
        public int? PersonID { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        public Adress()
        {

        }
    }
}
