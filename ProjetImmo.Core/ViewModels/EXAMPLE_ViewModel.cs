using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Tools;

namespace ProjetImmo.Core.ViewModels
{
    public class TestViewModel : BaseNotifyPropertyChanged
    {

        public void Test()
        {
            /*
             * APPLIQUER LES MODIFICATIONS SUR LA BASE DE DONNEES
             * => DataAccess.AgencyDbContext.Current.SaveChangesAsync
             */

            /*
            DataAccess.AgencyDbContext.Current.Add
            DataAccess.AgencyDbContext.Current.Update
            DataAccess.AgencyDbContext.Current.Remove
            DataAccess.AgencyDbContext.Current.Find
            */
            /*
            DataAccess.AgencyDbContext.Current.Estate.Add
            DataAccess.AgencyDbContext.Current.Estate.Where((e) => e.Referent.ID == 2).ToList();
            DataAccess.AgencyDbContext.Current.Estate.Where((e) => e.Referent.ID == 2).ToArray();
            DataAccess.AgencyDbContext.Current.Estate.All
            DataAccess.AgencyDbContext.Current.Estate.
            */

            /*
            DataAccess.AgencyDbContext.Current.Estate.Where((e) => e.Referent.ID == 2).Include(e => e.Referent).ToList();
            DataAccess.AgencyDbContext.Current.Estate.Where((e) => e.Referent.ID == 2).Include(e => e.Referent).Include([...]).ToList();
            */

        }

    }
}
