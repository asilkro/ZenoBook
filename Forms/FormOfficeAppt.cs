﻿using log4net;
using RepoDb;
using ZenoBook.Classes;
using ZenoBook.DataManipulation;

namespace ZenoBook.Forms;

public partial class FormOfficeAppt : Form
{
    private FormAppointment? form1;
    private readonly OfficeAppointment officeAppt;

    public FormOfficeAppt(Appointment appt)
    {
        InitializeComponent();
        officeAppt = new OfficeAppointment(appt.AppointmentId, appt.CustomerId, appt.StaffId, -1, appt.ServiceID,
            appt.Start,
            appt.End, false);
        var cx = ReturnCustomer(appt.CustomerId.ToString());
        if (cx != null)
        {
            officeAppt.OfficeId = (cx.PreferredOffice);
        }
        submitBtn.Enabled = false;
    }

    #region Other Methods

    private void FillOfficeFromSearch(Office? address)
    {
        if (address != null)
        {
            officeNameTB.Text = address.OfficeName;
            officeCityTB.Text = address.City;
            officeCountryTB.Text = address.Country;
            officeStateTB.Text = address.State;
            officeIdTB.Text = address.OfficeId.ToString();
        }

    }

    #endregion

    #region Event Handlers

    private void cancelBtn_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void validateBtn_Click(object sender, EventArgs e)
    {
        officeAppt.OfficeId = int.Parse(officeIdTB.Text);
        officeAppt.InHomeService = false;
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        if (OfficeAppointment.InsertOfficeAppt(officeAppt))
        {
            Close();
            form1?.Close();
        }
    }

    #endregion


    #region SQL

    public void ReturnOfficeAddress(string searchTerm)
    {
        using var connection = new Builder().Connect();
        try
        {
            if (int.TryParse(searchTerm, out int i))
            {
                var officeAddress = connection.Query<Office>("[zth].[office]", e => e.OfficeId == i);
                FillOfficeFromSearch(officeAddress.GetEnumerator().Current);
            }
        }
        catch (Exception e)
        {
            LogManager.GetLogger("LoggingRepo").Warn(e, e);
        }
    }

    private static Customer? ReturnCustomer(string searchTerm)
    {
        using var connection = new Builder().Connect();
        var space = ' ';
        var atSign = '@';

        if (searchTerm.Contains(space))
        {
            var searchTerms = searchTerm.Split(' ', 2);
            var first = searchTerms[0];
            var last = searchTerms[1];
            var sCustomer = connection.Query<Customer>("[zth].[customer]", e => e.First == first && e.Last == last)
                .FirstOrDefault();
            return sCustomer;
        }

        if (searchTerm.Contains(atSign))
        {
            var aCustomer = connection.Query<Customer>("[zth].[customer]", e => e.Email == searchTerm)
                .FirstOrDefault();
            return aCustomer;
        }

        if (int.TryParse(searchTerm, out var i))
        {
            var iCustomer = connection.Query<Customer>("[zth].[customer]", e => e.Customer_Id == i)
                .FirstOrDefault();
            return iCustomer;
        }

        return null;
    }

    #endregion
}