﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;
using ZenoBook.DataManipulation;

namespace ZenoBook.Classes
{
    internal class Staff
    {
        #region Properties / Fields

        public int StaffId { get; set; } // PK, auto_increment, not_null
        public string Name { get; set; } // PK, VARCHAR(32), not_null
        public int Phone { get; set; } // PK, not_null
        public string Email { get; set; } // VARCHAR(32)
        public int OfficeId { get; set; } // FK, Office.OfficeId
        public int LoginId { get; set; } // FK, Users.UserId
        // TODO: Do I want to use this?

        #endregion

        #region Constructors

        public Staff()
        {

        }

        public Staff(int staffId, string name, int phone, string email, int officeId, int loginId)
        {
            StaffId = staffId;
            Name = name;
            Phone = phone;
            Email = email;
            OfficeId = officeId;
            LoginId = loginId;
        }

        #endregion

        #region SQL

        public bool InsertStaff(Staff staff)
        {
            using (var connection = new Builder().Connection())
                try
                {
                    var id = connection.Insert("[zth].[staff]", entity: staff);
                    MessageBox.Show("Staff id " + id + " created.", "Staff Created");
                    return true;
                }
                catch (Exception e)
                {
                    LogManager.GetLogger("LoggingRepo").Warn(e, e);
                    return false;
                }

        }

        public bool DeleteStaff(int staffId)
        {
            using (var connection = new Builder().Connection())
                try
                {
                    var id = connection.Delete("[zth].[staff]", staffId);
                    MessageBox.Show("Staff id " + id + " removed.", "Customer Removed");
                    return true;
                }
                catch (Exception e)
                {
                    LogManager.GetLogger("LoggingRepo").Warn(e, e);
                    return false;
                }
        }

        public bool UpdateStaff(Staff staff)
        {
            using (var connection = new Builder().Connection())
                try
                {
                    {
                        var updatedStaff = connection.Update("[zth].[staff]", entity: staff);
                        MessageBox.Show("Staff id " + updatedStaff + " updated.", "Customer Updated");
                    }
                    return true;
                }
                catch (Exception e)
                {
                    LogManager.GetLogger("LoggingRepo").Warn(e, e);
                    return false;
                }
        }

        #endregion
    }
}
