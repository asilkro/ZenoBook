﻿using System;
using System.Data;
using MySqlConnector;
using RepoDb;
using RepoDb.Extensions;
using ZenoBook.Classes;
using ZenoBook.DataManipulation;

namespace ZenoBook.Forms;

public partial class Main : Form
{
    public Main()
    {
        InitializeComponent();
        DGVExtensions.populateDGV(apptsDataGridView, "appointment");
        DGVExtensions.populateDGV(cxDataGridView, "customer");
    }


    #region Event Handlers

    private void logoutBtn_Click(object sender, EventArgs e)
    {
        var newLogin = new Login();
        newLogin.ShowDialog();
        this.Close();
    }

    private void CreateApptBtn_Click(object sender, EventArgs e)
    {
        var apptForm = new FormAppointment();
        apptForm.ShowDialog();
    }

    private void UpdateApptBtn_Click(object sender, EventArgs e)
    {
        var selectedRow = apptsDataGridView.CurrentRow;
        if (selectedRow != null)
        {
            var row = apptsDataGridView.Rows.IndexOf(selectedRow);
            var selected = (int) apptsDataGridView["appointment_id", row].Value;
            using var connection = new Builder().Connect();
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var uAppt = UnifiedApptData.GetAppointment(selected);

                var apptFill = new Appointment();
                if (uAppt != null)
                {
                    apptFill.appointment_id = uAppt.appointment_id;
                    apptFill.customer_id = uAppt.customer_id;
                    apptFill.service_id = uAppt.service_id;
                    apptFill.staff_id = uAppt.staff_id;
                    apptFill.start = uAppt.start;
                    apptFill.end = uAppt.end;

                    switch (uAppt.office_id != 0 && uAppt.inhomeservice != 1)
                    {
                        case true:
                            //This should be the case for a OfficeAppt
                            OfficeAppointment officeAppt = new OfficeAppointment()
                            {
                                appointment_id = uAppt.appointment_id,
                                customer_id = uAppt.customer_id,
                                staff_id = uAppt.staff_id,
                                office_id = uAppt.office_id,
                                service_id = uAppt.service_id,
                                start = uAppt.start,
                                end = uAppt.end,
                                inhomeservice = uAppt.inhomeservice,
                            };
                            var apptOForm = new FormAppointment(apptFill);
                            var officeForm = new FormOfficeAppt(officeAppt);
                            apptOForm.ShowDialog();
                            officeForm.ShowDialog();
                            break;
                        case false:
                            HomeAppointment homeAppt = new HomeAppointment()
                            {
                                appointment_id = uAppt.appointment_id,
                                customer_id = uAppt.customer_id,
                                staff_id = uAppt.staff_id,
                                service_id = uAppt.service_id,
                                start = uAppt.start,
                                end = uAppt.end,
                                service_address_id = uAppt.service_address_id,
                                inhomeservice = uAppt.inhomeservice,
                            };
                            var apptHForm = new FormAppointment(apptFill);
                            var homeForm = new FormHomeAppt(homeAppt);
                            apptHForm.ShowDialog();
                            homeForm.ShowDialog();
                            break;
                    }

                    connection.Close();
                }
            }
        }
    }

    private void RemoveApptBtn_Click(object sender, EventArgs e)
    {
        var selectedRow = apptsDataGridView.CurrentRow;
        if (selectedRow != null)
        {
            var row = apptsDataGridView.Rows.IndexOf(selectedRow);
            var selected = (int) apptsDataGridView["appointment_id", row].Value;
            using (var connection = new Builder().Connect())
            {
                var result = UnifiedApptData.RemoveAppointment(selected);
                if (result)
                {
                    DGVExtensions.populateDGV(apptsDataGridView, "appointment");
                }
            }
        }
    }

    private void CxCreateBtn_Click(object sender, EventArgs e)
    {
        var cxForm = new FormCustomer();
        cxForm.ShowDialog();
    }

    private void UpdateCxBtn_Click(object sender, EventArgs e)
    {
        var selectedRow = cxDataGridView.CurrentRow;
        if (selectedRow != null)
        {
            var row = cxDataGridView.Rows.IndexOf(selectedRow);
            var selected = (int) cxDataGridView["customer_id", row].Value;
            {
                using (var connection = new Builder().Connect())
                {
                    var customer = connection.Query<Customer>("customer", selected).FirstOrDefault();
                    if (customer != null)
                    {
                        var cxForm = new FormCustomer(customer);
                        cxForm.ShowDialog();
                    }
                }
            }
        }
    }

    private void RemoveCxBtn_Click(object sender, EventArgs e)
    {
        var selectedRow = cxDataGridView.CurrentRow;
        var row = cxDataGridView.Rows.IndexOf(selectedRow);
        int selected = (int) cxDataGridView["customer_id", row].Value;
        if (selectedRow != null)
        {
            var result = Customer.DeleteCustomer(selected);
            if (result)
            {
                DGVExtensions.populateDGV(cxDataGridView, "customer");
            }
        }
    }

    private void apptSearchBtn_Click(object sender, EventArgs e)
    {
        DGVExtensions.searchDGV(apptsDataGridView, "appointment", apptSearchTB.Text);
    }

    private void cxSearchBtn_Click(object sender, EventArgs e)
    {
        DGVExtensions.searchDGV(cxDataGridView, "customer", cxSearchTB.Text);
    }

    #endregion
}