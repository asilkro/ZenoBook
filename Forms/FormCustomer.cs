﻿using System;
using System.Runtime.InteropServices;
using Accessibility;
using ZenoBook.Classes;
using ZenoBook.DataManipulation;

namespace ZenoBook.Forms;

public partial class FormCustomer : Form
{
    private Customer? _existingCx;

    public FormCustomer()
    {
        InitializeComponent();
        saveBtn.Enabled = false;
        saveBtn.Visible = false;
        // cxIdTB.Visible = false; // Simpler user experience without this visible when a Cx hasn't been created yet,
        // as it might not be known what the ID is.
        cxIdTB.Text = Helpers.AutoIncrementId("customer"); //TODO: RELIES ON AUTOINCREMENT WHICH MAY BE FLAKY
    }

    public FormCustomer(Customer customer)
    {
        _existingCx = customer;
        InitializeComponent();
        tbFirstName.Text = customer.first;
        tbLastName.Text = customer.last;
        tbPhone.Text = customer.phone;
        tbEmail.Text = customer.email;
        tbOffice.Text = customer.preferred_office.ToString();
        cxIdTB.Text = customer.customer_id.ToString();

        saveBtn.Enabled = false;
        saveBtn.Visible = false;
    }


    private void validateBtn_Click(object sender, EventArgs e)
    {
        var isThereAProblem = true;
        for (var index = 0; index < Controls.Count; index++)
        {
            var c = Controls[index];
            if (c is TextBox)
                if (!string.IsNullOrWhiteSpace(c.Text) &&
                    !string.IsNullOrEmpty(c.Text))
                {
                    isThereAProblem = false;
                    break;
                }
        }

        if (int.TryParse(cxIdTB.Text, out _))
        {
            isThereAProblem = false;
        }

        if (int.TryParse(tbPhone.Text, out _))
        {
            isThereAProblem = false;
        }

        if (isThereAProblem) return;
        validateBtn.Enabled = false;
        validateBtn.Visible = false;
        saveBtn.Enabled = true;
        saveBtn.Visible = true;
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
        var result = _existingCx switch
        {
            null => Customer.InsertCustomer(new Customer(customer_Id: int.Parse(cxIdTB.Text), first: tbFirstName.Text,
                last: tbLastName.Text, phone: tbPhone.Text, email: tbEmail.Text, int.Parse(tbOffice.Text))),
            not null => Customer.UpdateCustomer(new Customer(customer_Id: int.Parse(cxIdTB.Text),
                first: tbFirstName.Text, last: tbLastName.Text, phone: tbPhone.Text, email: tbEmail.Text,
                int.Parse(tbOffice.Text)))
        };

        if (result)
        {
            this.Close();
        }
    }
}