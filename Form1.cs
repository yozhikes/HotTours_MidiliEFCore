using HotTours_Midili;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotTours_Midili
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DataSource = ReadDb();
            Strips();
        }

        private void ЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "Данная программа позволяет вести реестр горячих туров",
            "Информация",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
        }

        private void Tsdob_Click(object sender, EventArgs e)
        {
            var infform = new Form2
            {
                Text = "Добавление товара"
            };
            if (infform.ShowDialog() == DialogResult.OK)
            {
                infform.tour.Id = Guid.NewGuid();
                using (ApplicationContext db = new ApplicationContext(DataBaseHelper.Options()))
                {
                    db.Tours.Add(infform.tour);
                    db.SaveChanges();
                }
                dataGridView1.DataSource = ReadDb();
                Strips();
            }
        }

        private void Tsizm_Click(object sender, EventArgs e)
        {
            var id = (Tour)dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].DataBoundItem;
            var infform = new Form2(id)
            {
                Text = "Изменение товара"
            };
            if (infform.ShowDialog() == DialogResult.OK)
            {
                id.Direction = infform.tour.Direction;
                id.Wifi = infform.tour.Wifi;
                id.Sum=infform.tour.Sum;
                id.Qty=infform.tour.Qty;
                id.Dop=infform.tour.Dop;
                id.Nights=infform.tour.Nights;
                id.Date=infform.tour.Date;
                UpDateDb(id);
                dataGridView1.DataSource = ReadDb();
                Strips();
            }
        }

        private void Tsud_Click(object sender, EventArgs e)
        {
            var id = (Tour)dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].DataBoundItem;
            RemoveDb(id);    
            dataGridView1.DataSource = ReadDb();
            Strips();
            if (dataGridView1.DataSource == null)
            {
                tsizm.Enabled = false;
                tsud.Enabled = false;
                изменитьToolStripMenuItem.Enabled=false;
                удалитьToolStripMenuItem.Enabled=false;
            }
        }

        private void ДобавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tsdob_Click(sender, e);
        }

        private void ИзменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tsizm_Click(sender, e);
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tsud_Click(sender, e);
        }
        public void Strips()
        {
            using (ApplicationContext db = new ApplicationContext(DataBaseHelper.Options()))
            {
                qtytours.Text = db.Tours.ToList().Count().ToString();
                sumtours.Text = db.Tours.ToList().Sum(x=>x.Sum).ToString();
                qtydops.Text = db.Tours.ToList().Count(x=>x.Dop!=0).ToString();
                sumdop.Text = db.Tours.ToList().Sum(x => x.Dop).ToString();
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            tsizm.Enabled = tsud.Enabled = dataGridView1.SelectedRows.Count > 0;
            удалитьToolStripMenuItem.Enabled = изменитьToolStripMenuItem.Enabled = dataGridView1.SelectedRows.Count > 0;

        }
        private static List<Tour> ReadDb()
        {
            using (ApplicationContext db = new ApplicationContext(DataBaseHelper.Options()))
            {
                return db.Tours.ToList();
            }
        }

        private static void UpDateDb(Tour tour)
        {
            using (ApplicationContext db = new ApplicationContext(DataBaseHelper.Options()))
            {
                var tourdb = db.Tours.FirstOrDefault(x => x.Id == tour.Id);
                if (tourdb != null)
                {
                    tourdb.Direction = tour.Direction;
                    tourdb.Price = tour.Price;
                    tourdb.Sum = tour.Sum;
                    tourdb.Qty = tour.Qty;
                    tourdb.Wifi = tour.Wifi;
                    tourdb.Date = tour.Date;
                    tourdb.Dop = tour.Dop;
                    tourdb.Nights = tour.Nights;
                    db.SaveChanges();
                }

            }
        }
        private static void RemoveDb(Tour tour)
        {
            using (ApplicationContext db = new ApplicationContext(DataBaseHelper.Options()))
            {
                var tours = db.Tours.FirstOrDefault(u => u.Id == tour.Id);
                if (tours != null)
                {
                    db.Tours.Remove(tours);
                    db.SaveChanges();
                }
            }
        }

    }
}
